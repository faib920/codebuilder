// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace CodeBuilder.Core.Template
{
    /// <summary>
    /// 代码生成的通知委托。
    /// </summary>
    /// <param name="current">当前生成的内容。</param>
    /// <param name="percentage">当前的进度百分比。</param>
    public delegate void CodeGenerateHandler(string current, int percentage);
}
