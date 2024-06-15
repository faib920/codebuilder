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
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;

namespace CodeBuilder
{
    public partial class frmTemplateCommit : FormBase
    {
        private readonly IDevHosting _hosting;

        public frmTemplateCommit(IDevHosting hosting)
        {
            InitializeComponent();
            _hosting = hosting;
        }

        public decimal Version { get; set; }

        public string CommitLog { get; set; }

        private void frmTemplateShare_Load(object sender, System.EventArgs e)
        {
            txtVer.Decimal = Version;
        }

        private async void btnOk_Click(object sender, System.EventArgs e)
        {
            errorProvider1.Clear();

            if (txtRemark.Text.Trim().Length == 0)
            {
                errorProvider1.SetError(txtRemark, "版本修改内容不能为空");
                return;
            }

            var client = new HttpClient().AddAccessToken();
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "category", _hosting.TemplateProvider.Name },
                { "code", _hosting.Template.Id },
                { "version", txtVer.Text },
            });

            var response = await client.PostAsync(Consts.TemplateServerUrl + "/precommit", content);
            if (!response.CheckAuthorized(_hosting))
            {
                return;
            }

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                _hosting.ShowError("无法连接到模板服务器。\r\n" + response.ReasonPhrase);
                return;
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeAnonymousType(json, new { status = 0, msg = string.Empty });
            if (result.status == 0)
            {
                errorProvider1.SetError(txtVer, result.msg);
                return;
            }

            Version = txtVer.Decimal.Value;
            CommitLog = txtRemark.Text;

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}
