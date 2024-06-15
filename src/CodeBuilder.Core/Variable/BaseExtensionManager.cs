// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using Fireasy.Common.Extensions;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace CodeBuilder.Core.Variable
{
    public class BaseExtensionManager
    {
        /// <summary>
        /// 初始化对象的默认值。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected static T InitializeDefaultValue<T>(T obj, Type _wrapType)
        {
            if (obj == null)
            {
                return default;
            }

            var map = _wrapType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(s => s.CanWrite)
                .Select(s => new { Property = s, DefaultValue = s.GetCustomAttributes<DefaultValueAttribute>().FirstOrDefault() })
                .Where(s => s.DefaultValue != null)
                .ToArray();

            foreach (var item in map)
            {
                item.Property.SetValue(obj, item.DefaultValue.Value.To(item.Property.PropertyType));
            }

            return obj;
        }
    }
}
