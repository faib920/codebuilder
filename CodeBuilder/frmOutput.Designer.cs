namespace CodeBuilder
{
    partial class frmOutput
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tlbClear = new System.Windows.Forms.ToolStripButton();
            this.tlbCopy = new System.Windows.Forms.ToolStripButton();
            this.tlbHelp = new System.Windows.Forms.ToolStripButton();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlbClear,
            this.tlbCopy,
            this.tlbHelp});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(675, 27);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tlbClear
            // 
            this.tlbClear.Image = global::CodeBuilder.Properties.Resources.clear;
            this.tlbClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbClear.Name = "tlbClear";
            this.tlbClear.Size = new System.Drawing.Size(57, 24);
            this.tlbClear.Text = "清空";
            this.tlbClear.Click += new System.EventHandler(this.tlbClear_Click);
            // 
            // tlbCopy
            // 
            this.tlbCopy.Image = global::CodeBuilder.Properties.Resources.copy;
            this.tlbCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbCopy.Name = "tlbCopy";
            this.tlbCopy.Size = new System.Drawing.Size(57, 24);
            this.tlbCopy.Text = "复制";
            this.tlbCopy.Click += new System.EventHandler(this.tlbCopy_Click);
            // 
            // tlbHelp
            // 
            this.tlbHelp.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tlbHelp.Image = global::CodeBuilder.Properties.Resources.help;
            this.tlbHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbHelp.Name = "tlbHelp";
            this.tlbHelp.Size = new System.Drawing.Size(57, 24);
            this.tlbHelp.Text = "帮助";
            this.tlbHelp.Click += new System.EventHandler(this.tlbHelp_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(0, 27);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(675, 62);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // frmOutput
            // 
            this.ClientSize = new System.Drawing.Size(675, 89);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HideOnClose = true;
            this.Name = "frmOutput";
            this.Text = "输出";
            this.Load += new System.EventHandler(this.frmOutput_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tlbClear;
        private System.Windows.Forms.ToolStripButton tlbCopy;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ToolStripButton tlbHelp;
    }
}