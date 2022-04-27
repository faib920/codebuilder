using System;

namespace CodeBuilder.Core.Forms
{
    public class DockFormBase : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public Action CloseAct { get; set; }

        public DockFormBase()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DockFormBase
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "DockFormBase";
            this.DockStateChanged += new System.EventHandler(this.DockForm_DockStateChanged);
            this.ResumeLayout(false);

        }

        protected override void OnClosed(EventArgs e)
        {
            if (CloseAct != null)
            {
                CloseAct();
            }

            base.OnClosed(e);
        }

        private void DockForm_DockStateChanged(object sender, EventArgs e)
        {
            if (DockState == WeifenLuo.WinFormsUI.Docking.DockState.Float)
            {
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
                this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            }
        }
    }
}
