// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using CodeBuilder.Core.Source;
using System;
using System.Collections.Generic;
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

        public List<Table> Tables { get; private set; }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSQL.Text))
            {
                _hosting.ShowWarn("SQL 不能为空。");
                return;
            }

            try
            {
                Tables = ((SourceProvider)_hosting.SourceProvider).ParseSQL(txtSQL.Text);

                DialogResult = DialogResult.OK;
            }
            catch (Exception exp)
            {
                _hosting.ShowError(exp);
            }
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
