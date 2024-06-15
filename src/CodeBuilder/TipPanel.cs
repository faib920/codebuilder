// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Windows.Forms;

namespace CodeBuilder
{
    public partial class TipPanel : UserControl
    {
        public TipPanel()
        {
            InitializeComponent();
        }

        public string Title { get; set; }

        public string Message { get; set; }

        private void tipPanel_Click(object sender, EventArgs e)
        {
            frmTip.Show(Parent, this, Title, Message);
        }
    }
}
