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
using Microsoft.Win32;
using System;
using System.Windows.Forms;

namespace CodeBuilder.Database
{
    public partial class frmOdbcConfig : frmConfigBase
    {
        public frmOdbcConfig(IDevHosting hosting)
        {
            InitializeComponent();
            _hosting = hosting;
        }

        private void frmOdbcConfig_Load(object sender, EventArgs e)
        {
            GetDrivers();
        }

        private void GetDrivers()
        {
            var reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\ODBC\ODBCINST.INI\ODBC Drivers");
            foreach (var r in reg.GetValueNames())
            {
                cboDrivers.Items.Add(r);
            }
        }

        protected override void ParseConnectionStr(ConnectionProperties properties)
        {
            if (!string.IsNullOrWhiteSpace(ConnectionString))
            {
                var protector = _hosting.ServiceProvider.TryGetService<IConnectionStringProtector>();

                var parameter = Provider.GetConnectionParameter(ConnectionString);
                cboDrivers.Text = properties.TryGetValue("driver");
                txtSvr.Text = parameter.Server;
                txtFile.Text = properties.TryGetValue("dbq");
                txtDb.Text = parameter.Database;
                txtUser.Text = parameter.UserId;
                txtPwd.Text = parameter.Password;
                txtPort.Text = properties.TryGetValue("port");

                chkProtect.Checked = protector.IsProtected(parameter.Password);
            }
        }

        protected override string BuildConnectionStr()
        {
            var str = $"driver={cboDrivers.Text};";
            if (!string.IsNullOrEmpty(txtFile.Text))
            {
                str += $"dbq={txtFile.Text};";
            }
            else if (!string.IsNullOrWhiteSpace(txtSvr.Text))
            {
                str += $"server={txtSvr.Text};";
            }
            str += $"database={txtDb.Text};uid={txtUser.Text};pwd={txtPwd.Text}";
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
            using (var dialog = new OpenFileDialog {  Filter = "所有文件(*.*)|*.*" })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtFile.Text = dialog.FileName;
                }
            }
        }
    }
}
