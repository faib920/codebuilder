// -----------------------------------------------------------------------
// <copyright company="Fireasy"
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
    public partial class Base64Converter : UserControl, IDevHostingAccessor
    {
        public Base64Converter()
        {
            InitializeComponent();
        }

        public IDevHosting Hosting { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                txtEnc.Text = Convert.ToBase64String(Encoding.UTF8.GetBytes(txtSource.Text));
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
                txtSource.Text = Encoding.UTF8.GetString(Convert.FromBase64String(txtEnc.Text));
            }
            catch (Exception)
            {
                Hosting.ShowError("无法转换!");
            }
        }
    }
}
