// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Source;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace CodeBuilder.Core
{
    /// <summary>
    /// 数据源提供者插件。
    /// </summary>
    public interface ISourceProvider : IPlugin
    {
        /// <summary>
        /// 获取图标。
        /// </summary>
        Image Icon { get; }

        /// <summary>
        /// 获取说明。
        /// </summary>
        string Description { get; }

        /// <summary>
        /// 连接数据源，获取预览表。
        /// </summary>
        /// <param name="option">选项。</param>
        /// <returns></returns>
        Task<List<Table>> PreviewAsync(SourceOption option, CancellationToken cancellationToken = default);

        /// <summary>
        /// 从历史中加载数据源
        /// </summary>
        /// <param name="history"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        Task<List<Table>> FromHistoryAsync(object history, SourceOption option, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取指定表的架构。
        /// </summary>
        /// <param name="tables">选定的数据表。</param>
        /// <param name="processHandler">数据表的读取进度通知。</param>
        /// <returns></returns>
        Task<List<Table>> GetSchemaAsync(List<Table> tables, TableSchemaProcessHandler processHandler, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取历史记录。
        /// </summary>
        /// <returns></returns>
        IEnumerable<object> GetHistory();

        /// <summary>
        /// 清空历史记录
        /// </summary>
        void ClearHistory();

        /// <summary>
        /// 删除历史记录
        /// </summary>
        void DeleteHistory(object record);
    }
}
