// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Source;
using Fireasy.Common.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CodeBuilder.Core
{
    /// <summary>
    /// 验证辅助类。
    /// </summary>
    public class ValidationUnity
    {
        /// <summary>
        /// 验证变量及所选定的数据表，使用 <see cref="RequiredCheckAttribute"/> 标记的属性为必填项。
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="tables"></param>
        /// <returns></returns>
        public static string Validate(Profile profile, IEnumerable<Table> tables)
        {
            PropertyInfo[] tableProperties = null;
            PropertyInfo[] columnProperties = null;
            var sb = new StringBuilder();

            foreach (var p in profile.GetType().GetProperties().Where(s => s.IsDefined<RequiredCheckAttribute>()))
            {
                var value = p.GetValue(profile);
                if (value == null || (value is string @string && string.IsNullOrEmpty(@string)))
                {
                    sb.AppendLine(string.Format("Profile 的 {0} 不能为空，请在【变量】窗口中设置；", p.Name));
                }
            }

            foreach (var table in tables)
            {
                if (tableProperties == null)
                {
                    tableProperties = table.GetType().GetProperties().Where(s => s.IsDefined<RequiredCheckAttribute>()).ToArray();
                }

                foreach (var p in tableProperties)
                {
                    var value = p.GetValue(table);
                    if (value == null || (value is string @string && string.IsNullOrEmpty(@string)))
                    {
                        sb.AppendLine(string.Format("表 {0} 的 {1} 不能为空，请在【属性】窗口中设置；", table.Name, p.Name));
                    }
                }

                foreach (var column in table.Columns)
                {
                    if (columnProperties == null)
                    {
                        columnProperties = column.GetType().GetProperties().Where(s => s.IsDefined<RequiredCheckAttribute>()).ToArray();
                    }

                    foreach (var c in columnProperties)
                    {
                        var value = c.GetValue(column);
                        if (value == null || (value is string @string && string.IsNullOrEmpty(@string)))
                        {
                            sb.AppendLine(string.Format("字段 {0}.{1} 的 {2} 不能为空，请在【属性】窗口中设置；", table.Name, column.Name, c.Name));
                        }
                    }
                }
            }

            if (sb.Length > 0)
            {
                sb.Insert(0, "必要字段校验结果：\n");
                sb.Remove(sb.Length - 3, 3);
                sb.Append("。");
            }

            return sb.ToString();
        }
    }
}
