// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using Fireasy.Common.Extensions;
using Fireasy.Data;
using System;
using System.Windows.Forms;

namespace CodeBuilder.Database
{
    public partial class frmFirebirdConfig : frmConfigBase
    {
        public frmFirebirdConfig(IDevHosting hosting)
        {
            InitializeComponent();
            _hosting = hosting;
        }

        protected override void ParseConnectionStr(ConnectionProperties properties)
        {
            if (!string.IsNullOrWhiteSpace(ConnectionString))
            {
                var protector = _hosting.ServiceProvider.TryGetService<IConnectionStringProtector>();

                var parameter = Provider.GetConnectionParameter(ConnectionString);
                txtSvr.Text = parameter.Server;
                txtDb.Text = parameter.Database;
                txtUser.Text = parameter.UserId;
                txtPwd.Text = parameter.Password;
                txtPort.Text = properties.TryGetValue("port");

                chkProtect.Checked = protector.IsProtected(parameter.Password);
            }
        }

        protected override string BuildConnectionStr()
        {
            var str = $"server={txtSvr.Text};Database={txtDb.Text};userid={txtUser.Text};password={txtPwd.Text}";
            if (!string.IsNullOrWhiteSpace(txtPort.Text))
            {
                str += ";port=" + txtPort.Text;
            }

            if (chkProtect.Checked)
            {
                var protector = _hosting.ServiceProvider.TryGetService<IConnectionStringProtector>();
                str = (string)protector.Encrypt(str);
            }

            return str;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Firebird DB|*.fdb|所有文件|*.*";
                dialog.FileName = txtDb.Text;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtDb.Text = dialog.FileName;
                }
            }
        }
    }
}
