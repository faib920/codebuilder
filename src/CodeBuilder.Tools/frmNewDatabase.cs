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
using System;

namespace CodeBuilder.Tools
{
    public partial class frmNewDatabase : FormBase
    {
        private readonly IDevHosting _hosting;

        public frmNewDatabase(IDevHosting hosting)
        {
            InitializeComponent();
            _hosting = hosting;
        }

        public Func<string, bool> CheckFunc { get; set; }

        public string Database => textBox1.Text;

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                errorProvider1.SetError(textBox1, "不能为空");
                return;
            }
            if (CheckFunc(textBox1.Text))
            {
                _hosting.ShowWarn("数据库标识已经存在，请重新输入。");
                return;
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
    }
}
