// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System.Collections.Generic;
using System.Windows.Forms;

namespace CodeBuilder.Core.Forms
{
    /// <summary>
    /// 上下文菜单管理接口。
    /// </summary>
    public interface IContextMenuManager
    {
        /// <summary>
        /// 获取上下文菜单荐。
        /// </summary>
        /// <returns></returns>
        IEnumerable<ToolStripItem> GetContextMenuItems();
    }
}
