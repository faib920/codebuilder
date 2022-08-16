
namespace CodeBuilder.Tools.Tools
{
    partial class CollComparer
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
            this.txtSource1 = new System.Windows.Forms.RichTextBox();
            this.txtSource2 = new System.Windows.Forms.RichTextBox();
            this.txtDiff1 = new System.Windows.Forms.RichTextBox();
            this.txtDiff2 = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtSource1
            // 
            this.txtSource1.Location = new System.Drawing.Point(3, 30);
            this.txtSource1.Name = "txtSource1";
            this.txtSource1.Size = new System.Drawing.Size(279, 265);
            this.txtSource1.TabIndex = 0;
            this.txtSource1.Text = "";
            this.txtSource1.TextChanged += new System.EventHandler(this.txtSource1_TextChanged);
            // 
            // txtSource2
            // 
            this.txtSource2.Location = new System.Drawing.Point(299, 30);
            this.txtSource2.Name = "txtSource2";
            this.txtSource2.Size = new System.Drawing.Size(279, 265);
            this.txtSource2.TabIndex = 1;
            this.txtSource2.Text = "";
            this.txtSource2.TextChanged += new System.EventHandler(this.txtSource2_TextChanged);
            // 
            // txtDiff1
            // 
            this.txtDiff1.Location = new System.Drawing.Point(3, 329);
            this.txtDiff1.Name = "txtDiff1";
            this.txtDiff1.Size = new System.Drawing.Size(279, 257);
            this.txtDiff1.TabIndex = 2;
            this.txtDiff1.Text = "";
            // 
            // txtDiff2
            // 
            this.txtDiff2.Location = new System.Drawing.Point(299, 329);
            this.txtDiff2.Name = "txtDiff2";
            this.txtDiff2.Size = new System.Drawing.Size(279, 257);
            this.txtDiff2.TabIndex = 3;
            this.txtDiff2.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(588, 30);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 28);
            this.button1.TabIndex = 4;
            this.button1.Text = "比较(&C)";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "集合1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(297, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "集合2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(297, 307);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "在集合1中不存在";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 307);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "在集合2中不存在";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(588, 68);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(80, 28);
            this.button2.TabIndex = 9;
            this.button2.Text = "示例(&I)";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // CollComparer
            // 
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtDiff2);
            this.Controls.Add(this.txtDiff1);
            this.Controls.Add(this.txtSource2);
            this.Controls.Add(this.txtSource1);
            this.Name = "CollComparer";
            this.Size = new System.Drawing.Size(777, 643);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtSource1;
        private System.Windows.Forms.RichTextBox txtSource2;
        private System.Windows.Forms.RichTextBox txtDiff1;
        private System.Windows.Forms.RichTextBox txtDiff2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button2;
    }
}
