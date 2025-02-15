// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Variable;
using Fireasy.Common.DependencyInjection;
using Fireasy.Common.Extensions;
using Fireasy.Common.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;

namespace CodeBuilder.Core.Source
{
    public class SchemaRepository : ISchemaRepository, ISingletonService
    {
        private readonly IBinarySerializer _binarySerializer;
        private readonly ISchemaExtensionManager _schemaExtensionManager;
        private Dictionary<string, PropertyInfo> _tableProperties;
        private Dictionary<string, PropertyInfo> _columnProperties;

        public SchemaRepository(IBinarySerializer binarySerializer, ISchemaExtensionManager schemaExtensionManager)
        {
            _binarySerializer = binarySerializer;
            _schemaExtensionManager = schemaExtensionManager;
        }

        public void SaveSchemaFile(string fileName, IEnumerable<Table> tables)
        {
            void PutToWriter(JsonTextWriter writer)
            {
                writer.WriteStartObject();

                writer.WritePropertyName("Version");
                writer.WriteValue("3.4");
                writer.WritePropertyName("DbType");
                writer.WriteValue(tables.FirstOrDefault()?.Host?.DbType);

                WriteTables(writer, tables);

                WriteReferences(writer, tables.SelectMany(s => s.ForeignKeys));

                writer.WriteEndObject();
            }

            if (fileName.EndsWith(".dss", StringComparison.OrdinalIgnoreCase))
            {
                using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                using (var cstream = new GZipStream(stream, CompressionMode.Compress))
                using (var tstream = new StreamWriter(cstream))
                using (var writer = new JsonTextWriter(tstream))
                {
                    PutToWriter(writer);
                }
            }
            else if (fileName.EndsWith(".dso", StringComparison.OrdinalIgnoreCase))
            {
                using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                using (var tstream = new StreamWriter(stream))
                using (var writer = new JsonTextWriter(tstream))
                {
                    PutToWriter(writer);
                }
            }
        }

        public void SaveRelationFile(string fileName, IEnumerable<Table> tables)
        {
            using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            using (var cstream = new GZipStream(stream, CompressionMode.Compress))
            using (var tstream = new StreamWriter(cstream))
            using (var writer = new JsonTextWriter(tstream))
            {
                writer.WriteStartObject();

                writer.WritePropertyName("Version");
                writer.WriteValue("3.3");

                WriteReferences(writer, tables.SelectMany(s => s.ForeignKeys));

                writer.WriteEndObject();
            }
        }

        public List<Table> ReadFile(string fileName)
        {
            var json = string.Empty;

            try
            {
                if (fileName.EndsWith(".dss", StringComparison.OrdinalIgnoreCase))
                {
                    using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                    using (var cstream = new GZipStream(stream, CompressionMode.Decompress))
                    using (var tstream = new StreamReader(cstream))
                    {
                        json = tstream.ReadToEnd();
                    }
                }
                else if (fileName.EndsWith(".dso", StringComparison.OrdinalIgnoreCase))
                {
                    using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                    using (var tstream = new StreamReader(stream))
                    {
                        json = tstream.ReadToEnd();
                    }
                }
            }
            catch (Exception exp)
            {
                throw new OpenSchemaFileException($"无法打开文件 {fileName}。", exp);
            }

            var obj = JsonConvert.DeserializeObject<JObject>(json);
            var host = new Host();

            var version = obj.GetValue("Version").Value<string>();
            if (Convert.ToDecimal(version) >= 3.4m)
            {
                host.DbType = obj.GetValue("DbType").Value<string>();
            }

            try
            {
                _tableProperties = new Dictionary<string, PropertyInfo>();
                _columnProperties = new Dictionary<string, PropertyInfo>();

                var tables = ReadTables(obj.GetValue("Tables"));
                var references = ReadReferences(obj.GetValue("References"));

                tables.ForEach(s => host.Attach(s));

                var dict = tables.ToDictionary(s => s._Name);

                foreach (var refer in references)
                {
                    Reference reference = null;

                    if (Convert.ToDecimal(version) >= 3.3m)
                    {
                        reference = FindReferenceV3_3(refer, dict);
                    }
                    else
                    {
                        reference = FindReference(refer, dict);
                    }

                    if (reference != null)
                    {
                        reference.FkColumn.BindForeignKey(reference);
                    }
                }

                return tables;
            }
            catch (Exception exp)
            {
                throw new OpenSchemaFileException(version, $"无法打开文件 {fileName}，可能文件版本 {version} 与当前版本不匹配。", exp);
            }
        }

        private Reference FindReference(RepReference reference, Dictionary<string, Table> tables)
        {
            tables.TryGetValue(reference.FkTable, out var fktable);
            tables.TryGetValue(reference.PkTable, out var pktable);
            var fkcolumn = fktable?.FindColumn(reference.FkColumn);
            var pkcolumn = pktable?.FindColumn(reference.PkColumn);

            if (fkcolumn == null)
            {
                return null;
            }

            return new Reference(pktable, pkcolumn, fktable, fkcolumn);
        }

        private Reference FindReferenceV3_3(RepReference reference, Dictionary<string, Table> tables)
        {
            tables.TryGetValue(reference.FkTable, out var fktable);
            tables.TryGetValue(reference.PkTable, out var pktable);
            var fkcolumn = fktable?.FindColumn(reference.FkColumn);
            var pkcolumn = pktable?.FindColumn(reference.PkColumn);

            if (pkcolumn == null)
            {
                return null;
            }

            var refer = new Reference(pktable, pkcolumn, fktable, fkcolumn);
            if (reference.OnUpdate != null)
            {
                refer.OnUpdate = (Constraint)reference.OnUpdate;
            }
            if (reference.OnDelete != null)
            {
                refer.OnDelete = (Constraint)reference.OnDelete;
            }
            if (reference.Relationship != null)
            {
                refer.Relationship = (Relationship)reference.Relationship;
            }
            return refer;
        }

        public void ReadRelationFile(string fileName, IEnumerable<Table> tables)
        {
            using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            using (var cstream = new GZipStream(stream, CompressionMode.Decompress))
            using (var tstream = new StreamReader(cstream))
            {
                var json = tstream.ReadToEnd();
                var obj = JsonConvert.DeserializeObject<JObject>(json);

                var version = obj.GetValue("Version").Value<string>();

                var host = new Host();
                var dict = tables.ToDictionary(s => s._Name);

                var references = ReadReferences((JArray)obj.GetValue("References"));

                foreach (var refer in references)
                {
                    Reference reference = null;

                    if (Convert.ToDecimal(version) >= 3.3m)
                    {
                        reference = FindReferenceV3_3(refer, dict);
                    }
                    else
                    {
                        reference = FindReference(refer, dict);
                    }

                    if (reference != null)
                    {
                        reference.FkColumn.BindForeignKey(reference);
                    }
                }
            }
        }

        private void WriteTables(JsonTextWriter writer, IEnumerable<Table> tables)
        {
            writer.WritePropertyName("Tables");

            writer.WriteStartArray();

            foreach (var t in tables)
            {
                WriteTable(writer, t);
            }

            writer.WriteEndArray();
        }

        private void WriteReferences(JsonTextWriter writer, IEnumerable<Reference> references)
        {
            writer.WritePropertyName("References");

            writer.WriteStartArray();

            foreach (var r in references)
            {
                WriteReference(writer, r);
            }

            writer.WriteEndArray();
        }

        private void WriteTable(JsonTextWriter writer, Table table)
        {
            writer.WriteStartObject();

            var properties = table.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var prop in properties)
            {
                if (prop.IsDefined<UnPersistentlyAttribute>())
                {
                    continue;
                }

                writer.WritePropertyName(prop.Name);
                writer.WriteValue(prop.GetValue(table));
            }

            writer.WritePropertyName(nameof(Table.Columns));
            writer.WriteStartArray();

            foreach (var c in table.Columns)
            {
                WriteColumn(writer, c);
            }

            writer.WriteEndArray();

            writer.WritePropertyName(nameof(Table.Indexes));
            writer.WriteStartArray();

            foreach (var u in table.Indexes)
            {
                WriteIndex(writer, u);
            }

            writer.WriteEndArray();
            writer.WriteEndObject();
        }

        private void WriteColumn(JsonTextWriter writer, Column column)
        {
            writer.WriteStartObject();

            var properties = column.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var prop in properties)
            {
                if (prop.IsDefined<UnPersistentlyAttribute>())
                {
                    continue;
                }

                writer.WritePropertyName(prop.Name);
                writer.WriteValue(prop.GetValue(column));
            }

            writer.WriteEndObject();
        }

        private void WriteIndex(JsonTextWriter writer, Index index)
        {
            writer.WriteStartObject();

            writer.WritePropertyName(nameof(index.Name));
            writer.WriteValue(index.Name);

            writer.WritePropertyName(nameof(index.IsUniqueKey));
            writer.WriteValue(index.IsUniqueKey);

            writer.WritePropertyName(nameof(index.Columns));
            writer.WriteStartArray();

            foreach (var column in index.Columns)
            {
                writer.WriteStartObject();

                writer.WritePropertyName(nameof(column.Name));
                writer.WriteValue(column.Name);

                writer.WritePropertyName(nameof(column.SortOrder));
                writer.WriteValue(column.SortOrder);

                writer.WriteEndObject();
            }

            writer.WriteEndArray();
            writer.WriteEndObject();
        }

        private void WriteReference(JsonTextWriter writer, Reference reference)
        {
            writer.WriteStartObject();

            writer.WritePropertyName(nameof(Reference.PkTable));
            writer.WriteValue(reference.PkTable._Name);

            writer.WritePropertyName(nameof(Reference.PkColumn));
            writer.WriteValue(reference.PkColumn._Name);

            writer.WritePropertyName(nameof(Reference.FkTable));
            writer.WriteValue(reference.FkTable._Name);

            writer.WritePropertyName(nameof(Reference.FkColumn));
            writer.WriteValue(reference.FkColumn._Name);

            writer.WritePropertyName(nameof(Reference.OnUpdate));
            writer.WriteValue((int)reference.OnUpdate);

            writer.WritePropertyName(nameof(Reference.OnDelete));
            writer.WriteValue((int)reference.OnDelete);

            writer.WritePropertyName(nameof(Reference.Relationship));
            writer.WriteValue((int)reference.Relationship);

            writer.WriteEndObject();
        }

        private List<Table> ReadTables(object array)
        {
            var tables = new List<Table>();
            if (!(array is JArray jarray))
            {
                return tables;
            }

            foreach (JObject obj in jarray)
            {
                var table = _schemaExtensionManager.Build<Table>();
                table._Name = obj.GetValue(nameof(Table._Name))?.Value<string>();

                foreach (var p in obj.Properties().Where(s => s.Name != nameof(Table._Name)))
                {
                    var value = obj.GetValue(p.Name);
                    if (p.Name == nameof(Table.Columns))
                    {
                        table.Columns.AddRange(ReadColumns(table, (JArray)value));
                    }
                    else if (p.Name == nameof(Table.Indexes))
                    {
                        var dic = table.Columns.ToDictionary(s => s.Name);
                        table.Indexes.AddRange(ReadIndexes(dic, (JArray)value));
                    }
                    else if (value is JValue jvalue)
                    {
                        if (p.Name == nameof(Table.IsView) && (bool)jvalue.Value == true)
                        {
                            table.SetIsView();
                        }
                        else
                        {
                            var property = _tableProperties.TryGetValue(p.Name, () => table.GetType().GetProperty(p.Name));
                            if (property != null && property.CanWrite)
                            {
                                property.SetValue(table, jvalue.Value.To(property.PropertyType));
                            }
                        }
                    }
                }

                tables.Add(table);
            }

            return tables;
        }

        private List<Column> ReadColumns(Table table, JArray array)
        {
            var columns = new List<Column>();

            foreach (JObject obj in array)
            {
                var column = _schemaExtensionManager.Build<Column>(table);
                column._Name = obj.GetValue(nameof(Table._Name))?.Value<string>();

                foreach (var prop in obj.Properties().Where(s => s.Name != nameof(Table._Name)))
                {
                    var value = obj.GetValue(prop.Name);
                    if (value is JValue jvalue)
                    {
                        var property = _columnProperties.TryGetValue(prop.Name, () => column.GetType().GetProperty(prop.Name));
                        if (property != null && property.CanWrite)
                        {
                            property.SetValue(column, jvalue.Value.To(property.PropertyType));
                        }
                    }
                }

                columns.Add(column);
            }

            return columns;
        }

        private List<Index> ReadIndexes(Dictionary<string, Column> columnDict, JArray array)
        {
            var indexes = new List<Index>();

            foreach (JObject obj in array)
            {
                var iname = obj.GetValue(nameof(Index.Name)).Value<string>();
                var isUniqueKey = obj.GetValue(nameof(Index.IsUniqueKey))?.Value<bool>();
                var columns = obj.GetValue(nameof(Index.Columns));

                var index = new Index(iname);
                index.IsUniqueKey = isUniqueKey ?? false;

                if (columns is JArray carray)
                {
                    foreach (JObject o in carray)
                    {
                        var cname = o.GetValue(nameof(IndexColumn.Name)).Value<string>();
                        if (columnDict.TryGetValue(cname, out var column))
                        {
                            var idxc = index.AddColumn(column);
                            idxc.SortOrder = o.GetValue(nameof(IndexColumn.SortOrder))?.Value<string>();
                        }
                    }
                }

                indexes.Add(index);
            }

            return indexes;
        }

        private List<RepReference> ReadReferences(object array)
        {
            var references = new List<RepReference>();
            if (!(array is JArray jarray))
            {
                return references;
            }

            foreach (JObject obj in jarray)
            {
                references.Add(new RepReference(
                    obj.Value<string>(nameof(Reference.PkTable)), 
                    obj.Value<string>(nameof(Reference.PkColumn)), 
                    obj.Value<string>(nameof(Reference.FkTable)), 
                    obj.Value<string>(nameof(Reference.FkColumn)),
                    obj.Value<int?>(nameof(Reference.OnUpdate)),
                    obj.Value<int?>(nameof(Reference.OnDelete)),
                    obj.Value<int?>(nameof(Reference.Relationship))));
            }

            return references;
        }

        private struct RepReference
        {
            public RepReference(string pkTable, string pkColumn, string fkTable, string fkColumn, int? onUpdate, int? onDelete, int? relationship)
            {
                PkTable = pkTable;
                PkColumn = pkColumn;
                FkTable = fkTable;
                FkColumn = fkColumn;
                OnUpdate = onUpdate;
                OnDelete = onDelete;
                Relationship = relationship;
            }

            public string PkTable { get; }
            public string PkColumn { get; }
            public string FkTable { get; }
            public string FkColumn { get; }
            public int? OnUpdate { get; }
            public int? OnDelete { get; }
            public int? Relationship { get; }
        }
    }
}
