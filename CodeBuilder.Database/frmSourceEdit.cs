// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using Fireasy.Data.Provider;
using System;
using System.Windows.Forms;

namespace CodeBuilder.Database
{
    public partial class frmSourceEdit : Form
    {
        private string _providerName;
        private readonly IDevHosting _hosting;

        public frmSourceEdit(IDevHosting hosting)
        {
            InitializeComponent();
            _hosting = hosting;
        }

        public frmSourceEdit(IDevHosting hosting, DbSourceStruct db)
            : this(hosting)
        {
            Current = db;
            _providerName = db.Type;
            Text = "添加 " + db.Type + " 数据源";
            txtName.Text = db.Name;
            txtConnStr.Text = db.ConnectionString;
        }

        public DbSourceStruct Current { get; set; }

        private void btnOk_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            if (txtName.Text.Trim() == string.Empty)
            {
                errorProvider1.SetError(txtName, "请输入数据源名称");
                return;
            }

            if (txtConnStr.Text.Trim() == string.Empty)
            {
                errorProvider1.SetError(txtConnStr, "请输入数据库连接字符串");
                return;
            }


            Current.Name = txtName.Text;
            Current.ConnectionString = txtConnStr.Text;
            DialogResult = DialogResult.OK;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            if (txtConnStr.Text.Trim() == string.Empty)
            {
                errorProvider1.SetError(txtConnStr, "请输入数据库连接字符串");
                return;
            }

            var sourceName = txtName.Text;
            if (string.IsNullOrEmpty(sourceName))
            {
                sourceName = "未命名";
            }

            var provider = ProviderHelper.GetDefinedProviderInstance(Current.Type);
            if (provider == null)
            {
                _hosting.ShowError(string.Format("没有发现 {0} 的数据库适配器。", Current.Type));
            }

            using (var db = new Fireasy.Data.Database(txtConnStr.Text, provider))
            {
                try
                {
                    var exp = db.TryConnect();
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
            }
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            var frm = ConfigForms.GetConfigForm(_providerName);
            if (frm == null)
            {
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
