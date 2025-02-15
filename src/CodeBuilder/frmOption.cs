// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using CodeBuilder.Core.DynamicFunc;
using CodeBuilder.Core.Forms;
using CodeBuilder.Core.Variable;
using Fireasy.Common.Extensions;
using Fireasy.Composition;
using Fireasy.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static CodeBuilder.Config;

namespace CodeBuilder
{
    public partial class frmOption : FormBase
    {
        private bool _isNeedRestart;
        private List<string> _pluginRemoved = new List<string>();
        private List<string> _changeFlags = new List<string>();
        private readonly IDevHosting _hosting;
        private Popup _popup;
        private bool _isLoading;
        private bool _isChanged;

        public frmOption(IDevHosting hosting)
        {
            InitializeComponent();
            _hosting = hosting;

            _popup = new Popup(textBox1) { DropShadowEnabled = false };
        }

        public Action OnPluginUpdated { get; set; }

        public bool IsChanged(string name)
        {
            return _changeFlags.Contains(name);
        }

        private void frmOption_Load(object sender, EventArgs e)
        {
            _isLoading = true;
            LoadEncodings();
            LoadPlugins();
            LoadPluginConfigureControls();
            _isLoading = false;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            foreach (TabPage pager in tabControl1.TabPages)
            {
                if (pager.Controls[0] is IConfigurableControl ctrl)
                {
                    if (!_isChanged && ctrl.IsChanged)
                    {
                        _isChanged = true;
                    }
                }
            }

            if (!_isChanged)
            {
                return;
            }

            var dialog = _hosting.ShowConfirm("选项已改变，关闭前是否保存?", 3);
            if (dialog == ShowMsgButton.Cancel)
            {
                e.Cancel = true;
            }
            else if (dialog == ShowMsgButton.Yes)
            {
                btnOk_Click(null, null);

                foreach (TabPage pager in tabControl1.TabPages)
                {
                    if (pager.Controls[0] is IConfigurableControl ctrl)
                    {
                        ctrl.Close();
                    }
                }
            }
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
            chkTempGroup.Checked = radioButton1.Enabled = radioButton2.Enabled = Config.Instance.TemplateGroup > 0;
            nudFontSize.Value = Config.Instance.FontSize;
            if (Config.Instance.TemplateGroup == TemplateGroupStyle.Language)
            {
                radioButton1.Checked = true;
            }
            if (Config.Instance.TemplateGroup == TemplateGroupStyle.Category)
            {
                radioButton2.Checked = true;
            }
            cboLogLevel.SelectedIndex = Config.Instance.LogLevel;
        }

        private void LoadPlugins()
        {
            var sources = _hosting.ServiceProvider.GetExportedServices<ISourceProvider>();
            var templates = _hosting.ServiceProvider.GetExportedServices<ITemplateProvider>();
            var tools = _hosting.ServiceProvider.GetExportedServices<IToolProvider>();
            var funcs = _hosting.ServiceProvider.GetExportedServices<IDynamicFuncProvider>();

            var sourceGroup = new TreeListGroup("数据源");
            var templateGroup = new TreeListGroup("模板");
            var toolGroup = new TreeListGroup("工具");
            var funcGroup = new TreeListGroup("函数");
            lstPlugin.Groups.Add(sourceGroup);
            lstPlugin.Groups.Add(templateGroup);
            lstPlugin.Groups.Add(toolGroup);
            lstPlugin.Groups.Add(funcGroup);

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
            funcs.ForEach(s => Add(funcGroup.Items, s, func));
        }

        private void LoadPluginConfigureControls()
        {
            var sources = _hosting.ServiceProvider.GetExportedServices<ISourceProvider>();
            var templates = _hosting.ServiceProvider.GetExportedServices<ITemplateProvider>();
            var tools = _hosting.ServiceProvider.GetExportedServices<IToolProvider>();

            foreach (var plugin in sources.OfType<IConfigureSupported>().Union(templates.OfType<IConfigureSupported>()).Union(tools.OfType<IConfigureSupported>()))
            {
                var control = plugin.GetOptionPanel();
                if (control != null)
                {
                    var pager = new TabPage(plugin.Name);
                    pager.Padding = new System.Windows.Forms.Padding(3);
                    pager.UseVisualStyleBackColor = true;
                    tabControl1.TabPages.Add(pager);
                    control.Dock = DockStyle.Fill;
                    pager.Controls.Add(control);
                }
            }
        }

        private void Add(TreeListItemCollection items, IPlugin plugin, Func<IPlugin, TreeListItem> func)
        {
            var item = func(plugin);
            if (item != null)
            {
                items.Add(item);

                if (plugin is IDynamicFuncProvider dfp)
                {
                    foreach (var desc in DynamicFuncHelper.GetMethodDescriptors(dfp))
                    {
                        var fxItem = item.Items.Add($"{desc.Name}");
                        fxItem.Cells[2].Value = "说明";
                        fxItem.Tag = desc;
                        fxItem.Cells[2].ForeColor = Color.Blue;
                        fxItem.Image = Properties.Resources.fx;
                    }
                }

                item.Expended = true;
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
                    StaticUnity.Encoding = Util.GetEncoding(Config.Instance.Encoding);
                }
            }

            var groupStyle = chkTempGroup.Checked ? radioButton1.Checked ? TemplateGroupStyle.Language : TemplateGroupStyle.Category : TemplateGroupStyle.None;

            if (Config.Instance.TemplateGroup != groupStyle)
            {
                _changeFlags.Add(nameof(Config.TemplateGroup));
            }
            if (Config.Instance.FontSize != (int)nudFontSize.Value)
            {
                _changeFlags.Add(nameof(Config.FontSize));
            }

            Config.Instance.CheckUpdate = chkCheckUpdate.Checked;
            Config.Instance.Source_View = chkView.Checked;
            Config.Instance.SkipWhenFileExists = chkOverFile.Checked;
            Config.Instance.TemplateGroup = groupStyle;
            Config.Instance.FontSize = (int)nudFontSize.Value;
            Config.Instance.LogLevel = cboLogLevel.SelectedIndex;

            Config.Instance.Save();

            foreach (TabPage pager in tabControl1.TabPages)
            {
                if (pager.Controls[0] is IConfigurableControl ctrl)
                {
                    ctrl.SaveChanges();
                }
            }

            if (_pluginRemoved.Count > 0)
            {
                _isNeedRestart = true;
                PlugInConfig.Remove(_pluginRemoved);
            }

            if (_isNeedRestart)
            {
                if (_hosting.ShowConfirm("必须重启 CodeBuilder 才能生效，是否立即重启?") == ShowMsgButton.Yes)
                {
                    Application.Restart();
                    return;
                }
            }

            DialogResult = DialogResult.OK;
        }

        private void mnuShop_Click(object sender, EventArgs e)
        {
            var frm = new frmPluginShop(_hosting);
            frm.OnUpdated = () =>
            {
                _isNeedRestart = true;
                OnPluginUpdated();
            };
            frm.ShowDialog();
        }

        private void mnuRemove_Click(object sender, EventArgs e)
        {
            if (!lstPlugin.HasSelectedItems)
            {
                return;
            }

            if (lstPlugin.SelectedItems[0].Level != 0)
            {
                return;
            }

            if (lstPlugin.SelectedItems[0].Cells[1].Text.Contains("CodeBuilder.Core"))
            {
                _hosting.ShowWarn("核心组件不允许移除!");
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

        private void chkTempGroup_CheckedChanged(object sender, EventArgs e)
        {
            radioButton1.Enabled = radioButton2.Enabled = chkTempGroup.Checked;

            if (!_isLoading)
            {
                _isChanged = true;
            }
        }

        private void btnAssemblyOfCommon_Click(object sender, EventArgs e)
        {
            using (var frm = new frmAssemblyOption(_hosting, "Common", () => AssemblyReferenceManager.CommonAssemblies))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    AssemblyReferenceManager.CommonAssemblies = frm.Assemblies;
                    AssemblyReferenceManager.Save();
                }
            }
        }

        private void btnAssemblyOfProfile_Click(object sender, EventArgs e)
        {
            using (var frm = new frmAssemblyOption(_hosting, "Profile", () => AssemblyReferenceManager.ProfileAssemblies))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    AssemblyReferenceManager.ProfileAssemblies = frm.Assemblies;
                    AssemblyReferenceManager.Save();
                }
            }
        }

        private void btnAssemblyOfSchema_Click(object sender, EventArgs e)
        {
            using (var frm = new frmAssemblyOption(_hosting, "Schema", () => AssemblyReferenceManager.SchemaAssemblies))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    AssemblyReferenceManager.SchemaAssemblies = frm.Assemblies;
                    AssemblyReferenceManager.Save();
                }
            }
        }

        private void lstPlugin_CellClick(object sender, TreeListCellEventArgs e)
        {
            if (e.Cell.Column.Index == 2)
            {
                var desc = e.Cell.Item.Tag as DynamicFuncAttribute;
                if (desc == null)
                {
                    return;
                }

                var point = lstPlugin.GetCellPosition(e.Cell);

                textBox1.Text = desc.Description;
                _popup.Show(lstPlugin, point.X, point.Y + lstPlugin.ItemHeight);
            }
        }

        private void combobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isLoading)
            {
                _isChanged = true;
            }
        }

        private void checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (!_isLoading)
            {
                _isChanged = true;
            }
        }
    }
}
