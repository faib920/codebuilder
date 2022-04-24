// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using System.ComponentModel.Composition;
using WeifenLuo.WinFormsUI.Docking;

namespace CodeBuilder.Tools
{
    [Export(typeof(IToolProvider))]
    public class GeneralTools : IToolProvider
    {
        private IDevHosting _hosting;

        public string Name
        {
            get { return "常用工具合集"; }
        }

        public void Initialize(IDevHosting hosting)
        {
            _hosting = hosting;
        }

        public void Execute()
        {
            new frmTools(_hosting).Show((DockPanel)_hosting.DockContainer, DockState.Document);
        }
    }
}
