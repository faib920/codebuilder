namespace CodeBuilder
{
    partial class frmOption
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkCheckUpdate = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cboEncoding = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lstPlugin = new Fireasy.Windows.Forms.TreeList();
            this.treeListColumn1 = new Fireasy.Windows.Forms.TreeListColumn();
            this.treeListColumn2 = new Fireasy.Windows.Forms.TreeListColumn();
            this.treeListColumn3 = new Fireasy.Windows.Forms.TreeListColumn();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkView = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(408, 442);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(400, 416);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "常规";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkCheckUpdate);
            this.groupBox3.Location = new System.Drawing.Point(17, 113);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(364, 90);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "其他";
            // 
            // chkCheckUpdate
            // 
            this.chkCheckUpdate.AutoSize = true;
            this.chkCheckUpdate.Location = new System.Drawing.Point(66, 34);
            this.chkCheckUpdate.Name = "chkCheckUpdate";
            this.chkCheckUpdate.Size = new System.Drawing.Size(120, 16);
            this.chkCheckUpdate.TabIndex = 0;
            this.chkCheckUpdate.Text = "启动时检测新版本";
            this.chkCheckUpdate.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cboEncoding);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(17, 17);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(364, 90);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "文件";
            // 
            // cboEncoding
            // 
            this.cboEncoding.DropDownHeight = 400;
            this.cboEncoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEncoding.FormattingEnabled = true;
            this.cboEncoding.IntegralHeight = false;
            this.cboEncoding.Location = new System.Drawing.Point(66, 34);
            this.cboEncoding.Name = "cboEncoding";
            this.cboEncoding.Size = new System.Drawing.Size(240, 20);
            this.cboEncoding.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "编码";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lstPlugin);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(400, 416);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "插件";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lstPlugin
            // 
            this.lstPlugin.AlternateBackColor = System.Drawing.Color.Empty;
            this.lstPlugin.Columns.AddRange(new Fireasy.Windows.Forms.TreeListColumn[] {
            this.treeListColumn1,
            this.treeListColumn2,
            this.treeListColumn3});
            this.lstPlugin.DataSource = null;
            this.lstPlugin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstPlugin.Footer = null;
            this.lstPlugin.GroupFont = new System.Drawing.Font("Consolas", 12F);
            this.lstPlugin.HandCursor = false;
            this.lstPlugin.Location = new System.Drawing.Point(3, 3);
            this.lstPlugin.Name = "lstPlugin";
            this.lstPlugin.NoneItemText = "没有可显示的数据";
            this.lstPlugin.RowNumberIndex = 0;
            this.lstPlugin.ShowGridLines = false;
            this.lstPlugin.Size = new System.Drawing.Size(394, 410);
            this.lstPlugin.SortKey = null;
            this.lstPlugin.SortOrder = System.Windows.Forms.SortOrder.None;
            this.lstPlugin.TabIndex = 0;
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn1.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn1.Formatter = null;
            this.treeListColumn1.Image = null;
            this.treeListColumn1.Text = "名称";
            this.treeListColumn1.Validator = null;
            this.treeListColumn1.Width = 120;
            // 
            // treeListColumn2
            // 
            this.treeListColumn2.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn2.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn2.Formatter = null;
            this.treeListColumn2.Image = null;
            this.treeListColumn2.Text = "程序集";
            this.treeListColumn2.Validator = null;
            this.treeListColumn2.Width = 200;
            // 
            // treeListColumn3
            // 
            this.treeListColumn3.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn3.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn3.Formatter = null;
            this.treeListColumn3.Image = null;
            this.treeListColumn3.Text = "版本";
            this.treeListColumn3.Validator = null;
            this.treeListColumn3.Width = 120;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(264, 466);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "确定(&O)";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(345, 466);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "取消(&C)";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkView);
            this.groupBox1.Location = new System.Drawing.Point(17, 209);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(364, 90);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "数据源";
            // 
            // chkView
            // 
            this.chkView.AutoSize = true;
            this.chkView.Location = new System.Drawing.Point(66, 34);
            this.chkView.Name = "chkView";
            this.chkView.Size = new System.Drawing.Size(72, 16);
            this.chkView.TabIndex = 0;
            this.chkView.Text = "读取视图";
            this.chkView.UseVisualStyleBackColor = true;
            // 
            // frmOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(432, 501);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.tabControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOption";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "首选项";
            this.Load += new System.EventHandler(this.frmOption_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TabPage tabPage2;
        private Fireasy.Windows.Forms.TreeList lstPlugin;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn1;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cboEncoding;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkCheckUpdate;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkView;
    }
}