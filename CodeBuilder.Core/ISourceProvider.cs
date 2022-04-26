// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CodeBuilder.Core.Source;
using System.Collections.Generic;

namespace CodeBuilder.Core
{
    /// <summary>
    /// 数据源提供者插件。
    /// </summary>
    public interface ISourceProvider : IPlugin
    {
        /// <summary>
        /// 连接数据源，获取预览表。
        /// </summary>
        /// <param name="option">选项。</param>
        /// <returns></returns>
        List<Table> Preview(SourceOption option);

        /// <summary>
        /// 获取指定表的架构。
        /// </summary>
        /// <param name="tables">选定的数据表。</param>
        /// <param name="processHandler">数据表的读取进度通知。</param>
        /// <returns></returns>
        List<Table> GetSchema(List<Table> tables, TableSchemaProcessHandler processHandler);

        /// <summary>
        /// 通过自定义文本解析表结构。
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        List<Table> ParseCustomContent(string content);

        /// <summary>
        /// 获取历史记录。
        /// </summary>
        /// <returns></returns>
        List<string> GetHistory();
    }
}
