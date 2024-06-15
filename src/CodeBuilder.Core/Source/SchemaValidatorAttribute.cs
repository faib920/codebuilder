// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;

namespace CodeBuilder.Core.Source
{
    /// <summary>
    /// 提供架构验证的类。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SchemaValidatorAttribute : Attribute
    {
        public SchemaValidatorAttribute(Type schemaType)
        {
            SchemaType = schemaType;
        }

        /// <summary>
        /// 获取或设置提供验证的架构类型。
        /// </summary>
        public Type SchemaType { get; set; }
    }
}
