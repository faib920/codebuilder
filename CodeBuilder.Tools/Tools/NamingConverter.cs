// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CodeBuilder.Tools.Tools
{
    public partial class NamingConverter : UserControl
    {
        public NamingConverter()
        {
            InitializeComponent();
        }

        private void txtSource_TextChanged(object sender, EventArgs e)
        {
            var camelChars = new List<char>();
            var hungaryChars = new List<char>();
            var pascalChars = new List<char>();

            var isUnderline = false;

            foreach (var c in txtSource.Text)
            {
                if (c == 95)
                {
                    isUnderline = true;
                    continue;
                }

                //驼峰
                if (camelChars.Count == 0 && c >= 65 && c <= 90) //首字母大写，转成小写
                {
                    camelChars.Add((char)(c + 32));
                }
                else if (isUnderline)
                {
                    if (c >= 97 && c <= 122)//小写转大写
                    {
                        camelChars.Add((char)(c - 32));
                    }
                    else
                    {
                        camelChars.Add(c);
                    }
                }
                else
                {
                    camelChars.Add(c);
                }

                //匈牙利
                if ((hungaryChars.Count > 0 && c >= 65 && c <= 90) || isUnderline)
                {
                    hungaryChars.Add('_');
                }

                if (c >= 65 && c <= 90)
                {
                    hungaryChars.Add((char)(c + 32));
                }
                else
                {
                    hungaryChars.Add(c);
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

            txtCamel.Text = string.Format(txtFormatter.Text, new string(camelChars.ToArray()));
            txtHungary.Text = string.Format(txtFormatter.Text, new string(hungaryChars.ToArray()));
            txtPascal.Text = string.Format(txtFormatter.Text, new string(pascalChars.ToArray()));
        }

        private void txtSource_Click(object sender, EventArgs e)
        {
            txtSource.SelectAll();
        }

        private void txtCamel_Click(object sender, EventArgs e)
        {
            txtCamel.SelectAll();
        }

        private void txtHungary_Click(object sender, EventArgs e)
        {
            txtHungary.SelectAll();
        }

        private void txtPascal_Click(object sender, EventArgs e)
        {
            txtPascal.SelectAll();
        }

        private void ck1_CheckedChanged(object sender, EventArgs e)
        {
            txtCamel.Enabled = !ck1.Checked;
        }

        private void ck2_CheckedChanged(object sender, EventArgs e)
        {
            txtHungary.Enabled = !ck2.Checked;
        }

        private void ck3_CheckedChanged(object sender, EventArgs e)
        {
            txtPascal.Enabled = !ck3.Checked;
        }

        private void ck0_CheckedChanged(object sender, EventArgs e)
        {
            txtFormatter.Enabled = !ck0.Checked;
        }
    }
}
