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
    public class DynamicFuncInvokeException : Exception
    {
        public DynamicFuncInvokeException(string message, Exception exception)
            : base (message, exception)
        { }
    }
}
