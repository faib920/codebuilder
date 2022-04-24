// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using Fireasy.Data;
using Fireasy.Data.Provider;
using System;
using System.Windows.Forms;

namespace CodeBuilder.Database
{
    public partial class frmFirebirdConfig : frmConfigBase
    {
        public frmFirebirdConfig()
        {
            InitializeComponent();
        }

        protected override IProvider Provider 
        {
            get { return FirebirdProvider.Instance; }
        }

        protected override void ParseConnectionStr(ConnectionProperties properties)
        {
            if (!string.IsNullOrWhiteSpace(ConnectionString))
            {
                var parameter = Provider.GetConnectionParameter(ConnectionString);
                txtSvr.Text = parameter.Server;
                txtDb.Text = parameter.Database;
                txtUser.Text = parameter.UserId;
                txtPwd.Text = parameter.Password;
                txtPort.Text = properties.TryGetValue("port");
            }
        }

        protected override string BuildConnectionStr()
        {
            var str = string.Format("server={0};Database={1};userid={2};password={3}", txtSvr.Text, txtDb.Text, txtUser.Text, txtPwd.Text);
            if (!string.IsNullOrWhiteSpace(txtPort.Text))
            {
                str += ";port=" + txtPort.Text;
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
