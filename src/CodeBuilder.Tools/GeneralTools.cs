// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using System.ComponentModel.Composition;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace CodeBuilder.Tools
{
    [Export(typeof(IToolProvider))]
    public class GeneralTools : IToolProvider
    {
        private IDevHosting _hosting;

        public string Name => "常用工具合集";

        public void Initialize(IDevHosting hosting)
        {
            _hosting = hosting;
        }

        public Form Execute(params object[] arguments)
        {
            var form = new frmTools(_hosting);
            if (_hosting.DockContainer != null)
            {
                form.Show((DockPanel)_hosting.DockContainer, DockState.Document);
            }
            else
            {
                form.Show();
            }

            return form;
        }
    }
}
