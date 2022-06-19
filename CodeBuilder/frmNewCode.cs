// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using CodeBuilder.Core.Forms;
using System;
using System.IO;
using System.Windows.Forms;

namespace CodeBuilder
{
    public partial class frmNewCode : FormBase
    {
        private readonly IDevHosting _hosting;

        public frmNewCode(IDevHosting hosting)
        {
            InitializeComponent();
            _hosting = hosting;
        }

        public ProjectTemplate Templage { get; private set; }

        private void frmNewCode_Load(object sender, EventArgs e)
        {
            var path = Path.Combine(_hosting.WorkPath, "projects");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            foreach (var file in Directory.GetFiles(path, "*.project", SearchOption.AllDirectories))
            {
                var info = new FileInfo(file);
                var name = info.Name.Substring(0, info.Name.Length - info.Extension.Length);
                var template = new ProjectTemplate();
                template.Name = name;
                template.FileName = file;
                template.Syntax = info.Directory.Name;
                var item = lstTemplate.Items.Add(name);
                item.Image = Properties.Resources.project;
                item.GroupKey = info.Directory.Name;
                item.Tag = template;
            }

            lstTemplate.Grouping(true);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (lstTemplate.SelectedItems.Count == 0)
            {
                _hosting.ShowWarn("请选择一个模板。");
                return;
            }

            Templage = lstTemplate.SelectedItems[0].Tag as ProjectTemplate;
            DialogResult = DialogResult.OK;
        }

        private void lstTemplate_ItemDoubleClick(object sender, Fireasy.Windows.Forms.TreeListItemEventArgs e)
        {
            btnOk_Click(null, null);
        }
    }
}
