// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Template;
using CodeBuilder.Core.Forms;
using Fireasy.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace CodeBuilder
{
    public partial class frmTemplate : DockFormBase
    {
        private readonly DevHosting _hosting;

        public frmTemplate(DevHosting hosting)
        {
            InitializeComponent();
            Icon = Properties.Resources.template;
            _hosting = hosting;
        }

        public Action<TemplateFile> OpenAct { get; set; }

        public Action<TemplateDefinition> OpenTemplateAct { get; set; }

        public Action TemplateAct { get; set; }

        public void Reload()
        {
            if (_hosting.TemplateProvider == null || _hosting.Template == null)
            {
                return;
            }

            tlbEdit.Enabled = false;
            _hosting.Template = TemplateParser.Parse(_hosting.Template.ConfigFileName);

            var storage = _hosting.TemplateProvider.GetStorage(_hosting.Template);

            lstPart.Items.Clear();
            lstPart.BeginUpdate();

            FillItems(lstPart.Items, storage.Directories, storage.Files);

            lstPart.EndUpdate();
        }

        private void FillItems(TreeListItemCollection items, List<TemplateDirectory> directories, List<TemplateFile> files)
        {
            foreach (var dir in directories)
            {
                var item = new TreeListItem(dir.Name);
                items.Add(item);

                item.Image = Properties.Resources.category;

                if (!string.IsNullOrEmpty(dir.Color))
                {
                    item.ForeColor = FromColor(dir.Color);
                }

                FillItems(item.Items, dir.Directories, dir.Files);

                item.Expended = true;
            }

            foreach (var file in files)
            {
                var item = new TreeListItem(file.ToString());
                items.Add(item);
                item.Tag = file;
                item.Image = Properties.Resources.fileT;

                if (!string.IsNullOrEmpty(file.Color))
                {
                    item.ForeColor = FromColor(file.Color);
                }
            }
        }

        private void frmTemplate_Load(object sender, EventArgs e)
        {
            Reload();
        }

        private void lstPart_ItemDoubleClick(object sender, TreeListItemEventArgs e)
        {
            if (!(e.Item.Tag is TemplateFile file))
            {
                return;
            }

            OpenAct?.Invoke(file);
        }

        private void lstPart_ItemSelectionChanged(object sender, TreeListItemSelectionEventArgs e)
        {
            tlbEdit.Enabled = lstPart.SelectedItems.Count > 0 && lstPart.SelectedItems[0].Tag is TemplateFile;
        }

        private void tlbNew_Click(object sender, EventArgs e)
        {
            using (var frm = new frmTemplateEditor(_hosting))
            {
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    TemplateAct();
                }
            }
        }

        private void tlbEdit_Click(object sender, EventArgs e)
        {
            if (lstPart.SelectedItems.Count == 0)
            {
                return;
            }

            var item = lstPart.SelectedItems[0];
            if (!(item.Tag is TemplateFile file))
            {
                return;
            }

            OpenAct?.Invoke(file);
        }

        private void tlbRefresh_Click(object sender, EventArgs e)
        {
            Reload();
        }

        private void tlbCopy_Click(object sender, EventArgs e)
        {
            if (_hosting.TemplateProvider == null || _hosting.Template == null)
            {
                _hosting.ShowInfo("当前没有选定模板，请从【模板】菜单中选择。");
                return;
            }

            using (var frm = new frmTemplateCopy(_hosting) { Template = _hosting.Template })
            {
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    _hosting.ShowInfo("模板复制成功，请从主窗口【模板】菜单中选择。");
                    TemplateAct();
                }
            }
        }

        private void tlbEditAsCode_Click(object sender, EventArgs e)
        {
            if (_hosting.TemplateProvider == null || _hosting.Template == null)
            {
                _hosting.ShowInfo("当前没有选定模板，请从【模板】菜单中选择。");
                return;
            }

            if (OpenTemplateAct != null && _hosting.Template != null)
            {
                OpenTemplateAct(_hosting.Template);
            }
        }

        private void tlbEditTemp_Click(object sender, EventArgs e)
        {
            if (_hosting.TemplateProvider == null || _hosting.Template == null)
            {
                _hosting.ShowInfo("当前没有选定模板，请从【模板】菜单中选择。");
                return;
            }

            using (var frm = new frmTemplateEditor(_hosting) { Template = _hosting.Template })
            {
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    TemplateAct();
                    Reload();
                }
            }
        }

        private void tlbHelp_Click(object sender, EventArgs e)
        {
            Process.Start("http://fireasy.cn/docs/codebuilder-template");
        }

        private void tlbExport_Click(object sender, EventArgs e)
        {
            if (_hosting.TemplateProvider == null || _hosting.Template == null)
            {
                _hosting.ShowInfo("当前没有选定模板，请从【模板】菜单中选择。");
                return;
            }

            using (var dialog = new SaveFileDialog()
            {
                Filter = "模板定义包(*.tdp)|*.tdp",
                Title = "导出模板",
                FileName = _hosting.Template.Name + ".tdp"
            })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var bytes = TemplateHelper.ZipPackage(_hosting, _hosting.Template);

                    File.WriteAllBytes(dialog.FileName, bytes);
                }
            }
        }

        private Color FromColor(string color)
        {
            var d = color.Split(',');
            return Color.FromArgb(int.Parse(d[0]), int.Parse(d[1]), int.Parse(d[2]));
        }
    }
}
