// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Forms;
using System;
using System.Windows.Forms;

namespace CodeBuilder.JsonTool
{
    public partial class frmMark : FormBase
    {
        public frmMark()
        {
            InitializeComponent();
        }

        public string MarkText
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
