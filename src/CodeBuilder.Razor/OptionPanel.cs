// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using System.Windows.Forms;

namespace CodeBuilder.Razor
{
    public partial class OptionPanel : UserControl
    {
        private readonly IDevHosting _hosting;

        public OptionPanel(IDevHosting hosting)
        {
            InitializeComponent();
            _hosting = hosting;
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            using (var frm = new frmOption(_hosting))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                }
            }
        }
    }
}
