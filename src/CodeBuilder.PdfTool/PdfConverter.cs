// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using System.ComponentModel.Composition;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace CodeBuilder.PdfTool
{
    [Export(typeof(IToolProvider))]
    public class PdfConverter : IToolProvider
    {
        private IDevHosting _hosting;

        public string Name => "PDF转换器";

        public Form Execute()
        {
            var form = new frmToImg(_hosting);
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

        public void Initialize(IDevHosting hosting)
        {
            _hosting = hosting;
        }
    }
}
