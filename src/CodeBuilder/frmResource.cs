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
using CodeBuilder.Core.Source;
using Fireasy.Composition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CodeBuilder
{
    public partial class frmResource : DockFormBase
    {
        internal const string WndName = "Resource";

        private readonly DevHosting _hosting;

        public Action<ISourceProvider, List<Table>, SourceOption> LoadHistoryAct { get; set; }

        public frmResource(DevHosting hosting)
        {
            InitializeComponent();
            Config.Instance.AddWindow(WndName).Save();

            Icon = Properties.Resources.resource;
            _hosting = hosting;
        }

        private void frmResource_Load(object sender, System.EventArgs e)
        {
            LoadResources();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            Config.Instance.RemoveWindow(WndName).Save();
            base.OnFormClosed(e);
        }

        public void LoadResources()
        {
            lstRes.BeginUpdate();
            lstRes.Items.Clear();

            var sources = _hosting.ServiceProvider.GetExportedServices<ISourceProvider>();
            foreach (var p in sources)
            {
                var item = lstRes.Items.Add(p.Name);
                item.Tag = p;
                item.Image = Properties.Resources.category;

                foreach (var h in p.GetHistory())
                {
                    var subitem = item.Items.Add(h.ToString());
                    subitem.Tag = h;
                    subitem.Image = Properties.Resources.file;
                }

                item.Expended = true;
            }

            lstRes.EndUpdate();
        }

        private void tlbRefresh_Click(object sender, System.EventArgs e)
        {
            LoadResources();
        }

        private void tlbDelete_Click(object sender, EventArgs e)
        {
            if (lstRes.SelectedItems.Count == 0)
            {
                if (_hosting.ShowConfirm("是否清空所有资源?") == ShowMsgButton.No)
                {
                    return;
                }

                var sources = _hosting.ServiceProvider.GetExportedServices<ISourceProvider>();
                foreach (var p in sources)
                {
                    p.ClearHistory();
                }

                LoadResources();
            }
            else if (lstRes.SelectedItems[0].Level == 0)
            {
                if (_hosting.ShowConfirm($"是否删除 {lstRes.SelectedItems[0].Text} 下的所有资源?") == ShowMsgButton.No)
                {
                    return;
                }

                var p = lstRes.SelectedItems[0].Tag as ISourceProvider;
                p.ClearHistory();
                LoadResources();
            }
            else if (lstRes.SelectedItems[0].Level == 1)
            {
                if (_hosting.ShowConfirm($"是否删除 {lstRes.SelectedItems[0].Text} ?") == ShowMsgButton.No)
                {
                    return;
                }

                var p = lstRes.SelectedItems[0].Parent.Tag as ISourceProvider;
                p.DeleteHistory(lstRes.SelectedItems[0].Tag);
                LoadResources();
            }
        }

        private async void lstRes_ItemDoubleClick(object sender, Fireasy.Windows.Forms.TreeListItemEventArgs e)
        {
            if (e.Item.Level == 1)
            {
                var provider = e.Item.Parent.Tag as ISourceProvider;
                _hosting.SourceProvider = provider;
                var option = new SourceOption { View = Config.Instance.Source_View };
                option.Selected = _hosting.GetTables().Select(s => s.Name).ToList();

                var tables = await provider.FromHistoryAsync(e.Item.Tag, option);

                if (tables != null && LoadHistoryAct != null)
                {
                    LoadHistoryAct(provider, tables, option);
                }
            }
        }
    }
}
