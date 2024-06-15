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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CodeBuilder.Tools.Tools
{
    public partial class TextConverter : UserControl, IDevHostingAccessor
    {
        public TextConverter()
        {
            InitializeComponent();
        }

        public IDevHosting Hosting { get; set; }

        private void txtSource_TextChanged(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            var split = comboBox1.Text;
            if (split == "{Tab}")
            {
                split = "\t";
            }

            var regex = new Regex(@"\{(\d)\}", RegexOptions.Compiled);

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

                var str = regex.Replace(txtFormatter.Text, match =>
                {
                    var index = Convert.ToInt32(match.Groups[1].Value);
                    if (index <= chars.Length - 1)
                    {
                        return chars[index];
                    }

                    return match.Groups[0].Value;
                });

                sb.AppendLine(str);
            }

            txtResult.Text = sb.ToString();
            txtResult.Refresh();
        }

        private void PropertyGenerator_Load(object sender, EventArgs e)
        {
            var list = TextConvertConfig.LoadConfig(Hosting);

            foreach (var k in list)
            {
                comboBox2.Items.Add(k);
            }

            if (list.Count > 0)
            {
                comboBox2.SelectedIndex = 0;
            }

            comboBox1.SelectedIndex = 0;
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            txtSource_TextChanged(txtSource, new EventArgs());
        }

        private void btnSample_Click(object sender, EventArgs e)
        {
            var split = comboBox1.SelectedIndex == 0 ? "\t" : comboBox1.Text;
            txtSource.Text = $"Name{split}姓名\r\nSex{split}性别\r\nAge{split}年龄\r\nMobile{split}手机号\r\nAddress{split}地址";
            txtSource.Refresh();
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
            txtFormatter.Text = ((TextConvertConfig.KeyValue)comboBox2.SelectedItem).Value;
            txtFormatter.Refresh();

            txtSource_TextChanged(null, null);
        }

        private void btnManage_Click(object sender, EventArgs e)
        {
            using (var frm = new frmConfigTextConverter(Hosting))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    var list = TextConvertConfig.LoadConfig(Hosting);
                    comboBox2.Items.Clear();

                    foreach (var k in list)
                    {
                        comboBox2.Items.Add(k);
                    }

                    if (list.Count > 0)
                    {
                        comboBox2.SelectedIndex = 0;
                    }
                }
            }
        }
    }
}
