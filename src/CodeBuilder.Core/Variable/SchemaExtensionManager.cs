// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Designer;
using CodeBuilder.Core.Initializers;
using CodeBuilder.Core.Source;
using CodeBuilder.Core.Template;
using CodeBuilder.Core.Validations;
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
    /// 架构扩展管理类。
    /// </summary>
    public class SchemaExtensionManager : BaseExtensionManager, ISchemaExtensionManager, ISingletonService
    {
        private static List<Type> _extendTypes = new List<Type>();
        private static HashSet<string> _namespaces = new HashSet<string>();
        private static List<string> _files = new List<string>();
        private static readonly Dictionary<Type, List<Type>> _extensionTypes = new Dictionary<Type, List<Type>>();
        private static readonly Dictionary<Type, Type> _cache = new Dictionary<Type, Type>();
        private static readonly Dictionary<Type, List<PropertyMap>> _propertyCache = new Dictionary<Type, List<PropertyMap>>();
        private readonly IServiceProvider _serviceProvider;

        public SchemaExtensionManager(IServiceProvider serviceProvider) 
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 初始化模板。
        /// </summary>
        /// <param name="definition">模板定义。</param>
        public void Initialize(TemplateDefinition definition)
        {
            if (definition != null)
            {
                _extendTypes.Clear();
                _extensionTypes.Clear();
                _cache.Clear();
                _propertyCache.Clear();

                var result = ComplileExtensionTypes(definition);
                _extendTypes = result.Types;
                _namespaces = result.Namespaces;
                _files = result.Files;
                _cache.Add(typeof(Table), GetWrapType<Table>());
                _cache.Add(typeof(Column), GetWrapType<Column>());
                _cache.Add(typeof(Reference), GetWrapType<Reference>());

                FindPartitionOutputParsers();
            }
        }

        /// <summary>
        /// 构造一个代理对象。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arguments">构造器参数。</param>
        /// <returns></returns>
        public T Build<T>(params object[] arguments)
        {
            try
            {
                var wrapType = GetWrapType<T>();
                return InitializeDefaultValue((T)Activator.CreateInstance(wrapType, arguments), wrapType);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// 获取包装过的类型。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Type GetWrapType<T>()
        {
            var schemaType = typeof(T);
            if (_cache.TryGetValue(schemaType, out Type wrapType))
            {
                return wrapType;
            }

            var extendTypes = FindExtensionTypes(schemaType);
            var properties = extendTypes.SelectMany(s => s.GetProperties(BindingFlags.Public | BindingFlags.Instance)).ToArray();
            if (properties.Length > 0)
            {
                return BuildWrapType(schemaType, properties);
            }

            return typeof(T);
        }

        /// <summary>
        /// 获取变量的所有属性映射。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<PropertyMap> GetPropertyMaps<T>()
        {
            var type = GetWrapType<T>();
            return _propertyCache.TryGetValue(typeof(T), () =>
                {
                    return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Where(s => s.Name != "_ID" && !s.IsDefined<DisGenerateAttribute>())
                        .Select(s => new PropertyMap(s)).ToList();
                });
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
        /// 在已编译的类列表中查找适合扩展的类。
        /// </summary>
        /// <param name="schemaType"></param>
        /// <returns></returns>
        private List<Type> FindExtensionTypes(Type schemaType)
        {
            return _extensionTypes.TryGetValue(schemaType, () =>
            {
                var result = new List<Type>();

                    foreach (var type in _extendTypes)
                    {
                        if (type.GetCustomAttributes<SchemaExtensionAttribute>(false).Any(s => s.SchemaType == schemaType))
                        {
                            result.Add(type);
                        }

                        if (type.GetCustomAttributes<SchemaInitializerAttribute>(false).Any(s => s.SchemaType == schemaType))
                        {
                            var initializer = Activator.CreateInstance(type) as ISchemaInitializer;
                            if (initializer != null)
                            {
                                InitializerUnity.Register(schemaType, initializer);
                            }
                        }
                        if (type.GetCustomAttributes<SchemaValidatorAttribute>(false).Any(s => s.SchemaType == schemaType))
                        {
                            var validator = Activator.CreateInstance(type) as ISchemaValidator;
                            if (validator != null)
                            {
                                Validations.ValidationUnity.Register(schemaType, validator);
                            }
                        }
                    }

                    return result;
                });
        }

        /// <summary>
        /// 动态编译扩展动态类。
        /// </summary>
        /// <param name="definition">模板定义。</param>
        /// <returns></returns>
        private CompileResult ComplileExtensionTypes(TemplateDefinition definition)
        {
            var pluginTypes = new List<Type>();
            var files = CompileHelper.GetExtensionFiles(definition, "schema", s => s.Schema);

            if (files.Length == 0)
            {
                return new CompileResult();
            }

            return CompileHelper.CompileTypes(_serviceProvider, definition, files, AssemblyReferenceManager.SchemaAssemblies);
        }

        /// <summary>
        /// 使用扩展属性对架构类进行包装。
        /// </summary>
        /// <param name="schemaType">架构类。</param>
        /// <param name="properties">扩展的属性列表。</param>
        /// <returns></returns>
        private Type BuildWrapType(Type schemaType, PropertyInfo[] properties)
        {
            /*
            var dyAssemblyBuilder = new DynamicAssemblyBuilder("SchemaExtension_" + schemaType.Name);
            var dyTypeBuilder = dyAssemblyBuilder.DefineType(schemaType.Name, baseType: schemaType);

            foreach (var property in properties)
            {
                var dyPropertyBuilder = dyTypeBuilder.DefineProperty(property.Name, property.PropertyType);
                dyPropertyBuilder.SetCustomAttribute<ExtendPropertyAttribute>();
                dyPropertyBuilder.DefineGetSetMethods();
                SetPropertyCustomAttributes(property, dyPropertyBuilder);
            }

            foreach (var con in schemaType.GetConstructors())
            {
                var parameters = con.GetParameters();
                var dyConsBuilder = dyTypeBuilder.DefineConstructor(parameters.Select(s => s.ParameterType).ToArray(), ilCoding: (b) =>
                    {
                        b.Emitter
                            .ldarg_0
                            .For(1, parameters.Length + 1, (e, i) => e.ldarg(i))
                            .call(con).ret();
                    });
            }

            return dyTypeBuilder.CreateType();
            */

            var source = $@"
{string.Join(Environment.NewLine, _namespaces.Select(s => "using " + s + ";"))}

[SerializableAttribute]
public class {schemaType.Name}_Wrap: {schemaType.FullName}
{{
{string.Join(Environment.NewLine, schemaType.GetConstructors().Select(s => "public " + schemaType.Name + "_Wrap(" + string.Join(", ", s.GetParameters().Select(t => t.ParameterType.Name + " " + t.Name)) + ") : base (" + string.Join(", ", s.GetParameters().Select(t => t.Name)) + ") {}"))}
{string.Join(Environment.NewLine, properties.Select(s => string.Join(Environment.NewLine, GetPropertyCustomAttributes(s)) + Environment.NewLine + "public " + s.PropertyType.Name + " " + s.Name + " { get; set; }"))}
}}
";

            return CompileHelper.CompileType(_serviceProvider, source, AssemblyReferenceManager.SchemaAssemblies.Union(AssemblyReferenceManager.CommonAssemblies).Union(LocalDynamicCache.CommonAssemblies).Union(_files).Distinct().ToArray());
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
            var ediAttr = property.GetCustomAttributes<EditorAttribute>().FirstOrDefault();
            var reqAttr = property.GetCustomAttributes<RequiredCheckAttribute>().FirstOrDefault();

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

            if (ediAttr != null)
            {
                var editorType = Type.GetType(ediAttr.EditorTypeName);
                if (editorType == null)
                {
                    editorType = _extendTypes.FirstOrDefault(s => s.AssemblyQualifiedName == ediAttr.EditorTypeName);
                }

                dyPropertyBuilder.SetCustomAttribute<EditorAttribute>(editorType, Type.GetType(ediAttr.EditorBaseTypeName));
            }

            if (reqAttr != null)
            {
                dyPropertyBuilder.SetCustomAttribute<RequiredCheckAttribute>();
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
            else
            {
                attributes.Add($"[CategoryAttribute(\"{CategoryConsts.Extension}\")]");
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
