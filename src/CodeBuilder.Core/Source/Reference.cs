// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Designer;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CodeBuilder.Core.Source
{
    /// <summary>
    /// 关系。
    /// </summary>
    public class Reference : SchemaBase, IModeView
    {
        public Reference()
        {
        }

        public Reference(Table pkTable, Column pkColumn, Table fkTable, Column fkColumn)
            : this ($"FK_{pkTable.Name}_{pkColumn.Name}", pkTable, pkColumn, fkTable, fkColumn)
        {
        }

        public Reference(string name, Table pkTable, Column pkColumn, Table fkTable, Column fkColumn)
        {
            Name = name;

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
        [Description("更新时的约束，0:无动作、1:限制、2:级联、3:设为NULL、4:设为默认值。")]
        public Constraint OnUpdate { get; set; }

        /// <summary>
        /// 获取或设置删除时的约束。
        /// </summary>
        [Description("删除时的约束，0:无动作、1:限制、2:级联、3:设为NULL、4:设为默认值。")]
        public Constraint OnDelete { get; set; }

        /// <summary>
        /// 获取或设置关系。
        /// </summary>
        [Description("关系，0:一对多、1:一对一、2:多对一、3:多对多。")]
        public Relationship Relationship { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Name) ? $"{PkTable.Name}.{PkColumn.Name}" : Name;
        }

        string IModeView.GetDisplayName(string view)
        {
            if (view == nameof(Table.ForeignKeys))
            {
                return string.IsNullOrEmpty(Name) ? $"{FkColumn.Name}" : $"{Name}({FkColumn.Name})";
            }
            else if (view == nameof(Table.SubKeys))
            {
                return string.IsNullOrEmpty(Name) ? $"{FkTable.Name}.{FkColumn.Name}" : $"{Name}({FkTable.Name}.{FkColumn.Name})";
            }

            return string.Empty;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Reference reobj))
            {
                return false;
            }

            return reobj.PkTable?._Name == PkTable?._Name && reobj.FkTable?._Name == FkTable?._Name &&
                reobj.PkColumn?._Name == PkColumn?._Name && reobj.FkColumn?._Name == FkColumn?._Name;
        }
    }

    /// <summary>
    /// 删除时，对于关系的约束。
    /// </summary>
    public enum Constraint
    {
        /// <summary>
        /// 无动作。
        /// </summary>
        NoAction,
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
        SetNull,
        /// <summary>
        /// 设为默认值
        /// </summary>
        SetDefault
    }

    /// <summary>
    /// 关系
    /// </summary>
    public enum Relationship
    {
        /// <summary>
        /// 一对多
        /// </summary>
        OneToMany,
        /// <summary>
        /// 一对一
        /// </summary>
        OneToOne,
        /// <summary>
        /// 多对一
        /// </summary>
        ManyToOne,
        /// <summary>
        /// 多对多
        /// </summary>
        ManyToMany,
    }
}
