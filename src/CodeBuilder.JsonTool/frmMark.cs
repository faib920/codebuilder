// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Forms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CodeBuilder.JsonTool
{
    public partial class frmMark : FormBase
    {
        public frmMark()
        {
            InitializeComponent();
        }

        public string MarkText
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        public bool ClearMarks => chkClear.Checked;

        public Color? MarkColor { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cboColor.SelectedIndex > -1)
            {
                MarkColor = Color.FromName(cboColor.SelectedItem.ToString());
            }

            DialogResult = DialogResult.OK;
        }

        private void cboColor_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1)
            {
                return;
            }

            var colorName = cboColor.Items[e.Index].ToString();
            var color = Color.FromName(colorName);
            e.Graphics.FillRectangle(new SolidBrush(color), e.Bounds);
            e.Graphics.DrawString(colorName, cboColor.Font, SystemBrushes.WindowText, e.Bounds, new StringFormat { LineAlignment = StringAlignment.Center });
        }
    }
}
