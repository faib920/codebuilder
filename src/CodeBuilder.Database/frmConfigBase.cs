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
using Fireasy.Common.Extensions;
using Fireasy.Data;
using Fireasy.Data.Extensions;
using Fireasy.Data.Provider;
using System;
using System.Windows.Forms;

namespace CodeBuilder.Database
{
    public partial class frmConfigBase : FormBase, IConnectionConfig
    {
        protected IDevHosting _hosting;

        public frmConfigBase()
        {
            InitializeComponent();
        }

        public string ConnectionString { get; set; }

        protected virtual IProvider Provider { get; private set; }

        public DialogResult ShowDialog(IntPtr handle)
        {
            return ShowDialog();
        }

        private void frmConfigBase_Load(object sender, System.EventArgs e)
        {
            if (ConnectionString != null)
            {
                ParseConnectionStr(new ConnectionString(ConnectionString).Properties);
            }
        }

        protected virtual void ParseConnectionStr(ConnectionProperties properties)
        {
        }

        protected virtual string BuildConnectionStr()
        {
            return (string)ConnectionString;
        }

        public frmConfigBase SetProvider(IProvider provider)
        {
            Provider = provider;
            return this;
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            ConnectionString = BuildConnectionStr();
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            var connstr = BuildConnectionStr();
            var databaseFactory = _hosting.ServiceProvider.TryGetService<IDatabaseFactory>();

            using (var db = databaseFactory.CreateDatabase(Provider.ProviderName, connstr))
            {
                db.Provider.UpdateConnectionParameter(db.ConnectionString, p => p.ConnectTimeout = "3");

                Cursor = Cursors.WaitCursor;

                try
                {
                    var exp = await db.TryConnectAsync();
                    if (exp == null)
                    {
                        _hosting.ShowInfo("连接成功。");
                    }
                    else
                    {
                        throw exp;
                    }
                }
                catch (Exception exp)
                {
                    _hosting.ShowError(string.Format("连接失败。详细信息如下：\n\n{0}", exp.Message));
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
        }
    }
}
