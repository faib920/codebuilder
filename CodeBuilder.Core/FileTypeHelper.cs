// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System.Collections.Generic;
using System.Linq;

namespace CodeBuilder.Core
{
    /// <summary>
    /// 文件类型辅助类。
    /// </summary>
    public class FileTypeHelper
    {
        private static Dictionary<string, string> _filters = new Dictionary<string, string>();

        static FileTypeHelper()
        {
            _filters.Add(".cs", "C#代码文件|*.cs");
            _filters.Add(".vb", "VB代码文件|*.vb");
            _filters.Add(".cfg", "配置文件|*.cfg");
            _filters.Add(".sql", "SQL脚本文件|*.sql");
            _filters.Add(".xml", "XML文档文件|*.xml");
            _filters.Add(".txt", "文本文件|*.txt");
            _filters.Add("-", "所有文件|*.*");
        }

        /// <summary>
        /// 注册文件扩展名。
        /// </summary>
        /// <param name="extension">扩展名。</param>
        /// <param name="descrition">文件类型描述。</param>
        public static void Register(string extension, string descrition)
        {
            if (_filters.ContainsKey(extension.ToLower()))
            {
                return;
            }

            _filters.Remove("-");
            _filters.Add(extension.ToLower(), descrition);
            _filters.Add("-", "所有文件|*.*");
        }

        /// <summary>
        /// 获取所有文件过滤符。
        /// </summary>
        /// <returns></returns>
        public static string GetAllFilters()
        {
            return string.Join("|", _filters.Select(s => s.Value));
        }

        /// <summary>
        /// 查找扩展名的索引。
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static int GetFilterIndex(string extension)
        {
            var index = 1;
            foreach (var kvp in _filters)
            {
                if (kvp.Key.Equals(extension, System.StringComparison.InvariantCultureIgnoreCase))
                {
                    return index;
                }

                index++;
            }

            return -1;
        }
    }
}
