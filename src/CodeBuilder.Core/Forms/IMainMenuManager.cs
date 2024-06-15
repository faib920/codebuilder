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
    /// 主菜单管理接口。
    /// </summary>
    public interface IMainMenuManager
    {
        /// <summary>
        /// 获取菜单条上的名称。
        /// </summary>
        string MenuText { get; }

        /// <summary>
        /// 获取插入点的Key。
        /// </summary>
        string InsertedMainMenuKey { get; }

        /// <summary>
        /// 获取显示方式。
        /// </summary>
        DisplayStyle DisplayStyle { get; }

        /// <summary>
        /// 获取菜单项。
        /// </summary>
        /// <returns></returns>
        IEnumerable<ToolStripItem> GetMenuItems();
    }

    public enum DisplayStyle
    {
        Actived,
        Added
    }
}
