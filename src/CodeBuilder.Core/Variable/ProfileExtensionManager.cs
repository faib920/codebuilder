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
using Fireasy.Common.Extensions;
using System;
using System.Collections.Generic;
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
        private static HashSet<Type> _extendTypes = new HashSet<Type>();
        private static HashSet<string> _namespaces = new HashSet<string>();
        private static HashSet<string> _files = new HashSet<string>();
        private static List<PropertyMap> _propertyCache = null;
        private readonly IServiceProvider _serviceProvider;
        private readonly ICompileManager _compileManager;
        private TemplateDefinition _definition;

        public ProfileExtensionManager(IServiceProvider serviceProvider, ICompileManager compileManager)
        {
            _serviceProvider = serviceProvider;
            _compileManager = compileManager;
        }

        /// <summary>
        /// 编译生成一个变量。
        /// </summary>
        /// <param name="definition">模板定义。</param>
        public Profile Build(TemplateDefinition definition)
        {
            if (definition != null)
            {
                _definition = definition;
                _propertyCache = null;
                _wrapType = null;
                _extendTypes = _compileManager.Profile.Types;
                _namespaces = _compileManager.Profile.Namespaces;
                _files = new HashSet<string>(_compileManager.Common.Files.Union(_compileManager.Profile.Files));
                _wrapType = GetWrapType();
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
        public Type GetWrapType()
        {
            return _compileManager.ProfileWrap?.Types?.FirstOrDefault();
        }

        /// <summary>
        /// 获取变量的所有属性映射。
        /// </summary>
        /// <param name="definition"></param>
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
    }
}
