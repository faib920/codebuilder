// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Fireasy.Common;
using Fireasy.Common.Security;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace CodeBuilder.Core
{
    /// <summary>
    /// 辅助类。
    /// </summary>
    public class Util
    {
        /// <summary>
        /// 获取应用程序的图标。
        /// </summary>
        /// <returns></returns>
        public static System.Drawing.Icon GetIcon()
        {
            return Properties.Resources.App;
        }

        /// <summary>
        /// 生成一个临时文件的路径。
        /// </summary>
        /// <returns></returns>
        public static string GenerateTempFileName()
        {
            var tempPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "temp\\codebuilder");
            Directory.CreateDirectory(tempPath);

            return Path.Combine(tempPath, RandomGenerator.Create() + ".dll");
        }

        /// <summary>
        /// 清理所生成的所有临时文件。
        /// </summary>
        public static void ClearTempFiles()
        {
            var tempPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "temp\\codebuilder");
            if (Directory.Exists(tempPath))
            {
                Directory.Delete(tempPath, true);
            }
        }

        /// <summary>
        /// 获取类型名称。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetTypeName(Type type)
        {
            if (type.IsGenericType)
            {
                var sb = new StringBuilder();
                var typeName = type.GetGenericTypeDefinition().Name;
                sb.AppendFormat("{0}<", typeName.Substring(0, typeName.IndexOf('`')));
                var assert = new AssertFlag();
                foreach (var t in type.GetGenericArguments())
                {
                    if (!assert.AssertTrue())
                    {
                        sb.Append(",");
                    }

                    sb.Append(GetTypeName(t));
                }

                sb.Append(">");
                return sb.ToString();
            }
            else
            {
                return type.Name;
            }
        }
    }
}
