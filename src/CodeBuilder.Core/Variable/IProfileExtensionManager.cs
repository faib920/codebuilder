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
    public interface IProfileExtensionManager
    {
        /// <summary>
        /// 编译生成一个变量。
        /// </summary>
        /// <param name="definition">模板定义。</param>
        Profile Build(TemplateDefinition definition);

        /// <summary>
        /// 获取变量的包装类。
        /// </summary>
        /// <param name="definition">模板定义。</param>
        /// <returns></returns>
        Type GetWrapType();

        /// <summary>
        /// 获取变量的所有属性映射。
        /// </summary>
        /// <param name="definition">模板定义。</param>
        /// <returns></returns>
        List<PropertyMap> GetPropertyMaps();
    }
}
