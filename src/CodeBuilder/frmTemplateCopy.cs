// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
            errorProvider1.Clear();

            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                errorProvider1.SetError(txtId, "模板标识不能为空");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNew.Text))
            {
                errorProvider1.SetError(txtNew, "模板名称不能为空");
                return;
            }

            var newPath = Path.Combine(_hosting.TemplateProvider.WorkDir, txtId.Text);
            var oldPath = Path.Combine(_hosting.TemplateProvider.WorkDir, Template.Id);

            if (Directory.Exists(newPath))
            {
                errorProvider1.SetError(txtId, txtId.Text + " 已经存在，请重新输入");
                return;
            }

            CopyDirectory(oldPath, newPath);

            var newConfigFileName = Path.Combine(_hosting.TemplateProvider.WorkDir, txtId.Text + ".template");
            var content = File.ReadAllText(Template.ConfigFileName);

            var setting = new JsonSerializerSettings { Formatting = Formatting.Indented };
            var jobj = (JObject)JsonConvert.DeserializeObject(content);
            jobj.Property("Name").Value = txtNew.Text;
            content = JsonConvert.SerializeObject(jobj, setting);
            File.WriteAllText(newConfigFileName, content);

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
