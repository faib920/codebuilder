// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;

namespace CodeBuilder.Core.Source
{
    /// <summary>
    /// 索引字段
    /// </summary>
    public class IndexColumn
    {
        public IndexColumn(Column column) 
        { 
            Name = column.Name;
        }

        public IndexColumn() { }

        /// <summary>
        /// 名称。
        /// </summary>
        [Description("名称。")]
        public string Name { get; private set; }

        /// <summary>
        /// 排序，ASC或DESC。
        /// </summary>
        [Description("排序，ASC或DESC。")]
        public string SortOrder { get; set; }
    }
}
