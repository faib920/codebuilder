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
    /// 模板目录。
    /// </summary>
    public class TemplateDirectory
    {
        public TemplateDirectory(string name)
        {
            Name = name;
            Directories = new List<TemplateDirectory>();
            Files = new List<TemplateFile>();
        }

        /// <summary>
        /// 获取或设置目录名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取子目录列表。
        /// </summary>
        public List<TemplateDirectory> Directories { get; private set; }

        /// <summary>
        /// 获取文件列表。
        /// </summary>
        public List<TemplateFile> Files { get; private set; }
    }
}
