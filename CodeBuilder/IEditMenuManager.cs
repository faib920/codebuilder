// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System.Windows.Forms;

namespace CodeBuilder
{
    public interface IEditMenuManager
    {
        ToolStripItemCollection GetEditMenuItems();

        string Key { get; }

        void Invoke(string command, object arguments);
    }
}
