// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Template;
using System.Collections.Generic;

namespace CodeBuilder.Core.Variable
{
    public interface ICompileManager
    {
        CompileResult Common { get; }

        CompileResult Profile { get; }

        CompileResult Schema { get; }

        CompileResult ProfileWrap { get; }

        Dictionary<string, CompileResult> SchemaWrap { get; }

        void Compile(TemplateDefinition definition, bool forceBuild = true);

        void ClearExpiredFiles();
    }
}
