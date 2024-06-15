// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Source;
using CodeBuilder.Core.Variable;
using Fireasy.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace CodeBuilder.Swagger
{
    public class SwaggerParser
    {
        private readonly IServiceProvider _serviceProvider;

        public SwaggerParser(IServiceProvider serviceProvider) 
        {
            _serviceProvider = serviceProvider;
        }

        public Dictionary<string, List<TNode>> Parse(string json)
        {
            var obj = JsonSerializer.Deserialize<JsonObject>(json);
            if (obj["openapi"]?.GetValue<string>().StartsWith("3.") == true)
            {
                return ParseV3(obj);
            }
            else if (obj["swagger"]?.GetValue<string>().StartsWith("2.") == true)
            {
                return ParseV2(obj);
            }

            return null;
        }

        private Dictionary<string, List<TNode>> ParseV3(JsonObject obj)
        {
            var prefixLength = "#/components/schemas/".Length;
            var paths = obj["paths"] as IDictionary<string, JsonNode>;
            var schemas = obj["components"]["schemas"] as IDictionary<string, JsonNode>;
            return Parse(paths, schemas, prefixLength);
        }

        private Dictionary<string, List<TNode>> ParseV2(JsonObject obj)
        {
            var prefixLength = "#/definitions/".Length;
            var paths = obj["paths"] as IDictionary<string, JsonNode>;
            var definitions = obj["definitions"] as IDictionary<string, JsonNode>;
            return Parse(paths, definitions, prefixLength);
        }

        private Dictionary<string, List<TNode>> Parse(IDictionary<string, JsonNode> paths, IDictionary<string, JsonNode> schemas, int prefixLength)
        {
            var result = new Dictionary<string, List<TNode>>();
            foreach (var path in paths.Keys)
            {
                var objPath = paths[path] as IDictionary<string, JsonNode>;
                var tags = objPath.First().Value["tags"] as JsonArray;
                var content = (objPath.First().Value["responses"] as IDictionary<string, JsonNode>)["200"]["content"];
                if (content == null)
                {
                    continue;
                }
                var schema = ((content as IDictionary<string, JsonNode>).First().Value)["schema"];
                if (schema["$ref"] == null)
                {
                    continue;
                }

                var tag = tags[0].GetValue<string>();
                if (!result.TryGetValue(tag, out var list))
                {
                    list = new List<TNode>();
                    result.Add(tag, list);
                }

                var refKey = schema["$ref"].GetValue<string>().Substring(prefixLength);
                if (TryFindSchema(refKey, schemas, out var nodes, prefixLength))
                {
                    foreach (var n in nodes)
                    {
                        if (!list.Any(s => s.Path == n.Path))
                        {
                            list.Add(n);
                        }
                    }
                }
            }

            return result;
        }

        private bool TryFindSchema(string refKey, IDictionary<string, JsonNode> schemas, out IEnumerable<TNode> nodes, int prefixLength)
        {
            if (!schemas.TryGetValue(refKey, out var node))
            {
                nodes = null;
                return false;
            }

            if (node is IDictionary<string, JsonNode> dict)
            {
                if (dict["properties"] == null)
                {
                    nodes = null;
                    return false;
                }

                var nested = new List<TNode>();
                var nestedc = 0;

                var properties = dict["properties"] as IDictionary<string, JsonNode>;
                foreach (var p in properties)
                {
                    if (p.Value["$ref"] != null)
                    {
                        continue;
                    }

                    var ptype = p.Value["type"].GetValue<string>();
                    if (ptype == "array")
                    {
                        if (p.Value["items"]["$ref"] != null)
                        {
                            var refKey1 = p.Value["items"]["$ref"].GetValue<string>().Substring(prefixLength);

                            if (refKey1 != refKey && TryFindSchema(refKey1, schemas, out IEnumerable<TNode> nested1, prefixLength))
                            {
                                nested.AddRange(nested1);
                                nestedc++;
                            }
                        }
                    }
                }

                nodes = new TNode[] { CreateNode(GetName(refKey), refKey, dict) };
                if (nestedc == 1)
                {
                    nodes = nested;
                }
                else if (nestedc > 1)
                {
                    nodes = nodes.Union(nested);
                }

                return true;
            }

            nodes = null;
            return false;
        }

        private string GetName(string key)
        {
            var d = -1;
            if ((d = key.IndexOf("`")) != -1)
            {
                key = key.Substring(0, d);
            }

            return key.Split('.').Last();
        }

        private TNode CreateNode(string name, string path, IDictionary<string, JsonNode> obj)
        {
            var schemaExtManager = _serviceProvider.TryGetService<ISchemaExtensionManager>();

            var node = new TNode();
            node.Name = name;
            node.Path = path;

            node.Table = schemaExtManager.Build<Table>();

            node.Table.Name = name;
            node.Table.ClassName = name;
            node.Table.Description = obj["description"]?.GetValue<string>();

            var properties = obj["properties"] as IDictionary<string, JsonNode>;
            foreach (var p in properties)
            {
                var column = schemaExtManager.Build<Column>(node.Table);
                column.Name = p.Key;
                column.PropertyName = p.Key;
                node.Table.Columns.Add(FormatColumn(column, p.Value));
            }

            return node;
        }

        private static Column FormatColumn(Column column, JsonNode p)
        {
            var ptype = p["type"]?.GetValue<string>();
            if (ptype == null)
            {
                column.DbType = DbType.Int32;
                return column;
            }

            var format = p["format"]?.GetValue<string>();
            var nullable = p["nullable"]?.GetValue<bool>();
            column.Description = p["description"]?.GetValue<string>();

            switch (ptype)
            {
                case "string":
                    switch (format)
                    {
                        case "date-time":
                            column.DbType = DbType.DateTime;
                            column.DataType = "date-time";
                            break;
                        default:
                            column.DbType = DbType.String;
                            column.DataType = "string";
                            break;
                    }
                    break;
                case "integer":
                    switch (format)
                    {
                        case "int32":
                            column.DbType = DbType.Int32;
                            column.DataType = "int32";
                            break;
                        case "int64":
                            column.DbType = DbType.Int64;
                            column.DataType = "int64";
                            break;
                    }
                    break;
                case "number":
                    switch (format)
                    {
                        case "double":
                            column.DbType = DbType.Double;
                            column.DataType = "double";
                            break;
                        case "float":
                            column.DbType = DbType.Single;
                            column.DataType = "float";
                            break;
                    }
                    break;
                case "boolean":
                    column.DbType = DbType.Boolean;
                    column.DataType = "boolean";
                    break;
            }

            if (nullable != null)
            {
                column.IsNullable = nullable.Value;
            }

            return column;
        }

    }

    public class TNode
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public Table Table { get; set; }
    }
}
