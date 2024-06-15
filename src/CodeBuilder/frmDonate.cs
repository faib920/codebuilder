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

namespace CodeBuilder
{
    public partial class frmDonate : FormBase
    {
        public frmDonate()
        {
            InitializeComponent();
            Icon = Util.GetIcon();
        }

        private void panel1_Click(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}
