namespace CodeBuilder
{
    partial class frmTemplateCommit
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
            this.components = new System.ComponentModel.Container();
            this.txtVer = new Fireasy.Windows.Forms.NumericTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtRemark = new Fireasy.Windows.Forms.ComplexTextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.errorProvider1 = new Fireasy.Windows.Forms.ErrorProvider(this.components);
            this.SuspendLayout();
            // 
            // txtVer
            // 
            this.txtVer.AllowNegative = true;
            this.txtVer.AllowNullable = false;
            this.txtVer.Decimal = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.txtVer.DefaultValue = 1D;
            this.txtVer.DigitsInGroup = 0;
            this.txtVer.Flags = 0;
            this.txtVer.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtVer.Location = new System.Drawing.Point(64, 21);
            this.txtVer.MaxDecimalPlaces = 2;
            this.txtVer.MaxWholeDigits = 9;
            this.txtVer.Name = "txtVer";
            this.txtVer.Prefix = "";
            this.txtVer.RangeMax = 100.99D;
            this.txtVer.RangeMin = 1D;
            this.txtVer.Size = new System.Drawing.Size(121, 24);
            this.txtVer.TabIndex = 21;
            this.txtVer.Text = "1.0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 19);
            this.label3.TabIndex = 22;
            this.label3.Text = "版本:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 19);
            this.label1.TabIndex = 23;
            this.label1.Text = "内容:";
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(64, 58);
            this.txtRemark.Multiline = true;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.txtRemark.Size = new System.Drawing.Size(309, 94);
            this.txtRemark.TabIndex = 24;
            this.txtRemark.WaterMarkText = "填写此次修改的内容说明";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(293, 173);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 28);
            this.btnCancel.TabIndex = 26;
            this.btnCancel.Text = "取消(&C)";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(207, 173);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 28);
            this.btnOk.TabIndex = 25;
            this.btnOk.Text = "确定(&O)";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // frmTemplateShare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.ClientSize = new System.Drawing.Size(393, 216);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtRemark);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtVer);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTemplateShare";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "提交新版本";
            this.Load += new System.EventHandler(this.frmTemplateShare_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Fireasy.Windows.Forms.NumericTextBox txtVer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private Fireasy.Windows.Forms.ComplexTextBox txtRemark;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private Fireasy.Windows.Forms.ErrorProvider errorProvider1;
    }
}