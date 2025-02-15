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
    public interface IChangeManager
    {
        /// <summary>
        /// 获取是否更改。
        /// </summary>
        bool IsChanged { get; }

        /// <summary>
        /// 保存。
        /// </summary>
        bool SaveChanges(bool notify = true);
    }
}
