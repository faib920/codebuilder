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
    /// 部件写入器。
    /// </summary>
    public class PartitionWriter
    {
        private static Parser _paser = new Parser();

        /// <summary>
        /// 清除所有缓存。
        /// </summary>
        public static void ClearCache()
        {
            _paser = new Parser();
        }

        /// <summary>
        /// 将生成的结果写入到磁盘文件中。
        /// </summary>
        /// <param name="result"></param>
        /// <param name="schema">架构对象。</param>
        /// <param name="profile">变量对象。</param>
        /// <param name="output">输出的文件路径。</param>
        public static void Write(GenerateResult result, object schema, object profile, string output)
        {
            var fileName = result.Partition.Output;
            fileName = _paser.Parse(schema, profile, fileName);

            var path = Path.Combine(output, fileName);
            var dir = path.Substring(0, path.LastIndexOf("\\"));
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            File.WriteAllText(path, result.Content, StaticUnity.Encoding);
        }
    }
}
