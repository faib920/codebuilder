// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Template;

namespace CodeBuilder.Core.Initializers
{
    /// <summary>
    /// 提供对变量进行初始化的接口。
    /// </summary>
    public interface IProfileInitializer
    {
        void Initialize(dynamic profile, TemplateDefinition template);
    }
}
