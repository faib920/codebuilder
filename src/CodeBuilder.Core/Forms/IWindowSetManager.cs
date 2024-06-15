// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace CodeBuilder.Core.Forms
{
    /// <summary>
    /// 提供对窗体集的管理。
    /// </summary>
    public interface IWindowSetManager
    {
        /// <summary>
        /// 获取窗体的标志。
        /// </summary>
        /// <param name="name">窗体名称。</param>
        /// <returns></returns>
        int? GetFlag(string name);

        /// <summary>
        /// 设置窗体的标记。
        /// </summary>
        /// <param name="name">窗体名称。</param>
        /// <param name="flag"></param>
        void SetFlag(string name, int flag);
    }
}
