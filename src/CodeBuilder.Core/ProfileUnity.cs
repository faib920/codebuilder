// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Initializers;
using CodeBuilder.Core.Template;
using CodeBuilder.Core.Variable;
using Fireasy.Common.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            var profileExtManager = hosting.ServiceProvider.TryGetService<IProfileExtensionManager>();
            string fileName;
            var profile = profileExtManager.Build(template);
            if (profile == null)
            {
                return null;
            }

            if (!string.IsNullOrEmpty(template?.TId))
            {
                fileName = Path.Combine(hosting.WorkPath, "profiles", "template." + template.TId + ".profile");
                if (File.Exists(fileName))
                {
                    return InitializerUnity.Initialize(hosting, FillByFile(profile, fileName), template);
                }
            }

            fileName = Path.Combine(hosting.WorkPath, "config", "profile.cfg");
            return InitializerUnity.Initialize(hosting, FillByFile(profile, fileName), template);
        }

        /// <summary>
        /// 载入变量对象。
        /// </summary>
        /// <param name="hosting">开发环境。</param>
        /// <param name="template">模板定义。</param>
        /// <param name="profileName">变量存储文件。</param>
        /// <returns></returns>
        public static Profile LoadProfile(IDevHosting hosting, TemplateDefinition template, string profileName)
        {
            if (File.Exists(profileName))
            {
                var profileExtManager = hosting.ServiceProvider.TryGetService<IProfileExtensionManager>();

                var profile = profileExtManager.Build(template);
                if (profile == null)
                {
                    return null;
                }

                return InitializerUnity.Initialize(hosting, FillByFile(profile, profileName), template);
            }

            return LoadProfile(hosting, template);
        }

        /// <summary>
        /// 将变量保存到本地。
        /// </summary>
        /// <param name="hosting">开发环境。</param>
        /// <param name="templateUId">模板UID。</param>
        /// <param name="profile">变量。</param>
        public static void SaveFile(IDevHosting hosting, string templateUId, Profile profile)
        {
            string fileName;
            if (string.IsNullOrEmpty(templateUId))
            {
                fileName = Path.Combine(hosting.WorkPath, "config", "profile.cfg");
            }
            else
            {
                var path = Path.Combine(hosting.WorkPath, "profiles");
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
            var dyobj = JsonConvert.DeserializeObject<JObject>(content);

            if (profile is IProfileInfo info)
            {
                info.FileName = filePath;
            }

            SetPropertyValue(profileType, profile, dyobj);

            return profile;
        }

        private static void SetPropertyValue(Type type, object obj, JObject jobj)
        {
            var members = type.GetProperties().Where(s => s.IsDefined<UnPersistentlyAttribute>()).Select(s => s.Name).ToList();

            foreach (var p in jobj.Properties())
            {
                var property = type.GetProperty(p.Name);
                if (property != null && property.CanWrite && !members.Contains(p.Name))
                {
                    var pvalue = jobj.GetValue(p.Name);
                    if (pvalue is JValue jvalue)
                    {
                        property.SetValue(obj, jvalue.Value.To(property.PropertyType), null);
                    }
                    else if (pvalue is JObject jobj1)
                    {
                        throw new NotImplementedException($"无法读取属性 {p.Name} 的值，暂不支持嵌套对象。");

                        //var o = Activator.CreateInstance(property.PropertyType);
                        //property.SetValue(obj, o);
                        //SetPropertyValue(property.PropertyType, o, jobj1);
                    }
                }
            }
        }

        /// <summary>
        /// 将变量保存到文件。
        /// </summary>
        /// <param name="profile">变量。</param>
        /// <param name="filePath">文件路径。</param>
        public static void SaveAsFile(Profile profile, string filePath)
        {
            var content = JsonConvert.SerializeObject(profile);

            Util.TryOperateFile(filePath, () => File.WriteAllText(filePath, content, Encoding.UTF8));
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