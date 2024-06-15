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
        /// <summary>
        /// 注册初始化器。
        /// </summary>
        /// <param name="initializer">初始化器。</param>
        public static void Register(IProfileInitializer initializer)
        {
            LocalDynamicCache.ProfileCache.Add(initializer);
        }

        /// <summary>
        /// 为指定的架构类型注册初始化器。
        /// </summary>
        /// <param name="schemaType">架构类型。</param>
        /// <param name="initializer">初始化器。</param>
        public static void Register(Type schemaType, ISchemaInitializer initializer)
        {
            if (!LocalDynamicCache.SchemaInitializerCache.TryGetValue(schemaType, out List<ISchemaInitializer> list))
            {
                list = new List<ISchemaInitializer>();
                LocalDynamicCache.SchemaInitializerCache.Add(schemaType, list);
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
            if (profile != null)
            {
                LocalDynamicCache.ProfileCache.ForEach(s => Util.AttachDevHosting(s, hosting).Initialize(profile, template));
            }

            return profile;
        }

        /// <summary>
        /// 初始化架构。
        /// </summary>
        /// <param name="hosting">开发环境。</param>
        /// <param name="schema">架构对象。</param>
        public static T Initialize<T>(IDevHosting hosting, T schema) where T : SchemaBase
        {
            if (schema != null)
            {
                foreach (var kvp in LocalDynamicCache.SchemaInitializerCache)
                {
                    if (kvp.Key.IsAssignableFrom(schema.GetType()))
                    {
                        kvp.Value.ForEach(s => Util.AttachDevHosting(s, hosting).Initialize(hosting.Profile, schema));
                    }
                }
            }

            return schema;
        }
    }
}
