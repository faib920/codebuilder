// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Initializers;
using Fireasy.Common.Emit;
using Fireasy.Common.Extensions;
using Microsoft.CSharp;
using Microsoft.VisualBasic;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CodeBuilder.Core.Variable
{
    /// <summary>
    /// 变量扩展管理类。
    /// </summary>
    public class ProfileExtensionManager
    {
        private static Type _wrapType;
        private static List<Type> _extendTypes = new List<Type>();
        private static List<PropertyMap> _propertyCache = null;
        private static string _templateUId = string.Empty;

        /// <summary>
        /// 编译生成一个变量。
        /// </summary>
        /// <param name="templateUId">模板UID。</param>
        /// <returns></returns>
        public static Profile Build(string templateUId)
        {
            if (_templateUId != templateUId)
            {
                _propertyCache = null;
                _wrapType = null;
                _extendTypes = ComplileExtensionTypes(templateUId);
                _wrapType = GetWrapType();
                _templateUId = templateUId;
            }

            return InitializeDefaultValue(_wrapType?.New<Profile>());
        }

        /// <summary>
        /// 获取变量的包装类。
        /// </summary>
        /// <returns></returns>
        public static Type GetWrapType()
        {
            var properties = _extendTypes.SelectMany(s => s.GetProperties(BindingFlags.Public | BindingFlags.Instance)).ToArray();

            if (properties.Length > 0)
            {
                try
                {
                    return BuildWrapType(properties);
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取变量的所有属性映射。
        /// </summary>
        /// <returns></returns>
        public static List<PropertyMap> GetPropertyMaps()
        {
            if (_propertyCache == null)
            {
                var type = GetWrapType();
                _propertyCache = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(s => s.Name != "_ID").Select(s => new PropertyMap(s)).ToList();
            }

            return _propertyCache;
        }

        /// <summary>
        /// 动态编译扩展动态类。
        /// </summary>
        /// <returns></returns>
        private static List<Type> ComplileExtensionTypes(string templateUId)
        {
            var pluginTypes = new List<Type>();
            foreach (var ext in new[] { ".cs", ".vb" })
            {
                var files = GetExtensionFiles(templateUId, ext);

                if (files.Length == 0)
                {
                    continue;
                }

                var fileName = Util.GenerateTempFileName();
                StaticUnity.DynamicAssemblies.Add(fileName);

                var compiler = new Fireasy.Common.Compiler.CodeCompiler();

                compiler.OutputAssembly = fileName;
                compiler.CodeProvider = GetCodeProvider(ext);
                foreach (var ass in AssemblyReferenceManager.ProfileAssemblies)
                {
                    compiler.Assemblies.Add(ass);
                }
                compiler.Assemblies.AddRange(CommonExtensionManager.GetCommonDynamicAssemblies());
                pluginTypes.AddRange(compiler.CompileAssembly(files).GetExportedTypes());
            }

            pluginTypes.Where(s => typeof(IProfileInitializer).IsAssignableFrom(s)).ForEach(s => InitializerUnity.Register(s.New<IProfileInitializer>()));

            return pluginTypes;
        }

        /// <summary>
        /// 获取目录下的可扩展的代码文件。
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        private static string[] GetExtensionFiles(string templateUId, string ext)
        {
            var list = new List<string>();

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "extensions\\profile");
            if (Directory.Exists(path))
            {
                if (string.IsNullOrEmpty(templateUId))
                {
                    return Directory.GetFiles(path, "*" + ext).Where(s => !Path.GetFileName(s).StartsWith("template.", StringComparison.OrdinalIgnoreCase)).ToArray();
                }
                else
                {
                    return Directory.GetFiles(path, "*" + ext).Where(s => !Path.GetFileName(s).StartsWith("template.", StringComparison.OrdinalIgnoreCase) || Path.GetFileName(s).Equals("template." + templateUId + ext, StringComparison.OrdinalIgnoreCase)).ToArray();
                }
            }

            return new string[0];
        }

        /// <summary>
        /// 根据文件扩展名获取相应的 <see cref="CodeDomProvider"/>。
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        private static CodeDomProvider GetCodeProvider(string ext)
        {
            if (ext == ".vb")
            {
                return new VBCodeProvider();
            }

            return new CSharpCodeProvider();
        }

        /// <summary>
        /// 初始化对象的默认值。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static Profile InitializeDefaultValue(Profile obj)
        {
            if (obj == null)
            {
                return null;
            }

            var map = _wrapType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(s => s.CanWrite)
                .Select(s => new { Property = s, DefaultValue = s.GetCustomAttributes<DefaultValueAttribute>().FirstOrDefault() })
                .Where(s => s.DefaultValue != null)
                .ToArray();

            foreach (var item in map)
            {
                item.Property.FastSetValue(obj, item.DefaultValue.Value);
            }

            return obj;
        }

        /// <summary>
        /// 使用扩展属性对架构类进行包装。
        /// </summary>
        /// <param name="schemaType">架构类。</param>
        /// <param name="properties">扩展的属性列表。</param>
        /// <returns></returns>
        private static Type BuildWrapType(PropertyInfo[] properties)
        {
            var dyAssemblyBuilder = new DynamicAssemblyBuilder("ProfileExtension");
            var dyTypeBuilder = dyAssemblyBuilder.DefineType("ProfileEx", baseType: typeof(Profile));

            foreach (var property in properties)
            {
                var dyPropertyBuilder = dyTypeBuilder.DefineProperty(property.Name, property.PropertyType);
                dyPropertyBuilder.SetCustomAttribute<ExtendPropertyAttribute>();
                dyPropertyBuilder.DefineGetSetMethods();
                SetPropertyCustomAttributes(property, dyPropertyBuilder);
            }

            var wrapType = dyTypeBuilder.CreateType();
            _propertyCache = wrapType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(s => new PropertyMap(s)).ToList();
            return wrapType;
        }

        /// <summary>
        /// 设置属性的自定义特性。
        /// </summary>
        /// <param name="property"></param>
        /// <param name="dyPropertyBuilder"></param>
        private static void SetPropertyCustomAttributes(PropertyInfo property, DynamicPropertyBuilder dyPropertyBuilder)
        {
            var broAttr = property.GetCustomAttributes<BrowsableAttribute>().FirstOrDefault();
            var catAttr = property.GetCustomAttributes<CategoryAttribute>().FirstOrDefault();
            var desAttr = property.GetCustomAttributes<DescriptionAttribute>().FirstOrDefault();
            var defAttr = property.GetCustomAttributes<DefaultValueAttribute>().FirstOrDefault();
            var disAttr = property.GetCustomAttributes<DisplayNameAttribute>().FirstOrDefault();
            var reqAttr = property.GetCustomAttributes<RequiredCheckAttribute>().FirstOrDefault();
            var unpAttr = property.GetCustomAttributes<UnPersistentlyAttribute>().FirstOrDefault();

            if (broAttr != null)
            {
                dyPropertyBuilder.SetCustomAttribute<BrowsableAttribute>(broAttr.Browsable);
            }

            if (catAttr != null)
            {
                dyPropertyBuilder.SetCustomAttribute<CategoryAttribute>(catAttr.Category);
            }

            if (desAttr != null)
            {
                dyPropertyBuilder.SetCustomAttribute<DescriptionAttribute>(desAttr.Description);
            }

            if (defAttr != null)
            {
                dyPropertyBuilder.SetCustomAttribute<DefaultValueAttribute>(defAttr.Value);
            }

            if (disAttr != null)
            {
                dyPropertyBuilder.SetCustomAttribute<DisplayNameAttribute>(disAttr.DisplayName);
            }

            if (reqAttr != null)
            {
                dyPropertyBuilder.SetCustomAttribute<RequiredCheckAttribute>();
            }

            if (unpAttr != null)
            {
                dyPropertyBuilder.SetCustomAttribute<UnPersistentlyAttribute>();
            }
        }
    }
}
