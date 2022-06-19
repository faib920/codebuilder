namespace CodeBuilder
{
    partial class frmPreBuild
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
            this.treeListColumn1 = new Fireasy.Windows.Forms.TreeListColumn();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSelectPath = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lstPart = new Fireasy.Windows.Forms.TreeList();
            this.treeListColumn2 = new Fireasy.Windows.Forms.TreeListColumn();
            this.SuspendLayout();
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn1.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn1.Formatter = null;
            this.treeListColumn1.Image = null;
            this.treeListColumn1.Text = "部件名称";
            this.treeListColumn1.Validator = null;
            this.treeListColumn1.Width = 200;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(466, 476);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 28);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "确定(&O)";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(552, 476);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 28);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "取消(&C)";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSelectPath
            // 
            this.btnSelectPath.Location = new System.Drawing.Point(367, 476);
            this.btnSelectPath.Name = "btnSelectPath";
            this.btnSelectPath.Size = new System.Drawing.Size(80, 28);
            this.btnSelectPath.TabIndex = 14;
            this.btnSelectPath.Text = "浏览(&B)";
            this.btnSelectPath.UseVisualStyleBackColor = true;
            this.btnSelectPath.Click += new System.EventHandler(this.btnSelectPath_Click);
            // 
            // txtPath
            // 
            this.txtPath.BackColor = System.Drawing.SystemColors.Window;
            this.txtPath.Location = new System.Drawing.Point(91, 477);
            this.txtPath.Name = "txtPath";
            this.txtPath.ReadOnly = true;
            this.txtPath.Size = new System.Drawing.Size(270, 24);
            this.txtPath.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 480);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 19);
            this.label2.TabIndex = 12;
            this.label2.Text = "输出目录:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 19);
            this.label1.TabIndex = 11;
            this.label1.Text = "生成部件:";
            // 
            // lstPart
            // 
            this.lstPart.AlternateBackColor = System.Drawing.Color.Empty;
            this.lstPart.CheckAllChecked = true;
            this.lstPart.Columns.AddRange(new Fireasy.Windows.Forms.TreeListColumn[] {
            this.treeListColumn1,
            this.treeListColumn2});
            this.lstPart.DataSource = null;
            this.lstPart.Footer = null;
            this.lstPart.GroupFont = new System.Drawing.Font("Consolas", 12F);
            this.lstPart.HandCursor = false;
            this.lstPart.Location = new System.Drawing.Point(13, 35);
            this.lstPart.Name = "lstPart";
            this.lstPart.NoneItemText = "没有可显示的数据";
            this.lstPart.RowNumberIndex = 0;
            this.lstPart.ShowCheckAllBoxOnHeader = true;
            this.lstPart.ShowCheckBoxes = true;
            this.lstPart.ShowGridLines = false;
            this.lstPart.Size = new System.Drawing.Size(619, 428);
            this.lstPart.SortKey = null;
            this.lstPart.SortOrder = System.Windows.Forms.SortOrder.None;
            this.lstPart.TabIndex = 10;
            this.lstPart.AfterItemCheckChange += new Fireasy.Windows.Forms.TreeListItemAfterCheckedEventHandler(this.lstPart_AfterItemCheckChange);
            this.lstPart.CheckAllChanged += new Fireasy.Windows.Forms.TreeListCheckAllEventHandler(this.lstPart_CheckAllChanged);
            // 
            // treeListColumn2
            // 
            this.treeListColumn2.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn2.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn2.Formatter = null;
            this.treeListColumn2.Image = null;
            this.treeListColumn2.Text = "输出文件";
            this.treeListColumn2.Validator = null;
            this.treeListColumn2.Width = 340;
            // 
            // frmPreBuild
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(646, 515);
            this.Controls.Add(this.btnSelectPath);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstPart);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPreBuild";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "代码生成配置";
            this.Load += new System.EventHandler(this.frmPreBuild_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Fireasy.Windows.Forms.TreeListColumn treeListColumn1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSelectPath;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private Fireasy.Windows.Forms.TreeList lstPart;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn2;
    }
}