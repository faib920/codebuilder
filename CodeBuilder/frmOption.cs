// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using CodeBuilder.Core.Forms;
using Fireasy.Common.Composition;
using Fireasy.Common.Extensions;
using Fireasy.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace CodeBuilder
{
    public partial class frmOption : FormBase
    {
        private bool _isNeedRestart;
        private List<string> _pluginRemoved = new List<string>();
        private readonly IDevHosting _hosting;

        public frmOption(IDevHosting hosting)
        {
            InitializeComponent();
            _hosting = hosting;
        }

        private void frmOption_Load(object sender, EventArgs e)
        {
            LoadEncodings();
            LoadPlugins();
        }

        private void LoadEncodings()
        {
            cboEncoding.DisplayMember = "DisplayName";
            cboEncoding.ValueMember = "Name";
            cboEncoding.Items.Add(new { DisplayName = "默认", Name = string.Empty });

            foreach (var en in Encoding.GetEncodings())
            {
                var index = cboEncoding.Items.Add(en);
                if (Config.Instance.Encoding == en.Name)
                {
                    cboEncoding.SelectedIndex = index;
                }
            }

            if (cboEncoding.SelectedIndex == -1)
            {
                cboEncoding.SelectedIndex = 0;
            }

            chkCheckUpdate.Checked = Config.Instance.CheckUpdate;
            chkView.Checked = Config.Instance.Source_View;
            chkOverFile.Checked = Config.Instance.SkipWhenFileExists;
        }

        private void LoadPlugins()
        {
            var sources = Imports.GetServices<ISourceProvider>();
            var templates = Imports.GetServices<ITemplateProvider>();
            var tools = Imports.GetServices<IToolProvider>();

            var sourceGroup = new TreeListGroup("数据源");
            var templateGroup = new TreeListGroup("模板");
            var toolGroup = new TreeListGroup("工具");
            lstPlugin.Groups.Add(sourceGroup);
            lstPlugin.Groups.Add(templateGroup);
            lstPlugin.Groups.Add(toolGroup);

            var cfg = PlugInConfig.Get();

            Func<IPlugin, TreeListItem> func = (s) =>
               {
                   var assembly = s.GetType().Assembly.GetName();

                   if (!cfg.Removed.Contains(s.Name))
                   {
                       var item = new TreeListItem(s.Name);
                       item.Image = Properties.Resources.plugin;
                       item.Cells.Add(assembly.Name);
                       item.Cells.Add(assembly.Version.ToString());
                       return item;
                   }

                   return null;
               };

            sources.ForEach(s => Add(sourceGroup.Items, s, func));
            templates.ForEach(s => Add(templateGroup.Items, s, func));
            tools.ForEach(s => Add(toolGroup.Items, s, func));
        }

        private void Add(TreeListItemCollection items, IPlugin plugin, Func<IPlugin, TreeListItem> func)
        {
            var item = func(plugin);
            if (item != null)
            {
                items.Add(item);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (cboEncoding.SelectedIndex != -1)
            {
                if (cboEncoding.SelectedIndex == 0)
                {
                    Config.Instance.Encoding = string.Empty;
                    StaticUnity.Encoding = Encoding.Default;
                }
                else
                {
                    Config.Instance.Encoding = ((EncodingInfo)cboEncoding.SelectedItem).Name;
                    StaticUnity.Encoding = Encoding.GetEncoding(Config.Instance.Encoding);
                }
            }

            Config.Instance.CheckUpdate = chkCheckUpdate.Checked;
            Config.Instance.Source_View = chkView.Checked;
            Config.Instance.SkipWhenFileExists = chkOverFile.Checked;
            Config.Save();

            if (_pluginRemoved.Count > 0)
            {
                _isNeedRestart = true;
                PlugInConfig.Remove(_pluginRemoved);
            }

            if (_isNeedRestart)
            {
                _hosting.ShowInfo("必须重启 CodeBuilder 才能生效。");
            }

            DialogResult = DialogResult.OK;
        }

        private void mnuShop_Click(object sender, EventArgs e)
        {
            var frm = new frmPluginShop(_hosting);
            frm.ShowDialog();
        }

        private void mnuRemove_Click(object sender, EventArgs e)
        {
            if (lstPlugin.SelectedItems.Count == 0)
            {
                return;
            }

            var other = CheckOther(lstPlugin.SelectedItems[0].Cells[1].Text);
            var message = other.Count > 0 ?
                "是否删除插件 " + lstPlugin.SelectedItems[0].Text + "(以及相关的 " + string.Join("、", other) + ")?" :
                "是否删除插件 " + lstPlugin.SelectedItems[0].Text + "?";

            if (_hosting.ShowConfirm(message) == ShowMsgButton.No)
            {
                return;
            }

            _pluginRemoved.Add(lstPlugin.SelectedItems[0].Cells[1].Text);
            Remove(lstPlugin.SelectedItems[0].Cells[1].Text);
        }

        private List<string> CheckOther(string name)
        {
            var list = new List<string>();
            foreach (var group in lstPlugin.Groups)
            {
                foreach (var item in group.Items)
                {
                    if (item != lstPlugin.SelectedItems[0] && item.Cells[1].Text == name)
                    {
                        list.Add(item.Text);
                    }
                }
            }

            return list;
        }

        private void Remove(string name)
        {
            foreach (var group in lstPlugin.Groups)
            {
                for (var i = group.Items.Count - 1; i >= 0; i--)
                {
                    var item = group.Items[i];
                    if (item.Cells[1].Text == name)
                    {
                        group.Items.Remove(item);
                    }
                }
            }
        }
    }
}
