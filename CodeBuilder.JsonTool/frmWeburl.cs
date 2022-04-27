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
    public partial class frmWeburl : FormBase
    {
        public frmWeburl()
        {
            InitializeComponent();
        }

        public string Weburl
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
