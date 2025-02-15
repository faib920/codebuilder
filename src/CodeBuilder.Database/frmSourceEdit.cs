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
    public partial class frmSourceEdit : FormBase
    {
        private string _providerName;
        private readonly IDevHosting _hosting;

        public frmSourceEdit(IDevHosting hosting)
        {
            InitializeComponent();
            _hosting = hosting;
        }

        public frmSourceEdit(IDevHosting hosting, DbSourceStruct db, bool isNew = false)
            : this(hosting)
        {
            Current = db;
            _providerName = db.Type;
            Text = (isNew ? "添加 " : "修改 ") + db.Type + " 数据源";
            txtName.Text = db.Name;
            txtConnStr.Text = db.ConnectionString;
        }

        public DbSourceStruct Current { get; set; }

        private void btnOk_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            if (txtName.Text.Trim() == string.Empty)
            {
                errorProvider1.SetError(txtName, "数据源名称不能为空");
                return;
            }

            if (txtConnStr.Text.Trim() == string.Empty)
            {
                errorProvider1.SetError(txtConnStr, "数据库连接字符串不能为空");
                return;
            }

            Current.Name = txtName.Text;
            Current.ConnectionString = txtConnStr.Text;
            DialogResult = DialogResult.OK;
        }

        private async void btnTest_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            if (txtConnStr.Text.Trim() == string.Empty)
            {
                errorProvider1.SetError(txtConnStr, "数据库连接字符串不能为空");
                return;
            }

            var sourceName = txtName.Text;
            if (string.IsNullOrEmpty(sourceName))
            {
                sourceName = "未命名";
            }

            var databaseFactory = _hosting.ServiceProvider.TryGetService<IDatabaseFactory>();
            var providerManager = _hosting.ServiceProvider.TryGetService<IProviderManager>();
            var provider = providerManager.GetDefinedProvider(Current.Type);
            if (provider == null)
            {
                _hosting.ShowError(string.Format("没有发现 {0} 的数据库适配器。", Current.Type));
            }

            using (var db = databaseFactory.CreateDatabase(Current.Type, txtConnStr.Text))
            {
                db.Provider.UpdateConnectionParameter(db.ConnectionString, p => p.ConnectTimeout = "3");

                Cursor = Cursors.WaitCursor;

                try
                {
                    var exp = await db.TryConnectAsync();
                    if (exp == null)
                    {
                        _hosting.ShowInfo(string.Format("{0} 连接成功。", sourceName));
                    }
                    else
                    {
                        throw exp;
                    }
                }
                catch (Exception exp)
                {
                    _hosting.ShowError(string.Format("{0} 连接失败。详细信息如下：\n\n{1}", sourceName, exp.Message));
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            var frm = ConfigForms.GetConfigForm(_hosting.ServiceProvider, _providerName);
            if (frm == null)
            {
                _hosting.ShowInfo("未提供 " + _providerName + " 的向导窗口。");
                return;
            }

            frm.ConnectionString = txtConnStr.Text;
            if (frm.ShowDialog(Handle) == DialogResult.OK)
            {
                txtConnStr.Text = frm.ConnectionString;
            }
        }
    }
}
