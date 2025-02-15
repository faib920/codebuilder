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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CodeBuilder.Core.DynamicFunc
{
    public class DynamicFuncHelper
    {
        /// <summary>
        /// 获取方法定义。
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static IEnumerable<DynamicFuncAttribute> GetMethodDescriptors(IDynamicFuncProvider provider)
        {
            return GetMethodDescriptors(provider.GetType());
        }

        /// <summary>
        /// 获取方法定义。
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static IEnumerable<DynamicFuncAttribute> GetMethodDescriptors(Type providerType)
        {
            var methods = providerType.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(s => s.IsDefined<DynamicFuncAttribute>());

            foreach (var method in methods)
            {
                yield return method.GetCustomAttribute<DynamicFuncAttribute>();
            }
        }
    }
}
