// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
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
            var ui = property.GetCustomAttribute<UICustomizedAttribute>();
            if (ui != null)
            {
                IsUICustomized = true;
                DisplayName = ui.Name;
                Width = ui.Width;
            }
            var display = property.GetCustomAttribute<DisplayNameAttribute>();
            if (display != null)
            {
                DisplayName = display.DisplayName;
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
        /// 获取显示名称。
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// 获取描述。
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// 获取是否可定制显示列。
        /// </summary>
        public bool IsUICustomized { get; private set; }

        /// <summary>
        /// 获取显示的宽度。
        /// </summary>
        public int Width { get; private set; }
    }
}
