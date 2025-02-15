// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;

namespace CodeBuilder.Core.DynamicFunc
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class DynamicFuncAttribute : Attribute
    {
        public DynamicFuncAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public string Description { get; set; }
    }
}
