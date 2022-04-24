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
    public class Remoter : IToolProvider
    {
        private IDevHosting _hosting;

        public string Name
        {
            get { return "远程连接器"; }
        }

        public void Initialize(IDevHosting hosting)
        {
            _hosting = hosting;
        }

        public void Execute()
        {
            new frmRemoter(_hosting).Show((DockPanel)_hosting.DockContainer, DockState.Document);
        }

        public class Connection
        {
            public string name { get; set; }

            public string host { get; set; }

            public string group { get; set; }
        }
    }
}
