// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Source;
using CodeBuilder.Core.Template;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeBuilder.Core
{
    /// <summary>
    /// 模板提供者插件。
    /// </summary>
    public interface ITemplateProvider : IPlugin
    {
        /// <summary>
        /// 获取插件工作目录。
        /// </summary>
        string WorkDir { get; }

        /// <summary>
        /// 生成代码文件。
        /// </summary>
        /// <param name="option">模板选项。</param>
        /// <param name="tables">所选定的数据表。</param>
        /// <param name="handler">代码生成进度通知。</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<GenerateResult> GenerateFilesAsync(TemplateOption option, List<Table> tables, CodeGenerateHandler handler, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取所有模板定义。
        /// </summary>
        /// <returns></returns>
        List<TemplateDefinition> GetTemplates();

        /// <summary>
        /// 获取模板定义存储信息。
        /// </summary>
        /// <param name="definition"></param>
        /// <returns></returns>
        TemplateStorage GetStorage(TemplateDefinition definition);
    }
}
