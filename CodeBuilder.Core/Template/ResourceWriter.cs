// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System.IO;

namespace CodeBuilder.Core.Template
{
    /// <summary>
    /// 资源写入器。
    /// </summary>
    public static class ResourceWriter
    {
        /// <summary>
        /// 将模板定义中的资源输出到指定的目录下。
        /// </summary>
        /// <param name="template">模板定义。</param>
        /// <param name="profile">变量对象。</param>
        /// <param name="output">输出路径。</param>
        public static void Write(TemplateDefinition template, Profile profile, string output)
        {
            if (template.Resources.Count == 0)
            {
                return;
            }

            var path = Path.Combine(template.ConfigFileName.Substring(0, template.ConfigFileName.LastIndexOf(".")), "Resources");
            foreach (var res in template.Resources)
            {
                var source = Path.Combine(path, res);
                var desc = new Parser().Parse(null, profile, Path.Combine(output, res));
                var descDir = new FileInfo(desc).DirectoryName;
                if (!Directory.Exists(descDir))
                {
                    Directory.CreateDirectory(descDir);
                }

                File.Copy(source, desc, true);
            }
        }
    }
}
