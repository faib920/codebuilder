using CodeBuilder.Core;
using CodeBuilder.Core.Forms;
using Fireasy.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CodeBuilder
{
    public partial class frmExtension : DockFormBase
    {
        private readonly IDevHosting _hosting;

        public frmExtension(IDevHosting hosting)
        {
            InitializeComponent();
            Icon = Properties.Resources.extension;
            _hosting = hosting;
        }

        public Action<string, CodeCategory> OpenAct { get; set; }

        private void frmExtension_Load(object sender, System.EventArgs e)
        {
            Reload();
        }

        public void Reload()
        {
            if (_hosting.TemplateProvider == null || _hosting.Template == null)
            {
                return;
            }

            tlbEdit.Enabled = false;
            lstExt.Items.Clear();
            lstExt.BeginUpdate();

            var item1 = lstExt.Items.Add("common");
            item1.Tag = CodeCategory.CommonExtension;
            item1.Image = Properties.Resources.category;

            if (_hosting.Template.Extension != null)
            {
                GetFiles(item1, "common", _hosting.Template.Extension.UseBase, _hosting.Template.Extension.Common);
            }

            var item2 = lstExt.Items.Add("profile");
            item2.Tag = CodeCategory.ProfileExtension;
            item2.Image = Properties.Resources.category;

            if (_hosting.Template.Extension != null)
            {
                GetFiles(item2, "profile", _hosting.Template.Extension.UseBase, _hosting.Template.Extension.Profile);
            }

            var item3 = lstExt.Items.Add("schema");
            item3.Tag = CodeCategory.SchemaExtension;
            item3.Image = Properties.Resources.category;

            if (_hosting.Template.Extension != null)
            {
                GetFiles(item3, "schema", _hosting.Template.Extension.UseBase, _hosting.Template.Extension.Schema);
            }

            lstExt.EndUpdate();
        }

        private void GetFiles(TreeListItem item, string catalog, bool useBase, IEnumerable<string> files)
        {
            if (useBase)
            {
                foreach (var ext in new[] { "base.cs", "base.vb" })
                {
                    foreach (var fileName in Directory.GetFiles(Path.Combine(_hosting.WorkPath, "extensions", catalog), ext))
                    {
                        var subitem = item.Items.Add(new FileInfo(fileName).Name);
                        subitem.Tag = fileName;
                        subitem.Image = Properties.Resources.codefile;
                    }
                }
            }

            if (files != null)
            {
                foreach (var fileName in files)
                {
                    if (item.Items.Any(s => s.Text.Equals(fileName, StringComparison.OrdinalIgnoreCase)))
                    {
                        continue;
                    }

                    var path = Path.Combine(_hosting.WorkPath, "extensions", catalog, fileName);
                    var subitem = item.Items.Add(new FileInfo(fileName).Name);
                    subitem.Tag = path;
                    subitem.Image = Properties.Resources.codefile;
                }
            }

            item.Expended = true;
        }

        private void lstExt_ItemDoubleClick(object sender, TreeListItemEventArgs e)
        {
            if (!(e.Item.Tag is string file))
            {
                return;
            }

            OpenAct?.Invoke(file, (CodeCategory)e.Item.Parent.Tag);
        }

        private void lstExt_ItemSelectionChanged(object sender, TreeListItemSelectionEventArgs e)
        {
            tlbEdit.Enabled = lstExt.SelectedItems.Count > 0 && lstExt.SelectedItems[0].Level == 1;
        }

        private void tlbRefresh_Click(object sender, EventArgs e)
        {
            Reload();
        }

        private void tlbEdit_Click(object sender, EventArgs e)
        {
            if (lstExt.SelectedItems.Count == 0)
            {
                return;
            }

            var item = lstExt.SelectedItems[0];
            if (item.Level == 0)
            {
                return;
            }

            OpenAct?.Invoke(item.Tag as string, (CodeCategory)item.Parent.Tag);
        }

        private void tlbHelp_Click(object sender, EventArgs e)
        {
            Process.Start("http://fireasy.cn/docs/codebuilder-extension");
        }
    }
}
