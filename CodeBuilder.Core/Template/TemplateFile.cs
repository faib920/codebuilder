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

        public TemplateFile(string name, string filePath, string language)
            : this (name, string.Empty, filePath, language)
        {
            IsPublic = true;
        }

        public TemplateFile(string name, string fileName, string filePath, string language)
        {
            Name = name;
            FileName = fileName;
            FilePath = filePath;
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
        /// 获取或设置文件路径。
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 获取或设置文件使用的语言。
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// 获取或设置颜色。
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// 获取或设置是否公共文件。
        /// </summary>
        public bool IsPublic { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(FileName) ? Name : FileName + " (" + Name + ")";
        }
    }
}
