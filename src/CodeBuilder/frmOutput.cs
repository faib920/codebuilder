// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using CodeBuilder.Core.Forms;
using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CodeBuilder
{
    public partial class frmOutput : DockFormBase
    {
        internal const string WndName = "Output";

        private readonly DevHosting _hosting;
        private Dictionary<Color, TextStyle> _styles = new Dictionary<Color, TextStyle>
        {
            { Color.Gray, new TextStyle(new SolidBrush(Color.Gray), null, FontStyle.Regular ) }
        };

        public frmOutput(DevHosting hosting)
        {
            InitializeComponent();
            Config.Instance.AddWindow(WndName).Save();

            Icon = Properties.Resources.output;
            _hosting = hosting;

            _hosting.LogPollAct = (type, time, msg) =>
            {
                if (type == 0)
                {
                    WriteMessage(SystemColors.WindowText, msg, time);
                }
                else if (type == 1)
                {
                    WriteMessage(Color.Red, msg, time);
                }
            };
        }

        private void frmOutput_Load(object sender, EventArgs e)
        {
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _hosting.LogPollAct = null;

            Config.Instance.RemoveWindow(WndName).Save();
            base.OnFormClosed(e);
        }

        private void WriteMessage(Color color, string msg, DateTime time)
        {
            Invoke(new Action(() =>
            {
                if (!_styles.TryGetValue(color, out var style))
                {
                    style = new TextStyle(new SolidBrush(color), null, FontStyle.Regular);
                    _styles.Add(color, style);
                }

                richTextBox1.AppendText(time.ToString("HH:mm:ss.fff "), _styles[Color.Gray]);
                richTextBox1.AppendText(msg, style);
                richTextBox1.AppendText(Environment.NewLine);

                richTextBox1.Navigate(richTextBox1.Lines.Count - 1);
            }));
        }

        private void tlbClear_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void tlbCopy_Click(object sender, EventArgs e)
        {
            if (richTextBox1.SelectionLength > 0)
            {
                Clipboard.SetText(richTextBox1.Text);
            }
            else if (richTextBox1.Text.Length > 0)
            {
                Clipboard.SetText(richTextBox1.SelectedText);
            }
        }

        private void tlbHelp_Click(object sender, EventArgs e)
        {
            Process.Start(WebHelper.GetRedirectUrl(_hosting, "/docs/codebuilder-output"));
        }
    }
}
