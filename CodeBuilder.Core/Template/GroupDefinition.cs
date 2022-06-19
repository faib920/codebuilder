// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System.Collections.Generic;

namespace CodeBuilder.Core.Template
{
    /// <summary>
    /// 组的定义。
    /// </summary>
    public class GroupDefinition
    {
        public GroupDefinition()
        {
            Groups = new List<GroupDefinition>();
            Partitions = new List<PartitionDefinition>();
        }

        /// <summary>
        /// 获取或设置组的名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置颜色。
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// 获取子级分组集合。
        /// </summary>
        public List<GroupDefinition> Groups { get; private set; }

        /// <summary>
        /// 获取部件集合。
        /// </summary>
        public List<PartitionDefinition> Partitions { get; private set; }
    }
}
