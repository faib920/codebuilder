// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CodeBuilder
{
    public partial class frmOutput : DockForm
    {
        private readonly DevHosting _hosting;

        public frmOutput(DevHosting hosting)
        {
            InitializeComponent();
            Icon = Properties.Resources.output;
            _hosting = hosting;
        }

        private void frmOutput_Load(object sender, EventArgs e)
        {
            _hosting.ConsoleInfoAct = s => WriteMessage(SystemColors.WindowText, s);
            _hosting.ConsoleErrorAct = s => WriteMessage(Color.Red, s);
        }

        private void WriteMessage(Color color, string msg)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.SelectionColor = Color.Gray;
            richTextBox1.SelectionFont = new Font("Consolas", 9);
            richTextBox1.SelectedText = DateTime.Now.ToString("mm:ss.fff ");
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.SelectionFont = richTextBox1.Font;
            richTextBox1.SelectionColor = color;
            richTextBox1.SelectedText = msg + Environment.NewLine;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
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
    }
}
