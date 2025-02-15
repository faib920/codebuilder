// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace CodeBuilder.Core.Variable
{
    public class CompileResult
    {
        public HashSet<string> Files { get; set; } = new HashSet<string>();

        public HashSet<string> Namespaces { get; set; } = new HashSet<string>();

        public HashSet<Type> Types { get; set; } = new HashSet<Type>();
    }

    public class CompilePersisResult
    {
        public HashSet<string> Files { get; set; }

        public HashSet<string> Namespaces { get; set; }

        public HashSet<string> Types { get; set; }
    }

    public class CompileResultFile
    {
        public CompilePersisResult Common { get; set; }

        public CompilePersisResult Profile { get; set; }

        public CompilePersisResult Schema { get; set; }

        public CompilePersisResult ProfileWrap { get; set; }

        public Dictionary<string, CompilePersisResult> SchemaWrap { get; set; }

        public IEnumerable<string> GetAllFiles()
        {
            if (Common?.Files.Count > 0)
            {
                foreach (var f in Common?.Files)
                {
                    yield return f;
                }
            }
            if (Profile?.Files.Count > 0)
            {
                foreach (var f in Profile?.Files)
                {
                    yield return f;
                }
            }
            if (Schema?.Files.Count > 0)
            {
                foreach (var f in Schema?.Files)
                {
                    yield return f;
                }
            }
            if (ProfileWrap?.Files.Count > 0)
            {
                foreach (var f in ProfileWrap?.Files)
                {
                    yield return f;
                }
            }
            if (SchemaWrap != null)
            {
                foreach (var k in SchemaWrap)
                {
                    if (k.Value?.Files?.Count > 0)
                    {
                        foreach (var f in k.Value.Files)
                        {
                            yield return f;
                        }
                    }
                }
            }
        }
    }
}
