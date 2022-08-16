// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CodeBuilder
{
    public partial class frmStart : Form
    {
        private readonly ApplicationContext _applicationContext;
        private frmMain _frmMain;
        private int _pre;

        public frmStart(ApplicationContext applicationContext)
        {
            InitializeComponent();
            _applicationContext = applicationContext;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            _frmMain = new frmMain(p => 
            {
                _pre = p;
                DrawProcessbar();
            });

            _frmMain.OnLoaded = () => this.Close();
            _applicationContext.MainForm = _frmMain;
            _frmMain.Show();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawProcessbar();
        }

        private void DrawProcessbar()
        {
            var width = Width / 100 * _pre;
            using (var b1 = new LinearGradientBrush(new Rectangle(0, 0, width, 3), Color.SteelBlue, Color.Cyan, 0f))
            using (var b2 = new LinearGradientBrush(new Rectangle(0, 0, 4, 3), Color.Cyan, Color.SteelBlue, 0f))
            using (var g = this.CreateGraphics())
            {
                g.FillRectangle(b2, width - 1, Height - 3, 4, 3);
                g.FillRectangle(b1, 0, Height - 3, width, 3);
            }
        }
    }
}
