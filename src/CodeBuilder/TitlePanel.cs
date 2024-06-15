// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace CodeBuilder
{
    public class TitlePanel : Panel
    {
        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var rect = new Rectangle(0, 0, Width, 30);
            using (var brush = new LinearGradientBrush(rect, Color.FromArgb(44, 128, 190), SystemColors.ButtonFace, 0f))
            {
                e.Graphics.FillRectangle(brush, rect);
            }

            rect.X += 10;
            e.Graphics.DrawString(Title, new Font("微软雅黑", 12, FontStyle.Bold), Brushes.White, rect, new StringFormat { LineAlignment = StringAlignment.Center });
        }
    }
}
