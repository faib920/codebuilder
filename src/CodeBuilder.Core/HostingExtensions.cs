// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System.Linq;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace CodeBuilder.Core
{
    public static class HostingExtensions
    {
        public static TForm FindForm<TForm>(this IDevHosting hosting) where TForm : Form
        {
            if (hosting == null || hosting.DockContainer == null)
            {
                return default;
            }

            var dockMgr = (DockPanel)hosting.DockContainer;
            var form = dockMgr.Contents.OfType<TForm>().FirstOrDefault();
            return form as TForm;
        }
    }
}
