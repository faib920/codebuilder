// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.ComponentModel;
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

            var desc = property.GetCustomAttribute<DescriptionAttribute>();
            if (desc != null)
            {
                Description = desc.Description;
            }
        }

        /// <summary>
        /// 获取属性名称。
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 获取属性类型。
        /// </summary>
        public Type Type { get; private set; }

        /// <summary>
        /// 获取描述名称。
        /// </summary>
        public string TypeName { get; private set; }

        /// <summary>
        /// 获取描述。
        /// </summary>
        public string Description { get; private set; }
    }
}
