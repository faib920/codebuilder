// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using CodeBuilder.Core.Template;
using CodeBuilder.Core.Forms;
using Fireasy.Common.Serialization;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace CodeBuilder
{
    public partial class frmTemplateCopy : FormBase
    {
        private readonly IDevHosting _hosting;

        public frmTemplateCopy(IDevHosting hosting)
        {
            InitializeComponent();
            _hosting = hosting;
        }

        public TemplateDefinition Template { get; set; }

        public string NewTemplateId
        {
            get
            {
                return txtId.Text;
            }
        }

        public string NewTemplateName
        {
            get
            {
                return txtNew.Text;
            }
        }

        private void frmTemplateCopy_Load(object sender, EventArgs e)
        {
            txtSource.Text = Template.Id;
            txtId.Text = Template.Id + "_Copy";
            txtNew.Text = Template.Name + "_副本";
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                _hosting.ShowWarn("模板标识不能为空。");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNew.Text))
            {
                _hosting.ShowWarn("模板名称不能为空。");
                return;
            }

            var newPath = Path.Combine(_hosting.TemplateProvider.WorkDir, txtId.Text);
            var oldPath = Path.Combine(_hosting.TemplateProvider.WorkDir, Template.Id);

            if (Directory.Exists(newPath))
            {
                _hosting.ShowWarn("[" + txtId.Text + "] 已经存在，请重新输入。");
                return;
            }

            CopyDirectory(oldPath, newPath);

            var option = new JsonSerializeOption { Indent = true };
            option.Converters.Add(new PartLoopConverter());
            var json = new JsonSerializer(option);
            Template.Id = txtId.Text;
            Template.Name = txtNew.Text;
            var content = json.Serialize(Template);

            var path = Path.Combine(_hosting.TemplateProvider.WorkDir, Template.Id + ".template");
            File.WriteAllText(path, content, Encoding.UTF8);

            DialogResult = DialogResult.OK;
        }

        private void CopyDirectory(string source, string target)
        {
            if (!Directory.Exists(target))
            {
                Directory.CreateDirectory(target);
            }

            foreach (var file in Directory.GetFiles(source))
            {
                File.Copy(file, Path.Combine( target, new FileInfo(file).Name));
            }

            foreach (var dir in Directory.GetDirectories(source))
            {
                CopyDirectory(dir, Path.Combine(target, new DirectoryInfo(dir).Name));
            }
        }
    }
}
