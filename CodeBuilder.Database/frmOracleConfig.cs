// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System.Collections.Generic;
using Fireasy.Common.Extensions;
using Fireasy.Data;
using Fireasy.Data.Provider;

namespace CodeBuilder.Database
{
    public partial class frmOracleConfig : frmConfigBase
    {
        public frmOracleConfig()
        {
            InitializeComponent();
        }
        protected override IProvider Provider
        {
            get { return OracleProvider.Instance; }
        }

        protected override void ParseConnectionStr(ConnectionProperties properties)
        {
            if (!string.IsNullOrWhiteSpace(ConnectionString))
            {
                var parameter = Provider.GetConnectionParameter(ConnectionString);
                txtSvr.Text = parameter.Server;
                txtUser.Text = parameter.UserId;
                txtPwd.Text = parameter.Password;
            }

        }

        protected override string BuildConnectionStr()
        {
            return string.Format("data source={0};user id={1};password={2};", txtSvr.Text, txtUser.Text, txtPwd.Text);
        }
    }
}
