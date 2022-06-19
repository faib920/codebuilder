// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;

namespace CodeBuilder.Core.Variable
{
    public class CompileException : Exception
    {
        public CompileException(string message)
            : base(message)
        {
        }

        public string FileName { get; set; }
    }
}
