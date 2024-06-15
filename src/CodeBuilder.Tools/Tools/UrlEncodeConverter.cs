// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using System;
using System.Text;
using System.Windows.Forms;

namespace CodeBuilder.Tools.Tools
{
    public partial class UrlEncodeConverter : UserControl, IDevHostingAccessor
    {
        public UrlEncodeConverter()
        {
            InitializeComponent();
        }

        public IDevHosting Hosting { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                txtEnc.Text = Uri.EscapeDataString(txtSource.Text);
            }
            catch (Exception)
            {
                Hosting.ShowError("无法转换!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                txtSource.Text = Uri.UnescapeDataString(txtEnc.Text);
            }
            catch (Exception)
            {
                Hosting.ShowError("无法转换!");
            }
        }
    }
}
