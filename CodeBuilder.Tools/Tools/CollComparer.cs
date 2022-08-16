// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CodeBuilder.Tools.Tools
{
    public partial class CollComparer : UserControl, IDevHostingAccessor
    {
        public CollComparer()
        {
            InitializeComponent();
        }

        public IDevHosting Hosting { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            var source1 = txtSource1.Text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var source2 = txtSource2.Text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            txtDiff1.Clear();
            txtDiff2.Clear();

            var c1 = 0;
            var c2 = 0;

            foreach (var s in source1)
            {
                if (!source2.Contains(s))
                {
                    txtDiff1.AppendText(s + "\r\n");
                    c1++;
                }
            }

            foreach (var s in source2)
            {
                if (!source1.Contains(s))
                {
                    txtDiff2.AppendText(s + "\r\n");
                    c2++;
                }
            }

            label4.Text = c1 == 0 ? "在集合2中不存在" : "在集合2中不存在(" + c1 + ")";
            label3.Text = c2 == 0 ? "在集合1中不存在" : "在集合1中不存在(" + c2 + ")";

            if (txtDiff1.Text.Length == 0 && txtDiff2.Text.Length == 0)
            {
                Hosting.ShowInfo("^_^ 两边集合完全一致。");
            }
        }

        private void txtSource1_TextChanged(object sender, EventArgs e)
        {
            var source1 = txtSource1.Text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            label1.Text = "集合1(" + source1.Length + ")";
        }

        private void txtSource2_TextChanged(object sender, EventArgs e)
        {
            var source2 = txtSource2.Text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            label2.Text = "集合2(" + source2.Length + ")";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtSource1.Text = "1234567\r\n4433455\r\n1955334\r\n9450222";
            txtSource2.Text = "1234567\r\n4433455\r\n9955336\r\n9450222\r\n9555443";

            button1_Click(button1, null);
        }
    }
}
