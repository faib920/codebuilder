// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Initializers;
using CodeBuilder.Core.Template;
using CodeBuilder.Core.Variable;
using Fireasy.Common.Extensions;
using Fireasy.Common.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CodeBuilder.Core
{
    /// <summary>
    /// 变量辅助类。
    /// </summary>
    public class ProfileUnity
    {
        /// <summary>
        /// 载入变量对象。
        /// </summary>
        /// <param name="hosting">开发环境。</param>
        /// <param name="template">模板定义。</param>
        /// <returns></returns>
        public static Profile LoadProfile(IDevHosting hosting, TemplateDefinition template)
        {
            string fileName;
            var profile = ProfileExtensionManager.Build(template?.TId);
            if (!string.IsNullOrEmpty(template?.TId))
            {
                fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "profiles", "template." + template.TId + ".profile");
                if (File.Exists(fileName))
                {
                    return InitializerUnity.Initialize(hosting, FillByFile(profile, fileName), template);
                }
            }

            fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "profile.cfg");
            return InitializerUnity.Initialize(hosting, FillByFile(profile, fileName), template);
        }

        /// <summary>
        /// 将变量保存到本地。
        /// </summary>
        /// <param name="templateUId">模板UID。</param>
        /// <param name="profile">变量。</param>
        public static void SaveFile(string templateUId, Profile profile)
        {
            string fileName;
            if (string.IsNullOrEmpty(templateUId))
            {
                fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "profile.cfg");
            }
            else
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "profiles");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                fileName = Path.Combine(path, "template." + templateUId + ".profile");
            }

            SaveAsFile(profile, fileName);
        }

        /// <summary>
        /// 使用本地文件来填充变量。
        /// </summary>
        /// <param name="profile">变量。</param>
        /// <param name="filePath">文件路径。</param>
        /// <returns></returns>
        public static Profile FillByFile(Profile profile, string filePath)
        {
            var profileType = profile.GetType();
            var content = File.ReadAllText(filePath);
            var json = new JsonSerializer();
            var dyobj = (IDictionary<string, object>)json.Deserialize<dynamic>(content);

            foreach (var kvp in dyobj)
            {
                var p = profileType.GetProperty(kvp.Key);
                if (p != null && p.CanWrite)
                {
                    p.SetValue(profile, kvp.Value, null);
                }
            }

            return profile;
        }

        /// <summary>
        /// 将变量保存到文件。
        /// </summary>
        /// <param name="profile">变量。</param>
        /// <param name="filePath">文件路径。</param>
        public static void SaveAsFile(Profile profile, string filePath)
        {
            var option = new JsonSerializeOption { Indent = true };

            var members = profile.GetType().GetProperties().Where(s => s.IsDefined<UnPersistentlyAttribute>()).OfType<MemberInfo>().ToList();
            option.ExclusiveMembers = members;
            var json = new JsonSerializer(option);
            var content = json.Serialize(profile);
            File.WriteAllText(filePath, content, Encoding.UTF8);
        }

        /// <summary>
        /// 验证变量中的属性必填项。
        /// </summary>
        /// <param name="profile">变量。</param>
        /// <returns></returns>
        public static string ValidateProfile(Profile profile)
        {
            var sb = new StringBuilder();
            var properties = profile.GetType().GetProperties().Where(s => s.IsDefined<RequiredCheckAttribute>()).ToList();
            foreach (var property in properties)
            {
                var value = property.GetValue(profile);
                if (value == null)
                {
                    sb.AppendLine(string.Format("{0} 为空", property.Name));
                }
            }

            return sb.ToString();
        }
    }
}