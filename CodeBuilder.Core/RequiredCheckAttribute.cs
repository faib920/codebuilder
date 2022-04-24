// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;

namespace CodeBuilder.Core
{
    /// <summary>
    /// 打上此特性后会进行必填项验证。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredCheckAttribute : Attribute
    {
    }
}
