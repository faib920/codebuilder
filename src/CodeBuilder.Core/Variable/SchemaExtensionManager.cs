// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Source;
using CodeBuilder.Core.Template;
using Fireasy.Common.DependencyInjection;
using Fireasy.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CodeBuilder.Core.Variable
{
    /// <summary>
    /// 架构扩展管理类。
    /// </summary>
    public class SchemaExtensionManager : BaseExtensionManager, ISchemaExtensionManager, ISingletonService
    {
        private static HashSet<Type> _extendTypes = new HashSet<Type>();
        private static HashSet<string> _namespaces = new HashSet<string>();
        private static HashSet<string> _files = new HashSet<string>();
        private static readonly Dictionary<Type, List<Type>> _extensionTypes = new Dictionary<Type, List<Type>>();
        private static readonly Dictionary<Type, List<PropertyMap>> _propertyCache = new Dictionary<Type, List<PropertyMap>>();
        private readonly IServiceProvider _serviceProvider;
        private readonly ICompileManager _compileManager;
        private TemplateDefinition _definition;

        public SchemaExtensionManager(IServiceProvider serviceProvider, ICompileManager compileManager)
        {
            _serviceProvider = serviceProvider;
            _compileManager = compileManager;
        }

        /// <summary>
        /// 初始化模板。
        /// </summary>
        /// <param name="definition">模板定义。</param>
        public void Initialize(TemplateDefinition definition)
        {
            if (definition != null)
            {
                _definition = definition;

                _extendTypes.Clear();
                _extensionTypes.Clear();
                _propertyCache.Clear();

                _extendTypes = _compileManager.Schema.Types;
                _namespaces = _compileManager.Schema.Namespaces;
                _files = new HashSet<string>(_compileManager.Common.Files.Union(_compileManager.Schema.Files));
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
            if (_compileManager.SchemaWrap?.TryGetValue(schemaType.Name, out var result) == true && result?.Types?.Count > 0)
            {
                return result.Types.First();
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
    }
}
