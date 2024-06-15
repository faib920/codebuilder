// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System.Collections.Generic;

namespace CodeBuilder.Core.Template
{
    /// <summary>
    /// 生成结果。
    /// </summary>
    public class GenerateResult
    {
        /// <summary>
        /// 获取或设置是否有错误。
        /// </summary>
        public bool HasError { get; set; }

        public List<GeneratePartitionResult> Partitions { get; set; } = new List<GeneratePartitionResult>();
    }

    public class GeneratePartitionResult
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="part">部件。</param>
        /// <param name="content">生成的内容。</param>
        public GeneratePartitionResult(PartitionDefinition part, string content)
        {
            Partition = part;
            Content = content;
            WriteToDisk = !string.IsNullOrEmpty(part.FilePath);
        }

        /// <summary>
        /// 获取生成的内容。
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// 获取生成的部件。
        /// </summary>
        public PartitionDefinition Partition { get; private set; }

        /// <summary>
        /// 获取是否写入到磁盘。
        /// </summary>
        public bool WriteToDisk { get; private set; }

        /// <summary>
        /// 清空内容。
        /// </summary>
        public void Clear()
        {
            Content = null;
        }
    }
}
