
namespace CodeBuilder.Tools.Tools
{
    partial class NamingConverter
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
            this.ck0 = new System.Windows.Forms.CheckBox();
            this.ck3 = new System.Windows.Forms.CheckBox();
            this.ck2 = new System.Windows.Forms.CheckBox();
            this.ck1 = new System.Windows.Forms.CheckBox();
            this.txtPascal = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtHungary = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCamel = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtFormatter = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ck0
            // 
            this.ck0.AutoSize = true;
            this.ck0.Location = new System.Drawing.Point(499, 49);
            this.ck0.Name = "ck0";
            this.ck0.Size = new System.Drawing.Size(48, 16);
            this.ck0.TabIndex = 13;
            this.ck0.Text = "锁定";
            this.ck0.UseVisualStyleBackColor = true;
            this.ck0.CheckedChanged += new System.EventHandler(this.ck0_CheckedChanged);
            // 
            // ck3
            // 
            this.ck3.AutoSize = true;
            this.ck3.Location = new System.Drawing.Point(499, 159);
            this.ck3.Name = "ck3";
            this.ck3.Size = new System.Drawing.Size(48, 16);
            this.ck3.TabIndex = 12;
            this.ck3.Text = "锁定";
            this.ck3.UseVisualStyleBackColor = true;
            this.ck3.CheckedChanged += new System.EventHandler(this.ck3_CheckedChanged);
            // 
            // ck2
            // 
            this.ck2.AutoSize = true;
            this.ck2.Location = new System.Drawing.Point(499, 123);
            this.ck2.Name = "ck2";
            this.ck2.Size = new System.Drawing.Size(48, 16);
            this.ck2.TabIndex = 11;
            this.ck2.Text = "锁定";
            this.ck2.UseVisualStyleBackColor = true;
            this.ck2.CheckedChanged += new System.EventHandler(this.ck2_CheckedChanged);
            // 
            // ck1
            // 
            this.ck1.AutoSize = true;
            this.ck1.Location = new System.Drawing.Point(499, 86);
            this.ck1.Name = "ck1";
            this.ck1.Size = new System.Drawing.Size(48, 16);
            this.ck1.TabIndex = 10;
            this.ck1.Text = "锁定";
            this.ck1.UseVisualStyleBackColor = true;
            this.ck1.CheckedChanged += new System.EventHandler(this.ck1_CheckedChanged);
            // 
            // txtPascal
            // 
            this.txtPascal.Location = new System.Drawing.Point(89, 158);
            this.txtPascal.Name = "txtPascal";
            this.txtPascal.Size = new System.Drawing.Size(398, 21);
            this.txtPascal.TabIndex = 9;
            this.txtPascal.Click += new System.EventHandler(this.txtPascal_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 161);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "帕斯卡命名";
            // 
            // txtHungary
            // 
            this.txtHungary.Location = new System.Drawing.Point(89, 121);
            this.txtHungary.Name = "txtHungary";
            this.txtHungary.Size = new System.Drawing.Size(398, 21);
            this.txtHungary.TabIndex = 7;
            this.txtHungary.Click += new System.EventHandler(this.txtHungary_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "匈牙利命名";
            // 
            // txtCamel
            // 
            this.txtCamel.Location = new System.Drawing.Point(89, 84);
            this.txtCamel.Name = "txtCamel";
            this.txtCamel.Size = new System.Drawing.Size(398, 21);
            this.txtCamel.TabIndex = 5;
            this.txtCamel.Click += new System.EventHandler(this.txtCamel_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "驼峰命名";
            // 
            // txtFormatter
            // 
            this.txtFormatter.Location = new System.Drawing.Point(89, 47);
            this.txtFormatter.Name = "txtFormatter";
            this.txtFormatter.Size = new System.Drawing.Size(398, 21);
            this.txtFormatter.TabIndex = 3;
            this.txtFormatter.Text = "{0}";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "转换的模板";
            // 
            // txtSource
            // 
            this.txtSource.Location = new System.Drawing.Point(89, 10);
            this.txtSource.Name = "txtSource";
            this.txtSource.Size = new System.Drawing.Size(398, 21);
            this.txtSource.TabIndex = 1;
            this.txtSource.Click += new System.EventHandler(this.txtSource_Click);
            this.txtSource.TextChanged += new System.EventHandler(this.txtSource_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "待转换文本";
            // 
            // NamingConverter
            // 
            this.Controls.Add(this.ck0);
            this.Controls.Add(this.ck3);
            this.Controls.Add(this.ck2);
            this.Controls.Add(this.ck1);
            this.Controls.Add(this.txtPascal);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtHungary);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtCamel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtFormatter);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSource);
            this.Controls.Add(this.label1);
            this.Name = "NamingConverter";
            this.Size = new System.Drawing.Size(563, 197);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox ck0;
        private System.Windows.Forms.CheckBox ck3;
        private System.Windows.Forms.CheckBox ck2;
        private System.Windows.Forms.CheckBox ck1;
        private System.Windows.Forms.TextBox txtPascal;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtHungary;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCamel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtFormatter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.Label label1;
    }
}
