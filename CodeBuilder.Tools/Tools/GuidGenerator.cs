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
    public partial class GuidGenerator : UserControl, IDevHostingAccessor
    {
        public GuidGenerator()
        {
            InitializeComponent();
        }

        public IDevHosting Hosting { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < numericUpDown1.Value; i++)
            {
                var guid = Guid.NewGuid().ToString();
                if (radioButton4.Checked)
                {
                    guid = "{" + guid + "}";
                }
                else if (radioButton5.Checked)
                {
                    guid = "[" + guid + "]";
                }
                else if (radioButton6.Checked)
                {
                    guid = string.Format(textBox2.Text, guid);
                }

                if (radioButton2.Checked)
                {
                    guid = guid.ToUpper();
                }

                sb.AppendLine(guid);
            }

            textBox1.Text = sb.ToString();
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.Enabled = radioButton6.Checked;
        }

    }
}
