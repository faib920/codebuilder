// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Source;
using CodeBuilder.Core.Validations;
using Fireasy.Common;
using Fireasy.Common.Extensions;
using Fireasy.Common.Security;
using Fireasy.Windows.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

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
        /// 获取工作目录。
        /// </summary>
        /// <returns></returns>
        public static string GetWorkPath()
        {
            var workPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".codebuilder3");

            if (!Directory.Exists(workPath))
            {
                workPath = Directory.GetCurrentDirectory();
            }

            return workPath;
        }

        /// <summary>
        /// 获取临时目录
        /// </summary>
        /// <returns></returns>
        public static string GetTempPath()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "codebuilder3");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }

        /// <summary>
        /// 获取临时目录
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static string GetTempPath(string directory)
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "codebuilder3", directory);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }

        /// <summary>
        /// 生成一个临时文件的路径。
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public static string GenerateTempFileName(out string assemblyName)
        {
            var tempPath = GetTempPath();

            assemblyName = RandomGenerator.Create();
            return Path.Combine(tempPath, assemblyName + ".dll");
        }

        /// <summary>
        /// 生成一个临时文件的路径。
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public static string GenerateTempFileName(string directory, out string assemblyName)
        {
            var tempPath = GetTempPath(directory);

            assemblyName = RandomGenerator.Create();
            return Path.Combine(tempPath, assemblyName + ".dll");
        }

        /// <summary>
        /// 清理所生成的所有临时文件。
        /// </summary>
        public static void ClearTempFiles()
        {
            var tempPath = GetTempPath("dynamic_funcs");
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

        public static void TryOperateFile(string fileName, Action action)
        {
            try
            {
                action();
            }
            catch (UnauthorizedAccessException exp)
            {
                ErrorMessageBox.Show("访问拒绝", new Exception("无法写入文件，请在快捷方式上弹出右键菜单，选择“属性”，在“兼容性”选项卡中勾选上“以管理员身份运行此程序”。", exp));
            }
        }

        /// <summary>
        /// 验证变量及所选定的数据表，使用 <see cref="RequiredCheckAttribute"/> 标记的属性为必填项。
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        public static string Validate(Profile profile)
        {
            var sb = new StringBuilder();

            foreach (var p in profile.GetType().GetProperties().Where(s => s.IsDefined<RequiredCheckAttribute>()))
            {
                var value = p.GetValue(profile);
                if (value == null || (value is string @string && string.IsNullOrEmpty(@string)))
                {
                    sb.AppendLine(string.Format("【变量】{0} 不能为空", p.Name));
                }
            }

            if (sb.Length > 0)
            {
                sb.Insert(0, "无法生成代码，以下项目验证失败：\n");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 验证变量及所选定的数据表，使用 <see cref="RequiredCheckAttribute"/> 标记的属性为必填项。
        /// </summary>
        /// <param name="hosting"></param>
        /// <param name="tables"></param>
        /// <returns></returns>
        public static ValidateResult Validate(IDevHosting hosting, IEnumerable<Table> tables)
        {
            if (hosting.Profile == null)
            {
                return ValidateResult.Success;
            }

            PropertyInfo[] tableProperties = null;
            PropertyInfo[] columnProperties = null;
            var result = new ValidateResult();

            foreach (var p in hosting.Profile.GetType().GetProperties().Where(s => s.IsDefined<RequiredCheckAttribute>()))
            {
                var value = p.GetValue(hosting.Profile);
                if (value == null || (value is string @string && string.IsNullOrEmpty(@string)))
                {
                    result.AddMessage(hosting.Profile, p.Name, "不能为空");
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
                        result.AddMessage(table, p.Name, "不能为空");
                    }
                }

                ValidateResult trlst = null;
                if (!(trlst = Validations.ValidationUnity.Validate(hosting, table)).IsSuccess)
                {
                    result.AddEntries(trlst.GetEntries());
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
                            result.AddMessage(column, c.Name, "不能为空");
                        }
                    }

                    ValidateResult crlst = null;
                    if (!(crlst = Validations.ValidationUnity.Validate(hosting, column)).IsSuccess)
                    {
                        result.AddEntries(crlst.GetEntries());
                    }
                }
            }

            return result;
        }

        public static void ShowValidation(IDevHosting hosting, ValidateResult result, Action<ValidateResult> showForm)
        {
            var entries = result.GetEntries();
            if (entries.All(s => s.Object == null))
            {
                hosting.ShowWarn("无法生成代码，以下项目校验失败:\n" + string.Join("\n", entries.Select(s => s.Message)));
            }
            else
            {
                showForm?.Invoke(result);
            }
        }

        public static Encoding GetEncoding(string encoding)
        {
            if (encoding.Equals("utf-8", StringComparison.OrdinalIgnoreCase))
            {
                return new UTF8Encoding(false);
            }

            return Encoding.GetEncoding(encoding);
        }

        internal static T AttachDevHosting<T>(T valiator, IDevHosting hosting)
        {
            var property = valiator.GetType().GetProperties().FirstOrDefault(s => s.PropertyType == typeof(IDevHosting));
            if (property != null && property.CanWrite)
            {
                var value = property.GetValue(valiator);
                if (value == null)
                {
                    property.SetValue(valiator, hosting);

                    var method = valiator.GetType().GetMethod("OnAttachHosting", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
                    if (method != null)
                    {
                        method.Invoke(valiator, null);
                    }
                }
            }

            return valiator;
        }

    }
}
