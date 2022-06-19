// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Initializers;
using CodeBuilder.Core.Template;
using Fireasy.Common.Emit;
using Fireasy.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace CodeBuilder.Core.Variable
{
    /// <summary>
    /// 变量扩展管理类。
    /// </summary>
    public class ProfileExtensionManager : BaseExtensionManager
    {
        private static Type _wrapType;
        private static List<Type> _extendTypes = new List<Type>();
        private static List<PropertyMap> _propertyCache = null;

        /// <summary>
        /// 编译生成一个变量。
        /// </summary>
        /// <param name="definition">模板定义。</param>
        public static Profile Build(TemplateDefinition definition)
        {
            if (definition != null)
            {
                _propertyCache = null;
                _wrapType = null;
                _extendTypes = ComplileExtensionTypes(definition);
                _wrapType = GetWrapType();
            }

            return InitializeDefaultValue(_wrapType?.New<Profile>(), _wrapType) ?? new Profile();
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
        /// <param name="definition">模板定义。</param>
        /// <returns></returns>
        private static List<Type> ComplileExtensionTypes(TemplateDefinition definition)
        {
            var files = GetExtensionFiles(definition, "profile", s => s.Profile);

            if (files.Length == 0)
            {
                return new List<Type>();
            }

            return Initialize(CompileTypes(definition, files, AssemblyReferenceManager.ProfileAssemblies));
        }

        private static List<Type> Initialize(List<Type> pluginTypes)
        {
            pluginTypes.Where(s => typeof(IPartitionOutputParser).IsAssignableFrom(s)).ForEach(s => Parser.AddParser(s.New<IPartitionOutputParser>()));
            pluginTypes.Where(s => typeof(IProfileInitializer).IsAssignableFrom(s)).ForEach(s => InitializerUnity.Register(s.New<IProfileInitializer>()));
            return pluginTypes;
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
