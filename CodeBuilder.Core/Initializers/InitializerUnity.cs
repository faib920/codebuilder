// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Source;
using CodeBuilder.Core.Template;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeBuilder.Core.Initializers
{
    /// <summary>
    /// 初始化管理单元。
    /// </summary>
    public class InitializerUnity
    {
        private static readonly Dictionary<Type, List<ISchemaInitializer>> _schemaCache = new Dictionary<Type, List<ISchemaInitializer>>();
        private static readonly List<IProfileInitializer> _profileCache = new List<IProfileInitializer>();

        /// <summary>
        /// 注册初始化器。
        /// </summary>
        /// <param name="initializer">初始化器。</param>
        public static void Register(IProfileInitializer initializer)
        {
            _profileCache.Add(initializer);
        }

        /// <summary>
        /// 为指定的架构类型注册初始化器。
        /// </summary>
        /// <param name="schemaType">架构类型。</param>
        /// <param name="initializer">初始化器。</param>
        public static void Register(Type schemaType, ISchemaInitializer initializer)
        {
            if (!_schemaCache.TryGetValue(schemaType, out List<ISchemaInitializer> list))
            {
                list = new List<ISchemaInitializer>();
                _schemaCache.Add(schemaType, list);
            }

            list.Add(initializer);
        }

        /// <summary>
        /// 初始化模板。
        /// </summary>
        /// <param name="hosting">开发环境。</param>
        /// <param name="profile">变量对象。</param>
        /// <param name="template">模板定义。</param>
        /// <returns></returns>
        public static Profile Initialize(IDevHosting hosting, Profile profile, TemplateDefinition template)
        {
            _profileCache.ForEach(s => AttachDevHosting(s, hosting).Initialize(profile, template));
            return profile;
        }

        /// <summary>
        /// 初始化架构。
        /// </summary>
        /// <param name="hosting">开发环境。</param>
        /// <param name="schema">架构对象。</param>
        public static void Initialize(IDevHosting hosting, SchemaBase schema)
        {
            foreach (var kvp in _schemaCache)
            {
                if (kvp.Key.IsAssignableFrom(schema.GetType()))
                {
                    kvp.Value.ForEach(s => AttachDevHosting(s, hosting).Initialize(hosting.Profile, schema));
                }
            }
        }

        private static ISchemaInitializer AttachDevHosting(ISchemaInitializer initializer, IDevHosting hosting)
        {
            var property = initializer.GetType().GetProperties().FirstOrDefault(s => s.PropertyType == typeof(IDevHosting));
            if (property != null && property.CanWrite)
            {
                property.SetValue(initializer, hosting);
            }

            return initializer;
        }

        private static IProfileInitializer AttachDevHosting(IProfileInitializer initializer, IDevHosting hosting)
        {
            var property = initializer.GetType().GetProperties().FirstOrDefault(s => s.PropertyType == typeof(IDevHosting));
            if (property != null && property.CanWrite)
            {
                property.SetValue(initializer, hosting);
            }

            return initializer;
        }
    }
}
