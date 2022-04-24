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

namespace CodeBuilder.JsonTool
{
    [Export(typeof(IToolProvider))]
    public class JsonFormatter : IToolProvider
    {
        private IDevHosting _hosting;

        public string Name
        {
            get { return "Json格式化"; }
        }

        public void Initialize(IDevHosting hosting)
        {
            _hosting = hosting;
        }

        public void Execute()
        {
            new frmJsonFormatter(_hosting).Show((DockPanel)_hosting.DockContainer, DockState.Document);
        }
    }
}
