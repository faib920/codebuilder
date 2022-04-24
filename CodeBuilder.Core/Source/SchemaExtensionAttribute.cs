// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;

namespace CodeBuilder.Core.Source
{
    /// <summary>
    /// 提供对架构的扩展类。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SchemaExtensionAttribute : Attribute
    {
        public SchemaExtensionAttribute(Type schemaType)
        {
            SchemaType = schemaType;
        }

        /// <summary>
        /// 获取或设置提供扩展的架构类型。
        /// </summary>
        public Type SchemaType { get; set; }
    }
}
