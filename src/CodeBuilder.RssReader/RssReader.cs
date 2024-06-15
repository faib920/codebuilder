// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using CodeBuilder.Core.Forms;
using CodeBuilder.Core.Tool;
using Fireasy.Common.Extensions;
using System.ComponentModel.Composition;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace CodeBuilder.RssReader
{
    [Export(typeof(IToolProvider))]
    public class RssReader : IToolProvider, IBootstrapTool
    {
        private IDevHosting _hosting;

        public string Name => "RSS订阅器";

        public void Initialize(IDevHosting hosting)
        {
            _hosting = hosting;
        }

        public Form Execute()
        {
            var form = new frmRssReader(_hosting);
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

        public void Run()
        {
            var windowSetManager = _hosting.ServiceProvider.TryGetService<IWindowSetManager>();
            if (windowSetManager.GetFlag("RssReader") != 0)
            {
                new frmRssReader(_hosting).Show((DockPanel)_hosting.DockContainer, DockState.DockRight);
            }
        }
    }
}
