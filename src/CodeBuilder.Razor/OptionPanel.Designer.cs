namespace CodeBuilder.Razor
{
    partial class OptionPanel
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnAssembly = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnAssembly
            // 
            this.btnAssembly.Location = new System.Drawing.Point(6, 29);
            this.btnAssembly.Name = "btnAssembly";
            this.btnAssembly.Size = new System.Drawing.Size(90, 28);
            this.btnAssembly.TabIndex = 0;
            this.btnAssembly.Text = "管理程序集";
            this.btnAssembly.UseVisualStyleBackColor = true;
            this.btnAssembly.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(179, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "在 RazorEngine 中引用入程序集";
            // 
            // OptionPanel
            // 
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnAssembly);
            this.Name = "OptionPanel";
            this.Size = new System.Drawing.Size(348, 353);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAssembly;
        private System.Windows.Forms.Label label1;
    }
}
