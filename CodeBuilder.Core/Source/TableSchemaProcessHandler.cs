// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace CodeBuilder.Core
{
    /// <summary>
    /// 数据表处理的通知委托。
    /// </summary>
    /// <param name="current">当前处理的表对象。</param>
    /// <param name="percentage">当前的进度百分比。</param>
    public delegate void TableSchemaProcessHandler(string current, int percentage);
}
