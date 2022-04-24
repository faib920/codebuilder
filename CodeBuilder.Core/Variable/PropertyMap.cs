// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Reflection;

namespace CodeBuilder.Core.Variable
{
    /// <summary>
    /// 属性映射。
    /// </summary>
    public class PropertyMap
    {
        public PropertyMap(PropertyInfo property)
        {
            Name = property.Name;
            Type = property.PropertyType;
            TypeName = Util.GetTypeName(Type);
        }

        /// <summary>
        /// 获取或设置属性名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置属性类型。
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// 获取或设置属性描述名称。
        /// </summary>
        public string TypeName { get; set; }
    }
}
