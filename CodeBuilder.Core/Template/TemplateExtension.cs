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
    /// 模板扩展。
    /// </summary>
    public class TemplateExtension
    {
        /// <summary>
        /// 使用基础的扩展文件。
        /// </summary>
        public bool UseBase { get; set; } = true;

        /// <summary>
        /// 获取或设置公共扩展文件列表。
        /// </summary>
        public List<string> Common { get; set; }

        /// <summary>
        /// 获取或设置架构扩展文件列表。
        /// </summary>
        public List<string> Schema { get; set; }

        /// <summary>
        /// 获取或设置变量扩展文件列表。
        /// </summary>
        public List<string> Profile { get; set; }
    }
}
