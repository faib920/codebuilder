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
    /// 模板文件。
    /// </summary>
    public class TemplateFile
    {
        public TemplateFile(string name)
        {
            Name = name;
        }

        public TemplateFile(string name, string fileName)
            : this (name)
        {
            FileName = fileName;
        }

        public TemplateFile(string name, string fileName, string language)
            : this(name, fileName)
        {
            Language = language;
        }

        /// <summary>
        /// 获取或设置名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置文件名称。
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 获取或设置文件使用的语言。
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// 获取或设置颜色。
        /// </summary>
        public string Color { get; set; }
    }
}
