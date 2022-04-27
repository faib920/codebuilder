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

namespace CodeBuilder.Tools
{
    public partial class frmConnEncrypt : FormBase
    {
        public frmConnEncrypt()
        {
            InitializeComponent();
            Icon = Util.GetIcon();
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            var type = Type.GetType("Fireasy.Data.ConnectionStringEncryptHelper, Fireasy.Data");
            var method = type.GetMethod("Encrypt", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            txt2.Text = method.Invoke(null, new object[] { txt1.Text }).ToString();
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            var type = Type.GetType("Fireasy.Data.ConnectionStringEncryptHelper, Fireasy.Data");
            var method = type.GetMethod("Decrypt", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            try
            {
                txt1.Text = method.Invoke(null, new object[] { txt2.Text }).ToString();
            }
            catch (Exception exp)
            {
                txt1.Text = "无法解密";
            }
        }
    }
}
