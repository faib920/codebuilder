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
    /// 插件定义。
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// 获取插件的名称。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="hosting">开发环境。</param>
        void Initialize(IDevHosting hosting);
    }
}
