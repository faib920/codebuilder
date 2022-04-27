// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CodeBuilder.Core.Source
{
    /// <summary>
    /// 关系。
    /// </summary>
    public class Reference : SchemaBase
    {
        public Reference()
        {
        }

        public Reference(Table pkTable, Column pkColumn, Table fkTable, Column fkColumn)
        {
            PkTable = pkTable;
            PkColumn = pkColumn;
            FkTable = fkTable;
            FkColumn = fkColumn;
        }

        /// <summary>
        /// 获取或设置外键的名称。
        /// </summary>
        [Description("外键的名称。")]
        public string Name { get; set; }

        /// <summary>
        /// 获取关联的数据表。
        /// </summary>
        [Description("关联的数据表。")]
        [Browsable(false)]
        public Table FkTable { get; private set; }

        /// <summary>
        /// 获取关联的字段。
        /// </summary>
        [Browsable(false)]
        [Description("关联的字段。")]
        public Column FkColumn { get; private set; }

        /// <summary>
        /// 获取主表。
        /// </summary>
        [Browsable(false)]
        [Description("主表。")]
        public Table PkTable { get; private set; }

        /// <summary>
        /// 获取主表字段。
        /// </summary>
        [Browsable(false)]
        [Description("主表字段。")]
        public Column PkColumn { get; private set; }

        /// <summary>
        /// 获取或设置更新时的约束。
        /// </summary>
        [Description("更新时的约束。")]
        public Constraint OnUpdate { get; set; }

        /// <summary>
        /// 获取或设置删除时的约束。
        /// </summary>
        [Description("删除时的约束。")]
        public Constraint OnDelete { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Name) ? string.Format("{0}.{1}", PkTable.Name, PkColumn.Name) : Name;
        }
    }

    /// <summary>
    /// 删除时，对于关系的约束。
    /// </summary>
    public enum Constraint
    {
        /// <summary>
        /// 限制。
        /// </summary>
        Restrict,
        /// <summary>
        /// 级联。
        /// </summary>
        Cascade,
        /// <summary>
        /// 设置为null。
        /// </summary>
        SetNull
    }
}
