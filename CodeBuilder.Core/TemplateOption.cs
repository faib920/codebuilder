// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Template;
using System.Collections.Generic;

namespace CodeBuilder.Core
{
    /// <summary>
    /// 生成代码时的模板选项。
    /// </summary>
    public class TemplateOption
    {
        /// <summary>
        /// 获取或设置模板定义。
        /// </summary>
        public TemplateDefinition Template { get; set; }

        /// <summary>
        /// 获取或设置要生成的部件。
        /// </summary>
        public List<PartitionDefinition> Partitions { get; set; }

        /// <summary>
        /// 获取或设置输出目录。
        /// </summary>
        public string OutputDirectory { get; set; }

        /// <summary>
        /// 获取动态程序集路径。
        /// </summary>
        public List<string> DynamicAssemblies { get; } = new List<string>();

        /// <summary>
        /// 获取或设置变量实例。
        /// </summary>
        public Profile Profile { get; set; }

        /// <summary>
        /// 获取或设置是否输出到磁盘。
        /// </summary>
        public bool WriteToDisk { get; set; }

        /// <summary>
        /// 获取或设置文件存在时是否跳过。
        /// </summary>
        public bool SkipWhenFileExists { get; set; }
    }
}
