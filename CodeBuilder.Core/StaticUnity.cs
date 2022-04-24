// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Text;

namespace CodeBuilder.Core
{
    /// <summary>
    /// 静态辅助类。
    /// </summary>
    public class StaticUnity
    {
        public static List<string> DynamicAssemblies { get; set; } = new List<string>();

        public static Encoding Encoding { get; set; } = Encoding.Default;
    }
}
