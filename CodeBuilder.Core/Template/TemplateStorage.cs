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
    /// 模板存储信息。
    /// </summary>
    public class TemplateStorage
    {
        /// <summary>
        /// 获取或设置模板目录列表。
        /// </summary>
        public List<TemplateDirectory> Directories { get; set; } = new List<TemplateDirectory>();

        /// <summary>
        /// 获取或设置模板文件列表。
        /// </summary>
        public List<TemplateFile> Files { get; set; } = new List<TemplateFile>();

        /// <summary>
        /// 从模板定义中获取所有的目录和文件。
        /// </summary>
        /// <param name="definition"></param>
        public void FromDefinition(TemplateDefinition definition)
        {
            FillFiles(Directories, Files, definition.Groups, definition.Partitions);
        }

        /// <summary>
        /// 递归获取目录和文件。
        /// </summary>
        /// <param name="dItems"></param>
        /// <param name="fItems"></param>
        /// <param name="groups"></param>
        /// <param name="partitions"></param>
        private void FillFiles(List<TemplateDirectory> dItems, List<TemplateFile> fItems, List<GroupDefinition> groups, List<PartitionDefinition> partitions)
        {
            foreach (var p in groups)
            {
                var dir = new TemplateDirectory(p.Name);
                dir.Color = p.Color;
                dItems.Add(dir);
                FillFiles(dir.Directories, dir.Files, p.Groups, p.Partitions);
            }

            foreach (var p in partitions)
            {
                fItems.Add(new TemplateFile(p.Name, p.FileName, p.FilePath, "C#") {  Color = p.Color });
            }
        }

    }
}
