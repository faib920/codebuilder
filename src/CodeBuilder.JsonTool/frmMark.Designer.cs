
namespace CodeBuilder.JsonTool
{
    partial class frmMark
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new Fireasy.Windows.Forms.ComplexTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.chkClear = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboColor = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "标记内容:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(102, 30);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(199, 24);
            this.textBox1.TabIndex = 1;
            this.textBox1.WaterMarkText = "使用竖线分隔";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(336, 27);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 28);
            this.button1.TabIndex = 2;
            this.button1.Text = "确定(&O)";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(336, 65);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(80, 28);
            this.button2.TabIndex = 3;
            this.button2.Text = "取消(&C)";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // chkClear
            // 
            this.chkClear.AutoSize = true;
            this.chkClear.Checked = true;
            this.chkClear.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkClear.Location = new System.Drawing.Point(102, 106);
            this.chkClear.Name = "chkClear";
            this.chkClear.Size = new System.Drawing.Size(106, 23);
            this.chkClear.TabIndex = 4;
            this.chkClear.Text = "清空现有标记";
            this.chkClear.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 19);
            this.label2.TabIndex = 5;
            this.label2.Text = "标记颜色:";
            // 
            // cboColor
            // 
            this.cboColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboColor.FormattingEnabled = true;
            this.cboColor.Items.AddRange(new object[] {
            "Red",
            "DeepSkyBlue",
            "Cyan",
            "Aquamarine",
            "Lime",
            "GreenYellow",
            "Orange",
            "Gold",
            "Yellow",
            "HotPink"});
            this.cboColor.Location = new System.Drawing.Point(102, 68);
            this.cboColor.Name = "cboColor";
            this.cboColor.Size = new System.Drawing.Size(199, 25);
            this.cboColor.TabIndex = 6;
            this.cboColor.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cboColor_DrawItem);
            // 
            // frmMark
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(448, 143);
            this.Controls.Add(this.cboColor);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkClear);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMark";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "标记";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private Fireasy.Windows.Forms.ComplexTextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox chkClear;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboColor;
    }
}