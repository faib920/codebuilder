// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using CodeBuilder.Core.Source;
using CodeBuilder.Core.Variable;
using Fireasy.Common.Extensions;
using Fireasy.Common.Security;
using Fireasy.Data;
using Fireasy.Data.Extensions;
using Fireasy.Data.Provider;
using Fireasy.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Windows.Forms;
using Schema = Fireasy.Data.Schema;

namespace CodeBuilder.Database
{
    [Export(typeof(ISourceProvider))]
    public class SourceProvider : ISourceProvider
    {
        private DbSourceStruct _con;
        private IDevHosting _hosting;

        public string Name
        {
            get { return "Database"; }
        }

        public void Initialize(IDevHosting hosting)
        {
            _hosting = hosting;
        }

        public List<Table> Preview(SourceOption option)
        {
            IEnumerable<Table> tables = null;
            using (var frm = new frmSourceMgr(_hosting))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    _con = frm.DbConStr;
                    tables = OpenDb(option);
                }
            }

            if (tables == null)
            {
                return null;
            }

            using (var frm = new frmTableSelector(_hosting, tables))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    option.SkipSchema = frm.IsCustomSQL;
                    return frm.Selected.Count == 0 ? null : frm.Selected;
                }
            }

            return null;
        }

        public List<Table> GetSchema(List<Table> tables, TableSchemaProcessHandler reding)
        {
            try
            {
                return GetDbSchema(tables, reding);
            }
            catch (Exception exp)
            {
                ErrorMessageBox.Show("获取数据架构时出错。", exp);
                return null;
            }
        }

        public List<Table> ParseSQL(string content)
        {
            var provider = ProviderHelper.GetDefinedProviderInstance(_con.Type);
            using (var db = new Fireasy.Data.Database(_con.ConnectionString, provider))
            {
                var tables = new List<Table>();
                var index = 0;
                var sqlSegments = content.IndexOf(';') != -1 ? content.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries) : new[] { content };
                foreach (var _sql in sqlSegments)
                {
                    var sql = _sql.Trim(new[] { '\r', '\n' });
                    if (string.IsNullOrWhiteSpace(sql))
                    {
                        continue;
                    }

                    var table = new Table { Description = string.Empty };

                    var gotoIndex = sql.IndexOf("=>");
                    if (gotoIndex != -1)
                    {
                        table.Name = sql.Substring(0, gotoIndex).Trim();
                        sql = sql.Substring(gotoIndex + 2);
                    }
                    else
                    {
                        table.Name = "Table" + (++index);
                    }

                    using (var reader = db.ExecuteReader((SqlCommand)sql, behavior: CommandBehavior.SchemaOnly))
                    {
                        var schema = reader.GetSchemaTable();
                        foreach (DataRow row in schema.Rows)
                        {
                            var column = new Column(table)
                                .TrySetValue(row, "ColumnName", (c, s) => c.Name = s.ToString())
                                .TrySetValue<bool>(row, "IsAutoIncrement", (c, s) => c.AutoIncrement = s)
                                .TrySetValue<long?>(row, "ColumnSize", (c, s) => c.Length = s)
                                .TrySetValue<int?>(row, "NumericPrecision", (c, s) => c.Precision = s)
                                .TrySetValue<int?>(row, "NumericScale", (c, s) => c.Scale = s)
                                .TrySetValue<bool>(row, "IsKey", (c, s) => c.IsPrimaryKey = s)
                                .TrySetValue<bool>(row, "AllowDBNull", (c, s) => c.IsNullable = s)
                                .TrySetValue(row, "DataType", (c, s) => c.DbType = ((Type)s).GetDbType())
                                .TrySetValue(row, "DataTypeName", (c, s) => c.DataType = s.ToStringSafely());

                            table.Columns.Add(column);
                        }
                    }

                    tables.Add(table);
                }

                return tables;
            }
        }

        public List<string> GetHistory()
        {
            return new List<string>();
        }

        private IEnumerable<Table> OpenDb(SourceOption option)
        {
            try
            {
                var provider = ProviderHelper.GetDefinedProviderInstance(_con.Type);
                using (var db = new Fireasy.Data.Database(_con.ConnectionString, provider))
                {
                    var schema = db.Provider.GetService<Schema.ISchemaProvider>();
                    var tables = schema.GetSchemas<Schema.Table>(db).ToList();
                    var views = option.View ? schema.GetSchemas<Schema.View>(db).ToList() : new List<Schema.View>();
                    return ConvertSchemaTables(tables, views);
                }
            }
            catch (Exception exp)
            {
                ErrorMessageBox.Show("获取数据架构时出错。", exp);
                return null;
            }
        }

        private List<Table> GetDbSchema(IList<Table> tables, TableSchemaProcessHandler processHandler)
        {
            var result = new List<Table>();
            var provider = ProviderHelper.GetDefinedProviderInstance(_con.Type);
            using (var db = new Fireasy.Data.Database(_con.ConnectionString, provider))
            {
                var providerName = db.Provider.GetType().Name;
                var schema = db.Provider.GetService<Schema.ISchemaProvider>();
                List<Schema.ForeignKey> foreignKeys = null;
                try
                {
                    foreignKeys = schema.GetSchemas<Schema.ForeignKey>(db).ToList();
                }
                catch { }

                var dbTypes = schema.GetSchemas<Schema.DataType>(db).ToList();
                var tableCount = tables.Count;
                var index = 0;

                var calc = new Func<int, int>(i =>
                    {
                        return (int)((i / (tableCount * 1.0)) * 100);
                    });

                var host = new Host();

                foreach (var t in tables)
                {
                    if (Processor.IsCancellationRequested())
                    {
                        return null;
                    }

                    if (processHandler != null)
                    {
                        processHandler.Invoke(t.Name, calc(++index));
                    }

                    if (t.IsView)
                    {
                        var columns = schema.GetSchemas<Schema.ViewColumn>(db, s => s.ViewName == t.Name);
                        t.Columns.AddRange(ConvertSchemaViewColumns(t, columns, dbTypes, providerName));
                        result.Add(t);
                    }
                    else
                    {
                        var columns = schema.GetSchemas<Schema.Column>(db, s => s.TableName == t.Name);
                        t.Columns.AddRange(ConvertSchemaColumns(t, columns, dbTypes, providerName));
                        result.Add(t);
                    }
                    host.Attach(t);
                }

                if (foreignKeys != null)
                {
                    ProcessForeignKeys(result, foreignKeys);
                }
            }

            return result;
        }

        private IEnumerable<Table> ConvertSchemaTables(IEnumerable<Schema.Table> tables, IEnumerable<Schema.View> views)
        {
            var index = 0;
            foreach (var t in tables)
            {
                var table = SchemaExtensionManager.Build<Table>();
                table.Name = t.Name;
                table.Description = t.Description ?? string.Empty;
                table.Index = ++index;

                yield return table;
            }

            foreach (var v in views)
            {
                var table = SchemaExtensionManager.Build<Table>(true);
                table.Name = v.Name;
                table.Description = v.Description ?? string.Empty;
                table.Index = ++index;

                yield return table;
            }
        }

        private IEnumerable<Column> ConvertSchemaColumns(Table table, IEnumerable<Schema.Column> columns, List<Schema.DataType> dbTypes, string providerName)
        {
            var index = 0;
            foreach (var c in columns)
            {
                if (Processor.IsCancellationRequested())
                {
                    yield break;
                }

                var column = SchemaExtensionManager.Build<Column>(table);

                column.Name = c.Name;
                column.AutoIncrement = c.Autoincrement;
                column.Description = c.Description ?? string.Empty;
                column.IsNullable = c.IsNullable;
                column.DataType = c.DataType;
                column.Length = c.Length;
                column.Scale = c.NumericScale;
                column.Precision = c.NumericPrecision;
                column.IsPrimaryKey = c.IsPrimaryKey;
                column.DbType = ConvertDbType(column, dbTypes, providerName);
                column.DefaultValue = c.Default.ToStringSafely();
                column.ColumnType = c.ColumnType;
                column.Index = ++index;

                yield return column;
            }
        }

        private IEnumerable<Column> ConvertSchemaViewColumns(Table table, IEnumerable<Schema.ViewColumn> columns, List<Schema.DataType> dbTypes, string providerName)
        {
            var index = 0;
            foreach (var c in columns)
            {
                if (Processor.IsCancellationRequested())
                {
                    yield break;
                }

                var column = SchemaExtensionManager.Build<Column>(table);

                column.Name = c.Name;
                column.AutoIncrement = c.Autoincrement;
                column.Description = c.Description ?? string.Empty;
                column.IsNullable = c.IsNullable;
                column.DataType = c.DataType;
                column.Length = c.Length;
                column.Scale = c.NumericScale;
                column.Precision = c.NumericPrecision;
                column.IsPrimaryKey = c.IsPrimaryKey;
                column.DbType = ConvertDbType(column, dbTypes, providerName);
                column.Index = ++index;

                yield return column;
            }
        }

        /// <summary>
        /// 处理外键。
        /// </summary>
        /// <param name="tables"></param>
        /// <param name="foreignKeys"></param>
        private void ProcessForeignKeys(List<Table> tables, List<Schema.ForeignKey> foreignKeys)
        {
            foreach (var sfk in foreignKeys)
            {
                if (Processor.IsCancellationRequested())
                {
                    break;
                }

                var pkTable = tables.FirstOrDefault(s => s.Name.Equals(sfk.PKTable, StringComparison.CurrentCultureIgnoreCase));
                var fkTable = tables.FirstOrDefault(s => s.Name.Equals(sfk.TableName, StringComparison.CurrentCultureIgnoreCase));
                if (pkTable != null && fkTable != null)
                {
                    var pkColumn = pkTable.Columns.FirstOrDefault(s => s.Name.Equals(sfk.PKColumn, StringComparison.CurrentCultureIgnoreCase));
                    var fkColumn = fkTable.Columns.FirstOrDefault(s => s.Name.Equals(sfk.ColumnName, StringComparison.CurrentCultureIgnoreCase));

                    if (pkColumn != null && fkColumn != null)
                    {
                        var fk = SchemaExtensionManager.Build<Reference>(pkTable, pkColumn, fkTable, fkColumn);
                        fk.Name = string.IsNullOrWhiteSpace(sfk.Name) ? "fk_" + RandomGenerator.Create() : sfk.Name;
                        fkColumn.BindForeignKey(fk);
                    }
                }
            }
        }

        private DbType? ConvertDbType(Column column, List<Schema.DataType> dbTypes, string providerName)
        {
            if (providerName == "OleDbProvider")
            {
                var oleDbType = (OleDbType)Enum.Parse(typeof(OleDbType), column.DataType);
                switch (oleDbType)
                {
                    case OleDbType.VarChar:
                    case OleDbType.VarWChar:
                    case OleDbType.WChar:
                    case OleDbType.Char:
                        return DbType.String;
                    case OleDbType.SmallInt:
                        return DbType.Int16;
                    case OleDbType.Integer:
                        return DbType.Int32;
                    case OleDbType.BigInt:
                        return DbType.Int64;
                    case OleDbType.Binary:
                        return DbType.Binary;
                    case OleDbType.Boolean:
                        return DbType.Boolean;
                    case OleDbType.Currency:
                        return DbType.Currency;
                    case OleDbType.Date:
                    case OleDbType.DBDate:
                    case OleDbType.DBTime:
                    case OleDbType.DBTimeStamp:
                        return DbType.DateTime;
                    case OleDbType.Numeric:
                    case OleDbType.Decimal:
                    case OleDbType.VarNumeric:
                        return DbType.Decimal;
                    case OleDbType.Double:
                        return DbType.Double;
                    case OleDbType.Single:
                        return DbType.Single;
                    case OleDbType.TinyInt:
                        return DbType.SByte;
                    case OleDbType.UnsignedSmallInt:
                        return DbType.UInt16;
                    case OleDbType.UnsignedInt:
                        return DbType.UInt32;
                    case OleDbType.UnsignedBigInt:
                        return DbType.UInt64;
                    case OleDbType.UnsignedTinyInt:
                        return DbType.Byte;
                    case OleDbType.Guid:
                        return DbType.Guid;
                }
            }
            else
            {
                var dbType = dbTypes.FirstOrDefault(s => s.Name.Equals(column.DataType, StringComparison.CurrentCultureIgnoreCase));
                if (dbType != null)
                {
                    return dbType.SystemType.GetDbType();
                }
            }

            return null;
        }
    }

    internal static class ColumnSchemaHelper
    {
        internal static Column TrySetValue(this Column column, DataRow row, string name, Action<Column, object> setter)
        {
            if (row.Table.Columns.Contains(name))
            {
                setter(column, row[name]);
            }

            return column;
        }

        internal static Column TrySetValue<T>(this Column column, DataRow row, string name, Action<Column, T> setter)
        {
            if (row.Table.Columns.Contains(name))
            {
                setter(column, row[name].To<T>());
            }

            return column;
        }
    }
}
