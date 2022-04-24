// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using Fireasy.Data;
using Fireasy.Data.Provider;

namespace CodeBuilder.Database
{
    public partial class frmMsSqlConfig : frmConfigBase
    {
        public frmMsSqlConfig()
        {
            InitializeComponent();
        }
        protected override IProvider Provider
        {
            get { return MsSqlProvider.Instance; }
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
            var str = string.Format("data source={0};initial catalog={1};user id={2};password={3}", txtSvr.Text, txtDb.Text, txtUser.Text, txtPwd.Text);
            if (!string.IsNullOrWhiteSpace(txtPort.Text))
            {
                str += ";port=" + txtPort.Text;
            }

            return str;
        }
    }
}
