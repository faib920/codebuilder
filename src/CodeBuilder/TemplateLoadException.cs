// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;

namespace CodeBuilder
{
    internal class TemplateLoadException : Exception
    {
        public TemplateLoadException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
