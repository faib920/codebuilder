// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;

namespace CodeBuilder.Core.Source
{
    public class OpenSchemaFileException : Exception
    {
        public OpenSchemaFileException(string version, string message, Exception exp)
            : base (message, exp)
        {
            Version = version;
        }

        public OpenSchemaFileException(string message, Exception exp)
            : base(message, exp)
        {
        }

        public string Version { get; set; }
    }
}
