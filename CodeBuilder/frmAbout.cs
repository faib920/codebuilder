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
using System.Diagnostics;
using System.Windows.Forms;

namespace CodeBuilder
{
    public partial class frmAbout : FormBase
    {
        public frmAbout()
        {
            InitializeComponent();
            Icon = Util.GetIcon();
        }

        private void frmAbout_Load(object sender, EventArgs e)
        {
            var version = GetType().Assembly.GetName().Version;
            lblVer.Text = version.ToString();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(linkLabel1.Text);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("mailto:" + linkLabel2.Text);
        }
    }
}
