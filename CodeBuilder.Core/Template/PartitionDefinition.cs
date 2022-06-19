// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Fireasy.Common.Serialization;
using System.IO;

namespace CodeBuilder.Core.Template
{
    /// <summary>
    /// 部件定义。
    /// </summary>
    public class PartitionDefinition
    {
        /// <summary>
        /// 获取或设置部件名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置模板文件名称。
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 获取或设置模板文件路径。
        /// </summary>
        [NoTextSerializable]
        public string FilePath { get; set; }

        /// <summary>
        /// 获取或设置输出定义。
        /// </summary>
        public string Output { get; set; }

        /// <summary>
        /// 获取或设置循环的类别。
        /// </summary>
        public PartitionLoop Loop { get; set; }

        /// <summary>
        /// 获取或设置语法。
        /// </summary>
        public string Syntax { get; set; }

        /// <summary>
        /// 获取或设置颜色。
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// 获取模板文件的内容。
        /// </summary>
        [NoTextSerializable]
        public string Content
        {
            get
            {
                return File.ReadAllText(FilePath);
            }
        }
    }
}
