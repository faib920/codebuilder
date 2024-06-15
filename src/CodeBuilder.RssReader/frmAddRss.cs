// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Forms;
using CodeHollow.FeedReader;
using System;
using System.Net.Http;

namespace CodeBuilder.RssReader
{
    public partial class frmAddRss : FormBase
    {
        public frmAddRss(bool isNew = false)
        {
            InitializeComponent();

            if (isNew)
            {
                Text = "添加源";
            }
        }

        public string RssName
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        public string RssUrl
        {
            get { return textBox2.Text; }
            set { textBox2.Text = value; }
        }

        public bool DefaultLoad
        {
            get { return checkBox1.Checked; }
            set { checkBox1.Checked = value; }
        }

        private async void btnOk_Click(object sender, System.EventArgs e)
        {
            errorProvider1.Clear();

            if (textBox1.Text.Trim().Length == 0)
            {
                errorProvider1.SetError(textBox1, "请输入RSS源名称");
                return;
            }
            if (textBox2.Text.Trim().Length == 0)
            {
                errorProvider1.SetError(textBox2, "请输入RSS源地址");
                return;
            }

            try
            {
                var client = new HttpClient();
                var response = await client.GetAsync(textBox2.Text);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    errorProvider1.SetError(textBox2, "输入的RSS源地址无效");
                    return;
                }

                var feed = await FeedReader.ReadAsync(textBox2.Text);
            }
            catch (Exception exp)
            {
                errorProvider1.SetError(textBox2, exp.Message);
                return;
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
    }
}
