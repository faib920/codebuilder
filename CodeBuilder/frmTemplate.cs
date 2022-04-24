// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Template;
using Fireasy.Windows.Forms;
using System;
using System.Collections.Generic;

namespace CodeBuilder
{
    public partial class frmTemplate : DockForm
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

                FillItems(item.Items, dir.Directories, dir.Files);

                item.Expended = true;
            }

            foreach (var file in files)
            {
                var item = new TreeListItem(file.Name);
                items.Add(item);
                item.Tag = file;
                item.Image = Properties.Resources.file;
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
            if (_hosting.TemplateProvider == null || _hosting.Template == null)
            {
                _hosting.ShowInfo("当前没有选择模板。");
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

        private void mnuRefresh_Click(object sender, EventArgs e)
        {
            Reload();
        }

        private void tlbCopy_Click(object sender, EventArgs e)
        {
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
            if (OpenTemplateAct != null && _hosting.Template != null)
            {
                OpenTemplateAct(_hosting.Template);
            }
        }
    }
}
