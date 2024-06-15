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
                writer.WriteValue("3.3");

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

            var version = obj.GetValue("Version").Value<string>();

            try
            {
                var host = new Host();

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

        private Reference FindReference((string PkTable, string PkColumn, string FkTable, string FkColumn) reference, Dictionary<string, Table> tables)
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

        private Reference FindReferenceV3_3((string PkTable, string PkColumn, string FkTable, string FkColumn) reference, Dictionary<string, Table> tables)
        {
            tables.TryGetValue(reference.FkTable, out var fktable);
            tables.TryGetValue(reference.PkTable, out var pktable);
            var fkcolumn = fktable?.FindColumn(reference.FkColumn);
            var pkcolumn = pktable?.FindColumn(reference.PkColumn);

            if (pkcolumn == null)
            {
                return null;
            }

            return new Reference(pktable, pkcolumn, fktable, fkcolumn);
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
                    else if (value is JValue jvalue)
                    {
                        if (p.Name == nameof(Table.IsView) && (bool)jvalue.Value == true)
                        {
                            table.SetIsView();
                        }
                        else
                        {
                            var property = table.GetType().GetProperty(p.Name);
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

        private List<Column> ReadColumns(Table table, object array)
        {
            var columns = new List<Column>();
            if (!(array is JArray jarray))
            {
                return columns;
            }

            foreach (JObject obj in jarray)
            {
                var column = _schemaExtensionManager.Build<Column>(table);
                column._Name = obj.GetValue(nameof(Table._Name))?.Value<string>();

                foreach (var prop in obj.Properties().Where(s => s.Name != nameof(Table._Name)))
                {
                    var value = obj.GetValue(prop.Name);
                    if (value is JValue jvalue)
                    {
                        var property = column.GetType().GetProperty(prop.Name);
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

        private List<(string PkTable, string PkColumn, string FkTable, string FkColumn)> ReadReferences(object array)
        {
            var references = new List<(string PkTable, string PkColumn, string FkTable, string FkColumn)>();
            if (!(array is JArray jarray))
            {
                return references;
            }

            foreach (JObject obj in jarray)
            {
                references.Add((obj.Value<string>(nameof(Reference.PkTable)), obj.Value<string>(nameof(Reference.PkColumn)), obj.Value<string>(nameof(Reference.FkTable)), obj.Value<string>(nameof(Reference.FkColumn))));
            }

            return references;
        }
    }
}
