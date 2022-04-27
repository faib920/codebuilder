using System.Windows.Forms;

namespace CodeBuilder.Core.Forms
{
    public class FormBase : Form
    {
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FormBase
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "FormBase";
            this.ResumeLayout(false);

        }

        public FormBase()
        {
            InitializeComponent();
        }
    }
}
