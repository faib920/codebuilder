// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using System.Windows.Forms;

namespace CodeBuilder.Database
{
    public partial class frmSQLEditor : Form
    {
        private readonly IDevHosting _hosting;

        public frmSQLEditor(IDevHosting hosting)
        {
            InitializeComponent();
            Icon = Util.GetIcon();

            txtSQL.TextEditorProperties.CaretLine = true;
            txtSQL.TextEditorProperties.ConvertTabsToSpaces = true;
            _hosting = hosting;
        }

        public string SQL
        {
            get { return txtSQL.Text; }
        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSQL.Text))
            {
                _hosting.ShowWarn("SQL 不能为空。");
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            txtSQL.Text += @"
customers=>SELECT CustomerId AS Id, CompanyName AS Name FROM customers;
employees=>SELECT * FROM employees;
SELECT CustomerId AS Id, CompanyName AS Name FROM customers;
";
        }
    }
}
