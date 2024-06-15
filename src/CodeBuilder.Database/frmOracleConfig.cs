// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System.Collections.Generic;
using CodeBuilder.Core;
using Fireasy.Common.Extensions;
using Fireasy.Data;
using Fireasy.Data.Provider;

namespace CodeBuilder.Database
{
    public partial class frmOracleConfig : frmConfigBase
    {
        public frmOracleConfig(IDevHosting hosting)
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
                txtUser.Text = parameter.UserId;
                txtPwd.Text = parameter.Password;

                chkProtect.Checked = protector.IsProtected(parameter.Password);
            }
        }

        protected override string BuildConnectionStr()
        {
            var str = $"data source={txtSvr.Text};user id={txtUser.Text};password={txtPwd.Text}";

            if (chkProtect.Checked)
            {
                var protector = _hosting.ServiceProvider.TryGetService<IConnectionStringProtector>();
                str = (string)protector.Encrypt(str);
            }

            return str;
        }
    }
}
