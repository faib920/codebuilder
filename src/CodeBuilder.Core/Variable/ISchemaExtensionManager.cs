// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Template;
using System;
using System.Collections.Generic;

namespace CodeBuilder.Core.Variable
{
    public interface ISchemaExtensionManager
    {
        /// <summary>
        /// 初始化模板。
        /// </summary>
        /// <param name="definition">模板定义。</param>
        void Initialize(TemplateDefinition definition);

        /// <summary>
        /// 构造一个代理对象。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arguments">构造器参数。</param>
        /// <returns></returns>
        T Build<T>(params object[] arguments);

        /// <summary>
        /// 获取包装过的类型。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Type GetWrapType<T>();

        /// <summary>
        /// 获取变量的所有属性映射。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<PropertyMap> GetPropertyMaps<T>();
    }
}
