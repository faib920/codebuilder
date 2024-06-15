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

namespace CodeBuilder.Tools
{
    [Export(typeof(IToolProvider))]
    public class DataTypeManageTool : IToolProvider
    {
        private IDevHosting _hosting;

        public string Name => "数据类型编辑器";

        public void Initialize(IDevHosting hosting)
        {
            _hosting = hosting;
        }

        public Form Execute()
        {
            var form = new frmDataTypeManager(_hosting);
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
