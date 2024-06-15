// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;

namespace CodeBuilder.Core.Source
{
    /// <summary>
    /// 提供对数据表的宿主。
    /// </summary>
    public sealed class Host
    {
        public Host()
        {
            Tables = new List<Table>();
        }

        /// <summary>
        /// 获取数据表集合。
        /// </summary>
        [TypeConverter(typeof(Designer.CollectionConverter))]
        [Editor(typeof(Designer.DisableViewEditor), typeof(UITypeEditor))]
        public List<Table> Tables { get; private set; }

        /// <summary>
        /// 将数据表附加到集合中。
        /// </summary>
        /// <param name="table"></param>
        public void Attach(Table table)
        {
            Tables.Add(table);
            table.Host = this;
        }
    }
}
