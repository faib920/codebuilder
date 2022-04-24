// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Source;
using CodeBuilder.Core.Variable;
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
        /// <param name="tables"></param>
        /// <returns></returns>
        public static List<Table> Refactoring(List<Table> tables)
        {
            var newTables = new List<Table>();

            foreach (var table in tables)
            {
                var newTable = SchemaExtensionManager.Build<Table>();
                newTable.Refactoring(table, () => SchemaExtensionManager.Build<Column>());
                newTables.Add(newTable);
            }

            foreach (var table in tables)
            {
                var newTable = newTables.FirstOrDefault(s => s.Name == table.Name);

                foreach (var column in table.Columns.Where(s => s.ForeignKey != null))
                {
                    var newColumn = newTable.Columns.FirstOrDefault(s => s.Name == column.Name);

                    Table pkTable = null, fkTable = null;
                    Column pkColumn = null, fkColumn = null;
                    var fk = column.ForeignKey;
                    if (fk.PkTable != null)
                    {
                        pkTable = newTables.FirstOrDefault(s => s.Name == fk.PkTable.Name);

                        if (fk.PkColumn != null && pkTable != null)
                        {
                            pkColumn = pkTable.Columns.FirstOrDefault(s => s.Name == fk.PkColumn.Name);
                        }
                    }

                    if (fk.FkTable != null)
                    {
                        fkTable = newTables.FirstOrDefault(s => s.Name == fk.FkTable.Name);

                        if (fk.FkColumn != null && fkTable != null)
                        {
                            fkColumn = fkTable.Columns.FirstOrDefault(s => s.Name == fk.FkColumn.Name);
                        }
                    }

                    var newRefer = SchemaExtensionManager.Build<Reference>(pkTable, pkColumn, fkTable, fkColumn);
                    newRefer.Name = fk.Name;

                    newColumn.BindForeignKey(newRefer);
                }
            }

            return newTables;
        }
    }
}
