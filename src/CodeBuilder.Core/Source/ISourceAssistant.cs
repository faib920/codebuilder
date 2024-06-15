// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System.Collections.Generic;

namespace CodeBuilder.Core.Source
{
    /// <summary>
    /// 数据源助手。
    /// </summary>
    public interface ISourceAssistant
    {
        /// <summary>
        /// 检查是否可为主键。
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        bool IsPrimaryKey(Column column);

        /// <summary>
        /// 查找外键关系。
        /// </summary>
        /// <param name="column"></param>
        /// <param name="tables"></param>
        /// <returns></returns>
        Reference FindForeignKey(Column column, IEnumerable<Table> tables);
    }
}
