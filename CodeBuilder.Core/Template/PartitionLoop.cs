// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace CodeBuilder.Core.Template
{
    /// <summary>
    /// 部件循环的类别。
    /// </summary>
    public enum PartitionLoop
    {
        /// <summary>
        /// 非循环。
        /// </summary>
        None,
        /// <summary>
        /// 循环所有数据表。
        /// </summary>
        Tables,
        /// <summary>
        /// 循环所有关系。
        /// </summary>
        References
    }
}
