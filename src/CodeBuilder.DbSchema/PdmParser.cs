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
using System.Xml;

namespace CodeBuilder.DbSchema
{
    public class PdmParser
    {
        private readonly XmlDocument _doc;
        private readonly string _pdmFileName;
        private readonly string _databaseType;
        private readonly List<string> _existsCodes = new List<string>();

        public PdmParser(string pdmFileName)
        {
            _pdmFileName = pdmFileName;
            _doc = new XmlDocument();
            _doc.Load(pdmFileName);

            var dbNode = _doc.SelectSingleNode("//project");
            _databaseType = dbNode.Attributes["database"].InnerText;
        }

        public static PdmDefinition Parse(string pdmFileName, bool hasView)
        {
            return new PdmParser(pdmFileName).ParseDefinition(hasView);
        }

        private PdmDefinition ParseDefinition(bool hasView)
        {
            var definition = new PdmDefinition();

            foreach (XmlNode node in _doc.SelectNodes("//project/schema"))
            {
                var schema = new PdmSchema { Name = node.Attributes["name"].InnerText };

                foreach (XmlNode tNode in node.SelectNodes("table"))
                {
                    var descNode = tNode.SelectSingleNode("comment");

                    var table = new PdmTable();
                    table.Name = tNode.Attributes["name"].InnerText;
                    table.Uri = $"//project/schema[@name='{schema.Name}']/table[@name='{tNode.Attributes["name"].InnerText}']";
                    if (descNode != null)
                    {
                        table.Description = descNode.InnerText;
                    }

                    schema.Tables.Add(table);
                }

                if (hasView)
                {
                    foreach (XmlNode tNode in node.SelectNodes("view"))
                    {
                        var table = new PdmTable(true);
                        table.Name = tNode.Attributes["name"].InnerText;
                        table.Uri = $"//project/schema[@name='{schema.Name}']/view[@name='{tNode.Attributes["name"].InnerText}']";
                        schema.Tables.Add(table);
                    }
                }

                definition.Schemas.Add(schema);
            }

            return definition;
        }

        public Table ParseTable(IServiceProvider serviceProvider, PdmTable table)
        {
            var schemaExtManager = serviceProvider.TryGetService<ISchemaExtensionManager>();

            var node = _doc.SelectSingleNode(table.Uri);
            var ndColumns = node.SelectNodes("column");
            var ndIndexes = node.SelectNodes("index");
            var newtable = schemaExtManager.Build<Table>(table.IsView);
            var descNode = node.SelectSingleNode("comment");

            newtable.Name = table.Name;
            if (descNode != null)
            {
                newtable.Description = descNode.InnerText;
            }

            var pkNodes = node.SelectNodes("index[@unique='PRIMARY_KEY']/column");

            foreach (XmlNode child in ndColumns)
            {
                var column = schemaExtManager.Build<Column>(newtable);
                column.Name = child.Attributes["name"].InnerText;

                var cdescNode = child.SelectSingleNode("comment");
                if (cdescNode != null)
                {
                    column.Description = cdescNode.InnerText;
                }

                if (child.Attributes["mandatory"] == null)
                {
                    column.IsNullable = true;
                }

                column.DataType = child.Attributes["type"].InnerText;
                column.DbType = DataTypeManager.GetDataType(_databaseType, column.DataType);

                if (child.Attributes["decimal"] != null)
                {
                    column.Precision = child.Attributes["length"].InnerText.To<int>();
                    column.Scale = child.Attributes["decimal"].InnerText.To<int>();
                    column.ColumnType = $"{column.DataType}({column.Precision}, {column.Scale})";
                }
                else if (child.Attributes["length"] != null)
                {
                    column.Length = child.Attributes["length"].InnerText.To<long>();
                    if (column.DbType == DbType.String || column.DbType == DbType.StringFixedLength || column.DbType == DbType.AnsiString || column.DbType == DbType.AnsiStringFixedLength)
                    {
                        column.ColumnType = $"{column.DataType}({column.Length})";
                    }
                    else
                    {
                        column.ColumnType = column.DataType;
                    }
                }

                var defNode = child.SelectSingleNode("defo");
                if (defNode != null)
                {
                    column.DefaultValue = defNode.InnerText;
                }

                for (var i = 0; i < pkNodes.Count; i++)
                {
                    if (pkNodes[i].Attributes["name"].InnerText == column.Name)
                    {
                        column.IsPrimaryKey = true;
                        break;
                    }
                }

                newtable.Columns.Add(column);
            }

            foreach (XmlNode child in ndIndexes)
            {
                var name = child.Attributes["name"].Value;
                var index = new Index(name);
                index.IsUniqueKey = child.Attributes["unique"]?.Value == "UNIQUE";

                foreach (XmlNode c in child.SelectNodes("column"))
                {
                    var column = newtable.FindColumn(c.Attributes["name"].Value);
                    if (column != null)
                    {
                        column.IsUniqueKey = index.IsUniqueKey;
                        index.AddColumn(column);
                    }
                }

                newtable.Indexes.Add(index);
            }

            return newtable;
        }

        public void ParseReferences(List<Table> source, List<Table> result)
        {
            foreach (var uri in source.GroupBy(s => ((PdmTable)s).Uri).ToDictionary(s => s.Key))
            {
                var node = _doc.SelectSingleNode(uri.Key);

                foreach (PdmTable t in uri.Value)
                {
                    var fktable = result.FirstOrDefault(s => s.Name == t.Name);

                    foreach (XmlNode child in node.SelectNodes("fk"))
                    {
                        var totable = child.Attributes["to_table"].InnerText;
                        var fkNode = child.SelectSingleNode("fk_column");

                        var pktable = result.FirstOrDefault(s => s.Name == totable);
                        if (pktable == null)
                        {
                            continue;
                        }

                        var fkcolumn = fktable.FindColumn(fkNode.Attributes["name"].InnerXml);
                        var pkcolumn = pktable.FindColumn(fkNode.Attributes["pk"].InnerXml);

                        fkcolumn.BindForeignKey(new Reference(pktable, pkcolumn, fktable, fkcolumn));
                    }
                }
            }
        }
    }
}
