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
    /// 表示不被持久化。打上此特性后，属性不会被保存到本地文件中。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class UnPersistentlyAttribute : Attribute
    {
    }
}
