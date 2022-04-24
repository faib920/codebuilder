// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Initializers;
using CodeBuilder.Core.Source;
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
    /// 架构扩展管理类。
    /// </summary>
    public class SchemaExtensionManager
    {
        private static List<Type> _extendTypes = new List<Type>();
        private static readonly Dictionary<Type, List<Type>> _extensionTypes = new Dictionary<Type, List<Type>>();
        private static readonly Dictionary<Type, Type> _cache = new Dictionary<Type, Type>();
        private static readonly Dictionary<Type, List<PropertyMap>> _propertyCache = new Dictionary<Type, List<PropertyMap>>();
        private static string _templateUId = string.Empty;

        /// <summary>
        /// 初始化模板。
        /// </summary>
        /// <param name="templateUId">模板UID。</param>
        public static void Initialize(string templateUId)
        {
            if (_templateUId != templateUId)
            {
                _extendTypes.Clear();
                _extensionTypes.Clear();
                _cache.Clear();
                _propertyCache.Clear();

                _extendTypes = ComplileExtensionTypes(templateUId);
                _cache.Add(typeof(Table), GetWrapType<Table>());
                _cache.Add(typeof(Column), GetWrapType<Column>());
                _cache.Add(typeof(Reference), GetWrapType<Reference>());

                _templateUId = templateUId;
            }
        }

        /// <summary>
        /// 构造一个代理对象。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arguments">构造器参数。</param>
        /// <returns></returns>
        public static T Build<T>(params object[] arguments)
        {
            try
            {
                var wrapType = GetWrapType<T>();
                return InitializeDefaultValue(wrapType.New<T>(arguments));
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
        public static Type GetWrapType<T>()
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
        public static List<PropertyMap> GetPropertyMaps<T>()
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
        /// 在已编译的类列表中查找适合扩展的类。
        /// </summary>
        /// <param name="schemaType"></param>
        /// <returns></returns>
        private static List<Type> FindExtensionTypes(Type schemaType)
        {
            return _extensionTypes.TryGetValue(schemaType, () =>
                {
                    var result = new List<Type>();

                    foreach (var type in _extendTypes)
                    {
                        foreach (var attr in type.GetCustomAttributes<SchemaExtensionAttribute>(false).Where(s => s.SchemaType == schemaType))
                        {
                            result.Add(type);
                        }

                        foreach (var attr in type.GetCustomAttributes<SchemaInitializerAttribute>(false).Where(s => s.SchemaType == schemaType))
                        {
                            var initializer = type.New<ISchemaInitializer>();
                            if (initializer != null)
                            {
                                InitializerUnity.Register(schemaType, initializer);
                            }
                        }
                    }

                    return result;
                });
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

                var compiler = new Fireasy.Common.Compiler.CodeCompiler();
                var fileName = Util.GenerateTempFileName();
                StaticUnity.DynamicAssemblies.Add(fileName);

                compiler.OutputAssembly = fileName;
                compiler.CodeProvider = GetCodeProvider(ext);
                foreach (var ass in AssemblyReferenceManager.SchemaAssemblies)
                {
                    compiler.Assemblies.Add(ass);
                }
                compiler.Assemblies.AddRange(CommonExtensionManager.GetCommonDynamicAssemblies());
                pluginTypes.AddRange(compiler.CompileAssembly(files).GetExportedTypes());
            }

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

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "extensions\\schema");
            if (Directory.Exists(path))
            {
                if (string.IsNullOrEmpty(templateUId))
                {
                    return Directory.GetFiles(path, "*" + ext).Where(s => !Path.GetFileName(s).StartsWith("template.", StringComparison.OrdinalIgnoreCase)).ToArray();
                }
                else
                {
                    return Directory.GetFiles(path, "*" + ext).Where(s => !Path.GetFileName(s).StartsWith("template.", StringComparison.OrdinalIgnoreCase) || Path.GetFileName(s) .Equals("template." + templateUId + ext, StringComparison.OrdinalIgnoreCase)).ToArray();
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
        private static T InitializeDefaultValue<T>(T obj)
        {
            var map = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
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
        private static Type BuildWrapType(Type schemaType, PropertyInfo[] properties)
        {
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
        }
    }
}
