// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace CodeBuilder
{
    public interface IChangeManager
    {
        /// <summary>
        /// 获取是否更改。
        /// </summary>
        bool IsChanged { get; }

        /// <summary>
        /// 保存文件。
        /// </summary>
        void SaveFile();
    }
}
