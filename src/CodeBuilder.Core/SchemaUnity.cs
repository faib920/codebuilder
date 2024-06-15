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
using System.Linq;

namespace CodeBuilder.Core
{
    /// <summary>
    /// 架构辅助类。
    /// </summary>
    public class SchemaUnity
    {
        /// <summary>
        /// 重构所有表。
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="tables"></param>
        /// <returns></returns>
        public static Dictionary<string, Table> Refactoring(IServiceProvider serviceProvider, List<Table> tables)
        {
            var schemaExtManager = serviceProvider.TryGetService<ISchemaExtensionManager>();

            var newTables = new Dictionary<string, Table>();
            var host = new Host();

            foreach (var table in tables)
            {
                var newTable = schemaExtManager.Build<Table>();
                newTable.Refactoring(table, () => schemaExtManager.Build<Column>());
                newTables.Add(table._Name, newTable);
                host.Attach(newTable);
            }

            foreach (var table in tables)
            {
                if (!newTables.TryGetValue(table._Name, out var newTable))
                {
                    continue;
                }

                foreach (var column in table.Columns.Where(s => s.ForeignKey != null))
                {
                    var newColumn = newTable.FindColumn(column._Name);
                    if (newColumn == null)
                    {
                        continue;
                    }

                    Table pkTable = null, fkTable = null;
                    Column pkColumn = null, fkColumn = null;
                    var fk = column.ForeignKey;
                    if (fk.PkTable != null && newTables.TryGetValue(fk.PkTable._Name, out pkTable))
                    {
                        if (fk.PkColumn != null && pkTable != null)
                        {
                            pkColumn = pkTable.FindColumn(fk.PkColumn._Name);
                        }
                    }

                    if (fk.FkTable != null && newTables.TryGetValue(fk.FkTable._Name, out fkTable))
                    {
                        if (fk.FkColumn != null && fkTable != null)
                        {
                            fkColumn = fkTable.FindColumn(fk.FkColumn._Name);
                        }
                    }

                    var newRefer = schemaExtManager.Build<Reference>(pkTable, pkColumn, fkTable, fkColumn);
                    newRefer.Name = fk.Name;

                    newColumn.BindForeignKey(newRefer);
                }
            }

            return newTables;
        }
    }
}
