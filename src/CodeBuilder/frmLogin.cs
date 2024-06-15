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
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeBuilder
{
    public partial class frmLogin : FormBase
    {
        private readonly IDevHosting _hosting;

        public frmLogin(IDevHosting hosting)
        {
            InitializeComponent();
            Icon = Util.GetIcon();
            _hosting = hosting;
        }

        public string UserAccount { get; set; }

        public string UserName { get; set; }

        public string AccessToken { get; set; }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            txtUser.Text = Config.Instance.UserAccount;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if (txtUser.Text.Trim().Length > 0)
            {
                txtPwd.Focus();
            }
            else
            {
                btnOk.Focus();
            }
        }

        private async void btnOk_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            btnOk.Enabled = false;

            if (txtUser.Text.Trim().Length == 0)
            {
                lblMsg.Text = "请输入账号";
                txtUser.Focus();
                btnOk.Enabled = true;
                return;
            }
            if (txtPwd.Text.Trim().Length == 0)
            {
                lblMsg.Text = "请输入密码";
                txtPwd.Focus();
                btnOk.Enabled = true;
                return;
            }

            var result = await IdentityHelper.LoginAsync(txtUser.Text, txtPwd.Text);
            if (!result.Succeed)
            {
                lblMsg.Text = result.Message;
                btnOk.Enabled = true;
                return;
            }

            lblMsg.Text = "登录成功";

            await Task.Delay(500);

            btnOk.Enabled = true;

            UserAccount = txtUser.Text;
            UserName = result.Data.name;
            AccessToken = result.Data.access_token;

            DialogResult = DialogResult.OK;

            Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start($"{WebHelper.HomeUrl}/register");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start($"{WebHelper.HomeUrl}/findpassword");
        }

        private void txtUser_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPwd.Focus();
            }
        }

        private void txtPwd_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOk_Click(null, null);
            }
        }
    }
}
