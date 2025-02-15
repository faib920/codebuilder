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
    /// 索引。
    /// </summary>
    public class Index : IModeView
    {
        public Index(string name, List<IndexColumn> columns)
        {
            Columns = columns;
            Name = name;
        }

        public Index(string name)
            : this(name, new List<IndexColumn>())
        {
        }

        public Index()
            : this(null)
        {
        }

        /// <summary>
        /// 名称。
        /// </summary>
        [Description("名称。")]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置是否为唯一键。
        /// </summary>
        [Description("是否为唯一键。")]
        public bool IsUniqueKey { get; set; }

        /// <summary>
        /// 索引字段集合。
        /// </summary>
        [Description("索引字段集合。")]
        public List<IndexColumn> Columns { get; private set; }

        public IndexColumn AddColumn(Column column)
        {
            var ic = new IndexColumn(column);
            Columns.Add(ic);
            return ic;
        }

        string IModeView.GetDisplayName(string view)
        {
            if (Columns == null)
            {
                return "(None)";
            }

            return $"{Name}: {string.Join(", ", Columns.Select(s => s.Name))}";
        }
    }
}
