namespace CodeBuilder.Core.Designer
{
    partial class ForeignKeyEditorForm
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
            this.lstObject = new Fireasy.Windows.Forms.TreeList();
            this.treeListColumn1 = new Fireasy.Windows.Forms.TreeListColumn();
            this.treeListColumn2 = new Fireasy.Windows.Forms.TreeListColumn();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnBind = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblLocCount = new System.Windows.Forms.Label();
            this.btnLocation = new System.Windows.Forms.Button();
            this.txtKeyword = new Fireasy.Windows.Forms.ComplexTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.chkPrimaryKey = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lstObject
            // 
            this.lstObject.AllowUpdateDataItem = false;
            this.lstObject.AlternateBackColor = System.Drawing.Color.Empty;
            this.lstObject.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstObject.CheckAllChecked = false;
            this.lstObject.Columns.AddRange(new Fireasy.Windows.Forms.TreeListColumn[] {
            this.treeListColumn1,
            this.treeListColumn2});
            this.lstObject.DataSource = null;
            this.lstObject.Footer = null;
            this.lstObject.FooterHeight = 28;
            this.lstObject.GroupFont = new System.Drawing.Font("Consolas", 12F);
            this.lstObject.HandCursor = false;
            this.lstObject.HeaderHeight = 28;
            this.lstObject.ItemHeight = 28;
            this.lstObject.Location = new System.Drawing.Point(12, 44);
            this.lstObject.Name = "lstObject";
            this.lstObject.NoneItemImage = null;
            this.lstObject.NoneItemText = "没有可显示的数据";
            this.lstObject.RowNumberIndex = 0;
            this.lstObject.ShowAlternateBackColor = true;
            this.lstObject.ShowPlusMinus = true;
            this.lstObject.ShowPlusMinusLines = false;
            this.lstObject.Size = new System.Drawing.Size(636, 410);
            this.lstObject.SortKey = null;
            this.lstObject.SortOrder = System.Windows.Forms.SortOrder.None;
            this.lstObject.TabIndex = 0;
            this.lstObject.DemandLoad += new Fireasy.Windows.Forms.TreeListDemandLoadEventHandler(this.listBox1_DemandLoad);
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn1.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn1.Formatter = null;
            this.treeListColumn1.Image = null;
            this.treeListColumn1.Text = "名称";
            this.treeListColumn1.Validator = null;
            this.treeListColumn1.Width = 300;
            // 
            // treeListColumn2
            // 
            this.treeListColumn2.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn2.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn2.Formatter = null;
            this.treeListColumn2.Image = null;
            this.treeListColumn2.Text = "备注";
            this.treeListColumn2.Validator = null;
            this.treeListColumn2.Width = 300;
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.Location = new System.Drawing.Point(396, 463);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(80, 28);
            this.btnRemove.TabIndex = 1;
            this.btnRemove.Text = "解除(&U)";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnBind
            // 
            this.btnBind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBind.Location = new System.Drawing.Point(482, 463);
            this.btnBind.Name = "btnBind";
            this.btnBind.Size = new System.Drawing.Size(80, 28);
            this.btnBind.TabIndex = 2;
            this.btnBind.Text = "绑定(&B)";
            this.btnBind.UseVisualStyleBackColor = true;
            this.btnBind.Click += new System.EventHandler(this.btnBind_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(568, 463);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 28);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消(&C)";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblLocCount
            // 
            this.lblLocCount.AutoSize = true;
            this.lblLocCount.Location = new System.Drawing.Point(469, 15);
            this.lblLocCount.Name = "lblLocCount";
            this.lblLocCount.Size = new System.Drawing.Size(0, 19);
            this.lblLocCount.TabIndex = 19;
            // 
            // btnLocation
            // 
            this.btnLocation.Location = new System.Drawing.Point(406, 11);
            this.btnLocation.Name = "btnLocation";
            this.btnLocation.Size = new System.Drawing.Size(60, 27);
            this.btnLocation.TabIndex = 18;
            this.btnLocation.Text = "下一个";
            this.btnLocation.UseVisualStyleBackColor = true;
            this.btnLocation.Click += new System.EventHandler(this.btnLocation_Click);
            // 
            // txtKeyword
            // 
            this.txtKeyword.Location = new System.Drawing.Point(57, 12);
            this.txtKeyword.Name = "txtKeyword";
            this.txtKeyword.Size = new System.Drawing.Size(342, 24);
            this.txtKeyword.TabIndex = 16;
            this.txtKeyword.WaterMarkText = "输入关键字或正则表达式，停留1秒或回车后定位";
            this.txtKeyword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtKeyword_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 19);
            this.label1.TabIndex = 17;
            this.label1.Text = "查找:";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // chkPrimaryKey
            // 
            this.chkPrimaryKey.AutoSize = true;
            this.chkPrimaryKey.Location = new System.Drawing.Point(12, 467);
            this.chkPrimaryKey.Name = "chkPrimaryKey";
            this.chkPrimaryKey.Size = new System.Drawing.Size(93, 23);
            this.chkPrimaryKey.TabIndex = 20;
            this.chkPrimaryKey.Text = "只列出主键";
            this.chkPrimaryKey.UseVisualStyleBackColor = true;
            this.chkPrimaryKey.CheckedChanged += new System.EventHandler(this.chkPrimaryKey_CheckedChanged);
            // 
            // ForeignKeyEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(660, 500);
            this.Controls.Add(this.chkPrimaryKey);
            this.Controls.Add(this.lblLocCount);
            this.Controls.Add(this.btnLocation);
            this.Controls.Add(this.txtKeyword);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnBind);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.lstObject);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(572, 394);
            this.Name = "ForeignKeyEditorForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "外键关联";
            this.Load += new System.EventHandler(this.ForeignKeyEditorForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Fireasy.Windows.Forms.TreeList lstObject;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn1;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn2;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnBind;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblLocCount;
        private System.Windows.Forms.Button btnLocation;
        private Fireasy.Windows.Forms.ComplexTextBox txtKeyword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox chkPrimaryKey;
    }
}