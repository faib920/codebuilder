// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using CodeBuilder.Core.Forms;
using System;
using System.Windows.Forms;

namespace CodeBuilder
{
    public partial class frmTemplateShopConfig : FormBase
    {
        public frmTemplateShopConfig()
        {
            InitializeComponent();
        }

        private void frmTemplateShopConfig_Load(object sender, EventArgs e)
        {
            txtServer.Text = Config.Instance.TemplateServerUrl;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Config.Instance.TemplateServerUrl = txtServer.Text;
            Config.Save();

            DialogResult = DialogResult.OK;
        }
    }
}
