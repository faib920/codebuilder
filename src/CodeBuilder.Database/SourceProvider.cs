// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
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
using Fireasy.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Schema = Fireasy.Data.Schema;

namespace CodeBuilder.Database
{
    [Export(typeof(ISourceProvider))]
    public class SourceProvider : ISourceProvider
    {
        private DbSourceStruct _con;
        private IDevHosting _hosting;
        private HistoryStorage<DbSourceStruct> _historyStorage;

        public string Name => "Database";

        public Image Icon => Properties.Resources.database;

        public string Description => "从数据库中获取数据表结构，支持 SqlServer、MySql、Oracle、SQLite、PostgreSql、Firebird、达梦、人大金仓、神通，以及 OleDb、Odbc 驱动。";

        public void Initialize(IDevHosting hosting)
        {
            _hosting = hosting;
            _historyStorage = new HistoryStorage<DbSourceStruct>(Path.Combine(_hosting.WorkPath, "history.database"), 10);
        }

        public async Task<List<Table>> PreviewAsync(SourceOption option, CancellationToken cancellationToken = default)
        {
            IEnumerable<Table> tables = null;
            using (var frm = new frmSourceMgr(_hosting))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    _con = frm.DbConStr;
                    tables = await OpenDbAsync(option);
                }
            }

            if (tables == null)
            {
                return null;
            }

            using (var frm = new frmTableSelector(_hosting, tables, option.Selected))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    option.SkipSchema = frm.IsCustomSQL;
                    option.Append = frm.Append;
                    _historyStorage.Add(_con);
                    _hosting.OnSourceHistoryChanged();

                    return frm.Selected.Count == 0 ? null : frm.Selected;
                }
            }

            return null;
        }

        public async Task<List<Table>> FromHistoryAsync(object history, SourceOption option, CancellationToken cancellationToken = default)
        {
            if (history is DbSourceStruct str)
            {
                _con = str;
                var tables = await OpenDbAsync(option);
                if (tables == null)
                {
                    return null;
                }

                using (var frm = new frmTableSelector(_hosting, tables, option.Selected))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        option.SkipSchema = frm.IsCustomSQL;
                        option.Append = frm.Append;
                        _historyStorage.Add(_con);
                        _hosting.OnSourceHistoryChanged();

                        return frm.Selected.Count == 0 ? null : frm.Selected;
                    }
                }
            }

            return null;
        }

        public async Task<List<Table>> GetSchemaAsync(List<Table> tables, TableSchemaProcessHandler reding, CancellationToken cancellationToken = default)
        {
            try
            {
                return await GetDbSchemaAsync(tables, reding, cancellationToken);
            }
            catch (Exception exp)
            {
                ErrorMessageBox.Show("获取数据架构时出错。", exp);
                return null;
            }
        }

        public async Task<List<Table>> ParseSQLAsync(string content)
        {
            var databaseFactory = _hosting.ServiceProvider.TryGetService<IDatabaseFactory>();
            var schemaExtManager = _hosting.ServiceProvider.TryGetService<ISchemaExtensionManager>();

            using (var db = databaseFactory.CreateDatabase(_con.Type, _con.ConnectionString))
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

                    var table = schemaExtManager.Build<Table>();
                    table.Description = string.Empty;

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

                    using (var reader = await db.ExecuteReaderAsync((SqlCommand)sql, behavior: CommandBehavior.SchemaOnly))
                    {
                        var schema = reader.GetSchemaTable();
                        foreach (DataRow row in schema.Rows)
                        {
                            var column = schemaExtManager.Build<Column>(table);

                            column.Description = string.Empty;
                            column.ColumnType = string.Empty;
                            column = column.TrySetValue(row, "ColumnName", (c, s) => c.Name = s.ToString())
                                .TrySetValue<bool>(row, "IsAutoIncrement", (c, s) => c.AutoIncrement = s)
                                .TrySetValue<long?>(row, "ColumnSize", (c, s) => c.Length = s)
                                .TrySetValue<int?>(row, "NumericPrecision", (c, s) => c.Precision = s)
                                .TrySetValue<int?>(row, "NumericScale", (c, s) => c.Scale = s)
                                .TrySetValue<bool>(row, "IsKey", (c, s) => c.IsPrimaryKey = s)
                                .TrySetValue<bool>(row, "AllowDBNull", (c, s) => c.IsNullable = s)
                                .TrySetValue(row, "DataType", (c, s) => c.DbType = ((Type)s).GetDbType())
                                .TrySetValue(row, "DataTypeName", (c, s) => c.DataType = s?.ToString());

                            table.Columns.Add(column);
                        }
                    }

                    tables.Add(table);
                }

                return tables;
            }
        }

        public IEnumerable<object> GetHistory()
        {
            return _historyStorage.Items;
        }

        public void ClearHistory()
        {
            _historyStorage.Clear();
        }

        public void DeleteHistory(object record)
        {
            _historyStorage.Delete((DbSourceStruct)record);
        }

        private async Task<IEnumerable<Table>> OpenDbAsync(SourceOption option)
        {
            try
            {
                var databaseFactory = _hosting.ServiceProvider.TryGetService<IDatabaseFactory>();
                using (var db = databaseFactory.CreateDatabase(_con.Type, _con.ConnectionString))
                {
                    var schema = db.Provider.GetService<Schema.ISchemaProvider>();
                    var tables = await schema.GetSchemasAsync<Schema.Table>(db).ToListAsync();
                    var views = option.View ? await schema.GetSchemasAsync<Schema.View>(db).ToListAsync() : new List<Schema.View>();
                    return ConvertSchemaTables(tables, views);
                }
            }
            catch (Exception exp)
            {
                ErrorMessageBox.Show("获取数据架构时出错。", exp);
                return null;
            }
        }

        private async Task<List<Table>> GetDbSchemaAsync(IList<Table> tables, TableSchemaProcessHandler processHandler, CancellationToken cancellationToken = default)
        {
            var databaseFactory = _hosting.ServiceProvider.TryGetService<IDatabaseFactory>();
            using (var db = databaseFactory.CreateDatabase(_con.Type, _con.ConnectionString))
            {
                var providerName = db.Provider.ProviderName;
                var schema = db.Provider.GetService<Schema.ISchemaProvider>();
                List<Schema.ForeignKey> foreignKeys = null;
                List<Schema.IndexColumn> indexColumns = null;

                var tableCount = tables.Count;

                var calc = new Func<int, int>(i =>
                    {
                        return (int)((i / (tableCount * 1.0)) * 100);
                    });

                var host = new Host();

                var result = schema.RestrictionMultipleQuerySupport ?
                    await BatchGetSchemaAsync(host, providerName, db, tables, processHandler, calc, cancellationToken) :
                    await GetSchemaAsync(host, providerName, db, tables, processHandler, calc, cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                try
                {
                    foreignKeys = await schema.GetSchemasAsync<Schema.ForeignKey>(db).ToListAsync();
                    indexColumns = await schema.GetSchemasAsync<Schema.IndexColumn>(db).ToListAsync();
                }
                catch { }

                if (foreignKeys != null)
                {
                    ProcessForeignKeys(result, foreignKeys);
                }

                if (indexColumns != null)
                {
                    ProcessUniqueKeys(result, indexColumns);
                }

                return result;
            }
        }

        private async Task<List<Table>> GetSchemaAsync(Host host, string providerName, IDatabase db, IList<Table> tables, TableSchemaProcessHandler processHandler, Func<int, int> calc, CancellationToken cancellationToken = default)
        {
            var schema = db.Provider.GetService<Schema.ISchemaProvider>();
            var result = new List<Table>();

            var index = 0;
            foreach (var t in tables)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                if (processHandler != null)
                {
                    processHandler.Invoke(t.Name, calc(++index));
                }

                if (t.IsView)
                {
                    var columns = await schema.GetSchemasAsync<Schema.ViewColumn>(db, s => s.ViewName == t.Name).ToListAsync();
                    t.Columns.AddRange(ConvertSchemaViewColumns(t, columns, providerName));
                    result.Add(t);
                }
                else
                {
                    var columns = await schema.GetSchemasAsync<Schema.Column>(db, s => s.TableName == t.Name).ToListAsync();
                    t.Columns.AddRange(ConvertSchemaColumns(t, columns, providerName));
                    result.Add(t);
                }

                host.Attach(t);
            }

            return result;
        }

        private async Task<List<Table>> BatchGetSchemaAsync(Host host, string providerName, IDatabase db, IList<Table> tables, TableSchemaProcessHandler processHandler, Func<int, int> calc, CancellationToken cancellationToken = default)
        {
            var schema = db.Provider.GetService<Schema.ISchemaProvider>();
            var result = new List<Table>();

            var split = Math.Min(Math.Max(5, tables.Count / 10), 50);

            var index = 0;
            foreach (var g in tables.GroupBy(s => s.IsView))
            {
                foreach (var splitTables in g.Split(split))
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return null;
                    }

                    var names = splitTables.Select(s => s.Name);

                    if (processHandler != null)
                    {
                        index += splitTables.Count();
                        processHandler.Invoke(splitTables.Last().Name, calc(index));
                    }

                    if (g.Key)
                    {
                        var columns = await schema.GetSchemasAsync<Schema.ViewColumn>(db, s => names.Contains(s.ViewName)).ToListAsync();

                        splitTables.ForEach(t => t.Columns.AddRange(ConvertSchemaViewColumns(t, columns.Where(s => s.ViewName == t.Name), providerName)));
                    }
                    else
                    {
                        var columns = await schema.GetSchemasAsync<Schema.Column>(db, s => names.Contains(s.TableName)).ToListAsync();

                        splitTables.ForEach(t => t.Columns.AddRange(ConvertSchemaColumns(t, columns.Where(s => s.TableName == t.Name), providerName)));
                    }

                    result.AddRange(splitTables);
                    splitTables.ForEach(t => host.Attach(t));
                }
            }

            return result;
        }

        private IEnumerable<Table> ConvertSchemaTables(IEnumerable<Schema.Table> tables, IEnumerable<Schema.View> views)
        {
            var schemaExtManager = _hosting.ServiceProvider.TryGetService<ISchemaExtensionManager>();

            foreach (var t in tables)
            {
                var table = schemaExtManager.Build<Table>();
                table.Name = t.Name;
                table.Schema = t.Schema;
                table.Description = t.Description ?? string.Empty;

                yield return table;
            }

            foreach (var v in views)
            {
                var table = schemaExtManager.Build<Table>(true);
                table.Name = v.Name;
                table.Schema = v.Schema;
                table.Description = v.Description ?? string.Empty;

                yield return table;
            }
        }

        private IEnumerable<Column> ConvertSchemaColumns(Table table, IEnumerable<Schema.Column> columns, string providerName)
        {
            var schemaExtManager = _hosting.ServiceProvider.TryGetService<ISchemaExtensionManager>();

            foreach (var c in columns)
            {
                var column = schemaExtManager.Build<Column>(table);

                column.Name = c.Name;
                column.AutoIncrement = c.Autoincrement;
                column.Description = c.Description ?? string.Empty;
                column.IsNullable = c.IsNullable;
                column.DataType = c.DataType;
                column.Length = c.Length;
                column.Scale = c.NumericScale;
                column.Precision = c.NumericPrecision;
                column.IsPrimaryKey = c.IsPrimaryKey;
                column.DbType = c.DbType;
                column.DefaultValue = c.Default?.ToString();
                column.ColumnType = c.ColumnType ?? string.Empty;

                var dbType = DataTypeManager.GetDataType(providerName, column.DataType);
                if (dbType != null && dbType != column.DbType)
                {
                    column.DbType = dbType;
                }

                yield return column;
            }
        }

        private IEnumerable<Column> ConvertSchemaViewColumns(Table table, IEnumerable<Schema.ViewColumn> columns, string providerName)
        {
            var schemaExtManager = _hosting.ServiceProvider.TryGetService<ISchemaExtensionManager>();

            var index = 0;
            foreach (var c in columns)
            {
                var column = schemaExtManager.Build<Column>(table);

                column.Name = c.Name;
                column.AutoIncrement = c.Autoincrement;
                column.Description = c.Description ?? string.Empty;
                column.IsNullable = c.IsNullable;
                column.DataType = c.DataType;
                column.Length = c.Length;
                column.Scale = c.NumericScale;
                column.Precision = c.NumericPrecision;
                column.IsPrimaryKey = c.IsPrimaryKey;
                column.DbType = c.DbType;
                column.Index = ++index;
                column.ColumnType = c.ColumnType ?? string.Empty;

                var dbType = DataTypeManager.GetDataType(providerName, column.DataType);
                if (dbType != null && dbType != column.DbType)
                {
                    column.DbType = dbType;
                }

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
            var schemaExtManager = _hosting.ServiceProvider.TryGetService<ISchemaExtensionManager>();
            var dict = tables.ToDictionary(s => s._Name);

            foreach (var sfk in foreignKeys)
            {
                if (dict.TryGetValue(sfk.PKTable, out var pkTable) && dict.TryGetValue(sfk.TableName, out var fkTable))
                {
                    var pkColumn = pkTable.FindColumn(sfk.PKColumn);
                    var fkColumn = fkTable.FindColumn(sfk.ColumnName);

                    if (pkColumn != null && fkColumn != null)
                    {
                        var fk = schemaExtManager.Build<Reference>(pkTable, pkColumn, fkTable, fkColumn);
                        fk.Name = string.IsNullOrWhiteSpace(sfk.Name) ? "fk_" + RandomGenerator.Create() : sfk.Name;
                        fkColumn.BindForeignKey(fk);
                    }
                }
            }
        }

        private void ProcessUniqueKeys(List<Table> tables, List<Schema.IndexColumn> indexColumns)
        {
            var dict = tables.ToDictionary(s => s._Name);

            foreach (var idxColumn in indexColumns.Where(s => s.Type == Schema.IndexType.Unique))
            {
                if (dict.TryGetValue(idxColumn.TableName, out var table))
                {
                    var column = table.FindColumn(idxColumn.ColumnName);
                    if (column != null)
                    {
                        column.IsUniqueKey = true;
                    }
                }
            }
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
