// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System.Collections.Generic;
using System.Windows.Forms;

namespace CodeBuilder
{
    public interface IContextMenuManager
    {
        IEnumerable<ToolStripItem> GetContextMenuItems();
    }
}
