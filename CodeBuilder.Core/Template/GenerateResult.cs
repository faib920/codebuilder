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
    /// 生成结果。
    /// </summary>
    public class GenerateResult
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="part">部件。</param>
        /// <param name="content">生成的内容。</param>
        public GenerateResult(PartitionDefinition part, string content)
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
    }
}
