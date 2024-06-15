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
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CodeBuilder.Tools.Tools
{
    public partial class UnicodeConverter : UserControl, IDevHostingAccessor
    {
        private static Regex reUnicodeChar = new Regex(@"[^\u0000-\u00ff]", RegexOptions.Compiled);
        private static Regex reUnicode = new Regex(@"\\u([0-9a-fA-F]{4})", RegexOptions.Compiled);

        public UnicodeConverter()
        {
            InitializeComponent();
        }

        public IDevHosting Hosting { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                txtEnc.Text = Encode(txtSource.Text);
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
                txtSource.Text = Decode(txtEnc.Text);
            }
            catch (Exception)
            {
                Hosting.ShowError("无法转换!");
            }
        }

        private static string Encode(string s)
        {
            return reUnicodeChar.Replace(s, m => string.Format(@"\u{0:x4}", (short)m.Value[0]));
        }

        private static string Decode(string s)
        {
            return reUnicode.Replace(s, m =>
            {
                short c;
                if (short.TryParse(m.Groups[1].Value, System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture, out c))
                {
                    return "" + (char)c;
                }
                return m.Value;
            });
        }
    }
}
