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
using Fireasy.Common.DependencyInjection;
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
    public class ProfileExtensionManager : BaseExtensionManager, IProfileExtensionManager, ISingletonService
    {
        private static Type _wrapType;
        private static List<Type> _extendTypes = new List<Type>();
        private static HashSet<string> _namespaces = new HashSet<string>();
        private static List<string> _files = new List<string>();
        private static List<PropertyMap> _propertyCache = null;
        private readonly IServiceProvider _serviceProvider;

        public ProfileExtensionManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 编译生成一个变量。
        /// </summary>
        /// <param name="definition">模板定义。</param>
        public Profile Build(TemplateDefinition definition)
        {
            if (definition != null)
            {
                _propertyCache = null;
                _wrapType = null;
                var result = ComplileExtensionTypes(definition);
                _extendTypes = result.Types;
                _namespaces = result.Namespaces;
                _files = result.Files;
                _wrapType = GetWrapType();

                FindPartitionOutputParsers();
            }

            if (_wrapType == null)
            {
                return null;
            }

            return InitializeDefaultValue((Profile)Activator.CreateInstance(_wrapType), _wrapType) ?? new Profile();
        }

        /// <summary>
        /// 获取变量的包装类。
        /// </summary>
        /// <returns></returns>
        public Type GetWrapType()
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
        public List<PropertyMap> GetPropertyMaps()
        {
            if (_propertyCache == null)
            {
                var type = GetWrapType();
                if (type == null)
                {
                    return null;
                }

                _propertyCache = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(s => s.Name != "_ID").Select(s => new PropertyMap(s)).ToList();
            }

            return _propertyCache;
        }

        /// <summary>
        /// 查找 <see cref="IPartitionOutputParser"/>。
        /// </summary>
        private void FindPartitionOutputParsers()
        {
            foreach (var type in _extendTypes)
            {
                if (typeof(IPartitionOutputParser).IsAssignableFrom(type))
                {
                    var parser = Activator.CreateInstance(type) as IPartitionOutputParser;
                    Parser.AddParser(parser);
                }
            }
        }

        /// <summary>
        /// 动态编译扩展动态类。
        /// </summary>
        /// <param name="definition">模板定义。</param>
        /// <returns></returns>
        private CompileResult ComplileExtensionTypes(TemplateDefinition definition)
        {
            var files = CompileHelper.GetExtensionFiles(definition, "profile", s => s.Profile);

            if (files.Length == 0)
            {
                return new CompileResult();
            }

            return Initialize(CompileHelper.CompileTypes(_serviceProvider, definition, files, AssemblyReferenceManager.ProfileAssemblies));
        }

        private CompileResult Initialize(CompileResult result)
        {
            result.Types.Where(s => typeof(IProfileInitializer).IsAssignableFrom(s)).ForEach(s => InitializerUnity.Register(Activator.CreateInstance(s) as IProfileInitializer));
            return result;
        }

        /// <summary>
        /// 使用扩展属性对架构类进行包装。
        /// </summary>
        /// <param name="schemaType">架构类。</param>
        /// <param name="properties">扩展的属性列表。</param>
        /// <returns></returns>
        private Type BuildWrapType(PropertyInfo[] properties)
        {
            /*
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
            */

            var source = $@"
{string.Join(Environment.NewLine, _namespaces.Select(s => "using " + s + ";"))}

public class Profile_Wrap : CodeBuilder.Core.Profile
{{
public Profile_Wrap() {{}}
{string.Join(Environment.NewLine, properties.Select(s => string.Join(Environment.NewLine, GetPropertyCustomAttributes(s)) + Environment.NewLine + "public " + s.PropertyType.Name + " " + s.Name + " { get; set; }" ))}
}}
";

            return CompileHelper.CompileType(_serviceProvider, source, AssemblyReferenceManager.ProfileAssemblies.Union(AssemblyReferenceManager.CommonAssemblies).Union(LocalDynamicCache.CommonAssemblies).Union(_files).Distinct().ToArray());
        }

        /// <summary>
        /// 设置属性的自定义特性。
        /// </summary>
        /// <param name="property"></param>
        /// <param name="dyPropertyBuilder"></param>
        private void SetPropertyCustomAttributes(PropertyInfo property, DynamicPropertyBuilder dyPropertyBuilder)
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

            if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
            {
                dyPropertyBuilder.SetCustomAttribute<BrowsableAttribute>(false);
            }
        }

        /// <summary>
        /// 获取属性的自定义特性。
        /// </summary>
        /// <param name="property"></param>
        /// <param name="dyPropertyBuilder"></param>
        private List<string> GetPropertyCustomAttributes(PropertyInfo property)
        {
            var attributes = new List<string>();
            var broAttr = property.GetCustomAttributes<BrowsableAttribute>().FirstOrDefault();
            var catAttr = property.GetCustomAttributes<CategoryAttribute>().FirstOrDefault();
            var desAttr = property.GetCustomAttributes<DescriptionAttribute>().FirstOrDefault();
            var defAttr = property.GetCustomAttributes<DefaultValueAttribute>().FirstOrDefault();
            var disAttr = property.GetCustomAttributes<DisplayNameAttribute>().FirstOrDefault();
            var reqAttr = property.GetCustomAttributes<RequiredCheckAttribute>().FirstOrDefault();
            var unpAttr = property.GetCustomAttributes<UnPersistentlyAttribute>().FirstOrDefault();

            if (broAttr != null)
            {
                attributes.Add($"[BrowsableAttribute({(broAttr.Browsable ? "true" : "false")})]");
            }

            if (catAttr != null)
            {
                attributes.Add($"[CategoryAttribute(\"{catAttr.Category}\")]");
            }

            if (desAttr != null)
            {
                attributes.Add($"[DescriptionAttribute(\"{desAttr.Description}\")]");
            }

            if (defAttr != null)
            {
                attributes.Add($"[DefaultValueAttribute(typeof({property.PropertyType.Name}), \"{defAttr.Value}\")]");
            }

            if (disAttr != null)
            {
                attributes.Add($"[DisplayNameAttribute(\"{disAttr.DisplayName}\")]");
            }

            if (reqAttr != null)
            {
                attributes.Add($"[RequiredCheckAttribute]");
            }

            if (unpAttr != null)
            {
                attributes.Add($"[UnPersistentlyAttribute]");
            }

            if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
            {
                attributes.Add($"[BrowsableAttribute(false)]");
            }

            return attributes;
        }
    }
}
