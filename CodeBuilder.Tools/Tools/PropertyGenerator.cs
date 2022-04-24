// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CodeBuilder.Tools.Tools
{
    public partial class PropertyGenerator : UserControl
    {
        public PropertyGenerator()
        {
            InitializeComponent();
        }

        private void txtSource_TextChanged(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            var split = comboBox1.Text;
            if (split == "{Tab}")
            {
                split = "\t";
            }

            foreach (var r in txtSource.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (r.Trim().Length == 0)
                {
                    continue;
                }

                var chars = r.Split(new[] { split }, StringSplitOptions.None);
                if (checkBox1.Checked)
                {
                    chars = chars.Select(s => FormatPascal(s)).ToArray();
                }

                sb.AppendLine(string.Format(txtFormatter.Text, chars)); 
            }

            txtResult.Text = sb.ToString();
        }

        private void txtSource_Click(object sender, EventArgs e)
        {
        }

        private void txtResult_Click(object sender, EventArgs e)
        {
            txtResult.SelectAll();
        }

        private void PropertyGenerator_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }

        private void btnSample_Click(object sender, EventArgs e)
        {
            var split = comboBox1.SelectedIndex == 0 ? "\t" : comboBox1.Text;
            txtSource.Text = $"Name{split}姓名\r\nSex{split}性别\r\nAge{split}年龄\r\nMobile{split}手机号\r\nAddress{split}地址";
            txtSource_TextChanged(null, null);
        }

        private string FormatPascal(string str)
        {
            var pascalChars = new List<char>();

            var isUnderline = false;

            foreach (var c in str)
            {
                if (c == 95)
                {
                    isUnderline = true;
                    continue;
                }

                //帕斯卡
                if ((pascalChars.Count == 0 || isUnderline))
                {
                    if (c >= 97 && c <= 122) //小写转大写
                    {
                        pascalChars.Add((char)(c - 32));
                    }
                    else
                    {
                        pascalChars.Add(c);
                    }
                }
                else
                {
                    pascalChars.Add(c);
                }

                isUnderline = false;
            }

            return new string(pascalChars.ToArray());
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 0)
            {
                txtFormatter.Text = "/// <summary>\r\n" +
"/// {1}\r\n" +
"/// </summary>\r\n" +
"[JsonProperty(\"{0}\")]\r\n" +
"public string {0} {{ get; set; }}\r\n";
            }
            else if (comboBox2.SelectedIndex == 1)
            {
                txtFormatter.Text = "target.{0} = source.{0};";
            }
        }
    }
}
