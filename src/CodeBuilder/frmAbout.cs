//#define BETA
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
using Fireasy.Windows.Forms;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace CodeBuilder
{
    public partial class frmAbout : FormBase
    {
        private readonly IDevHosting _hosting;
        private readonly string _app;
        private readonly string _version;
        private Popup _popup;

        public frmAbout(IDevHosting hosting)
        {
            InitializeComponent();
            Icon = Util.GetIcon();
            _hosting = hosting;
            _popup = new Popup(panel7) { DropShadowEnabled = false, Resizable = false };
        }

        public frmAbout(IDevHosting hosting, string app, string version)
            : this(hosting)
        {
            _app = app;
            _version = version;
        }

        private void frmAbout_Load(object sender, EventArgs e)
        {
#if BETA
            var version = string.IsNullOrEmpty(_app) ? GetType().Assembly.GetName().Version.ToString() + " beta" : $"{_version} beta for {_app}";
#else
            var version = string.IsNullOrEmpty(_app) ? GetType().Assembly.GetName().Version.ToString() : $"{_version} for {_app}";
#endif
            lblVer.Text = version;

            label2.Text = string.Format("Copyright © 2010 - {0} Fireasy", DateTime.Today.Year);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(WebHelper.GetRedirectUrl(_hosting, "/codebuilder"));
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            Process.Start("mailto:55570729@qq.com");
        }

        private void panel6_Click(object sender, EventArgs e)
        {
            Process.Start($"{WebHelper.HomeUrl}/codebuilder");
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            panel7.BackgroundImage = Properties.Resources.qq;
            _popup.Show(panel1);
        }

        private void panel3_Click(object sender, EventArgs e)
        {
            panel7.BackgroundImage = Properties.Resources.wx;
            _popup.Show(panel3);
        }

        private void panel5_Click(object sender, EventArgs e)
        {
            panel7.BackgroundImage = Properties.Resources.gzh;
            _popup.Show(panel5);
        }
    }
}
