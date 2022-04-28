
namespace CodeBuilder.JsonTool
{
    partial class frmJsonFormatter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmJsonFormatter));
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.treeListColumn1 = new Fireasy.Windows.Forms.TreeListColumn();
            this.treeListColumn2 = new Fireasy.Windows.Forms.TreeListColumn();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuFind = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMark = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tRed = new System.Windows.Forms.ToolStripMenuItem();
            this.tDsBlue = new System.Windows.Forms.ToolStripMenuItem();
            this.tCyan = new System.Windows.Forms.ToolStripMenuItem();
            this.tAquamarine = new System.Windows.Forms.ToolStripMenuItem();
            this.tLime = new System.Windows.Forms.ToolStripMenuItem();
            this.tGreenYellow = new System.Windows.Forms.ToolStripMenuItem();
            this.tOrange = new System.Windows.Forms.ToolStripMenuItem();
            this.tGold = new System.Windows.Forms.ToolStripMenuItem();
            this.tYellow = new System.Windows.Forms.ToolStripMenuItem();
            this.tHotPink = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuClearMark = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCopyKey = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.txtSource = new System.Windows.Forms.RichTextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuFromWeb = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHistory = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuView = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.treeList1 = new Fireasy.Windows.Forms.TreeList();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.plnStatus = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.TextBox();
            this.plnFind = new System.Windows.Forms.Panel();
            this.btnTreeClose = new System.Windows.Forms.Button();
            this.btnTreeFind = new System.Windows.Forms.Button();
            this.txtTreeKeyword = new Fireasy.Windows.Forms.ComplexTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtResult = new ICSharpCode.TextEditor.TextEditorControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.mnuExpand = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCollapse = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.contextMenuStrip2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.plnStatus.SuspendLayout();
            this.plnFind.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 166);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(827, 3);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn1.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn1.Formatter = null;
            this.treeListColumn1.Image = null;
            this.treeListColumn1.Text = "Key";
            this.treeListColumn1.Validator = null;
            this.treeListColumn1.Width = 200;
            // 
            // treeListColumn2
            // 
            this.treeListColumn2.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn2.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn2.Formatter = null;
            this.treeListColumn2.Image = null;
            this.treeListColumn2.Text = "Value";
            this.treeListColumn2.Validator = null;
            this.treeListColumn2.Width = 300;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFind,
            this.mnuMark,
            this.toolStripMenuItem1,
            this.mnuCopyKey,
            this.mnuCopy,
            this.toolStripSeparator1,
            this.mnuExpand,
            this.mnuCollapse});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(227, 178);
            // 
            // mnuFind
            // 
            this.mnuFind.Name = "mnuFind";
            this.mnuFind.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.mnuFind.Size = new System.Drawing.Size(226, 24);
            this.mnuFind.Text = "查找...";
            this.mnuFind.Click += new System.EventHandler(this.mnuFind_Click);
            // 
            // mnuMark
            // 
            this.mnuMark.Name = "mnuMark";
            this.mnuMark.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.mnuMark.Size = new System.Drawing.Size(226, 24);
            this.mnuMark.Text = "查找并标记...";
            this.mnuMark.Click += new System.EventHandler(this.mnuMark_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tRed,
            this.tDsBlue,
            this.tCyan,
            this.tAquamarine,
            this.tLime,
            this.tGreenYellow,
            this.tOrange,
            this.tGold,
            this.tYellow,
            this.tHotPink,
            this.mnuClearMark});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(180, 24);
            this.toolStripMenuItem1.Text = "使用颜色标记";
            // 
            // tRed
            // 
            this.tRed.BackColor = System.Drawing.Color.Red;
            this.tRed.Name = "tRed";
            this.tRed.Size = new System.Drawing.Size(173, 24);
            this.tRed.Text = "Red";
            this.tRed.Click += new System.EventHandler(this.mnuColor_Click);
            // 
            // tDsBlue
            // 
            this.tDsBlue.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.tDsBlue.Name = "tDsBlue";
            this.tDsBlue.Size = new System.Drawing.Size(173, 24);
            this.tDsBlue.Text = "DeepSkyBlue";
            this.tDsBlue.Click += new System.EventHandler(this.mnuColor_Click);
            // 
            // tCyan
            // 
            this.tCyan.BackColor = System.Drawing.Color.Cyan;
            this.tCyan.Name = "tCyan";
            this.tCyan.Size = new System.Drawing.Size(173, 24);
            this.tCyan.Text = "Cyan";
            this.tCyan.Click += new System.EventHandler(this.mnuColor_Click);
            // 
            // tAquamarine
            // 
            this.tAquamarine.BackColor = System.Drawing.Color.Aquamarine;
            this.tAquamarine.Name = "tAquamarine";
            this.tAquamarine.Size = new System.Drawing.Size(173, 24);
            this.tAquamarine.Text = "Aquamarine";
            this.tAquamarine.Click += new System.EventHandler(this.mnuColor_Click);
            // 
            // tLime
            // 
            this.tLime.BackColor = System.Drawing.Color.Lime;
            this.tLime.Name = "tLime";
            this.tLime.Size = new System.Drawing.Size(173, 24);
            this.tLime.Text = "Lime";
            this.tLime.Click += new System.EventHandler(this.mnuColor_Click);
            // 
            // tGreenYellow
            // 
            this.tGreenYellow.BackColor = System.Drawing.Color.GreenYellow;
            this.tGreenYellow.Name = "tGreenYellow";
            this.tGreenYellow.Size = new System.Drawing.Size(173, 24);
            this.tGreenYellow.Text = "GreenYellow";
            this.tGreenYellow.Click += new System.EventHandler(this.mnuColor_Click);
            // 
            // tOrange
            // 
            this.tOrange.BackColor = System.Drawing.Color.Orange;
            this.tOrange.Name = "tOrange";
            this.tOrange.Size = new System.Drawing.Size(173, 24);
            this.tOrange.Text = "Orange";
            this.tOrange.Click += new System.EventHandler(this.mnuColor_Click);
            // 
            // tGold
            // 
            this.tGold.BackColor = System.Drawing.Color.Gold;
            this.tGold.Name = "tGold";
            this.tGold.Size = new System.Drawing.Size(173, 24);
            this.tGold.Text = "Gold";
            this.tGold.Click += new System.EventHandler(this.mnuColor_Click);
            // 
            // tYellow
            // 
            this.tYellow.BackColor = System.Drawing.Color.Yellow;
            this.tYellow.Name = "tYellow";
            this.tYellow.Size = new System.Drawing.Size(173, 24);
            this.tYellow.Text = "Yellow";
            this.tYellow.Click += new System.EventHandler(this.mnuColor_Click);
            // 
            // tHotPink
            // 
            this.tHotPink.BackColor = System.Drawing.Color.HotPink;
            this.tHotPink.Name = "tHotPink";
            this.tHotPink.Size = new System.Drawing.Size(173, 24);
            this.tHotPink.Text = "HotPink";
            this.tHotPink.Click += new System.EventHandler(this.mnuColor_Click);
            // 
            // mnuClearMark
            // 
            this.mnuClearMark.Name = "mnuClearMark";
            this.mnuClearMark.Size = new System.Drawing.Size(173, 24);
            this.mnuClearMark.Text = "取消标记";
            this.mnuClearMark.Click += new System.EventHandler(this.mnuClearMark_Click);
            // 
            // mnuCopyKey
            // 
            this.mnuCopyKey.Name = "mnuCopyKey";
            this.mnuCopyKey.Size = new System.Drawing.Size(180, 24);
            this.mnuCopyKey.Text = "复制 Key";
            this.mnuCopyKey.Click += new System.EventHandler(this.mnuCopyKey_Click);
            // 
            // mnuCopy
            // 
            this.mnuCopy.Name = "mnuCopy";
            this.mnuCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.mnuCopy.Size = new System.Drawing.Size(226, 24);
            this.mnuCopy.Text = "复制 Value";
            this.mnuCopy.Click += new System.EventHandler(this.mnuCopy_Click);
            // 
            // txtSource
            // 
            this.txtSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSource.ContextMenuStrip = this.contextMenuStrip1;
            this.txtSource.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSource.Location = new System.Drawing.Point(0, 0);
            this.txtSource.Name = "txtSource";
            this.txtSource.Size = new System.Drawing.Size(827, 166);
            this.txtSource.TabIndex = 0;
            this.txtSource.Text = "";
            this.txtSource.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSource_KeyDown);
            this.txtSource.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSource_KeyUp);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFromWeb,
            this.mnuHistory,
            this.mnuView});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(211, 98);
            // 
            // mnuFromWeb
            // 
            this.mnuFromWeb.Name = "mnuFromWeb";
            this.mnuFromWeb.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.mnuFromWeb.Size = new System.Drawing.Size(210, 24);
            this.mnuFromWeb.Text = "载入网址";
            this.mnuFromWeb.Click += new System.EventHandler(this.mnuFromWeb_Click);
            // 
            // mnuHistory
            // 
            this.mnuHistory.Name = "mnuHistory";
            this.mnuHistory.Size = new System.Drawing.Size(153, 24);
            this.mnuHistory.Text = "载入历史";
            // 
            // mnuView
            // 
            this.mnuView.Name = "mnuView";
            this.mnuView.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.mnuView.Size = new System.Drawing.Size(210, 24);
            this.mnuView.Text = "预览和美化";
            this.mnuView.Click += new System.EventHandler(this.mnuView_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 169);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(827, 476);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.treeList1);
            this.tabPage1.Controls.Add(this.plnStatus);
            this.tabPage1.Controls.Add(this.plnFind);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(819, 450);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Json 视图";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // treeList1
            // 
            this.treeList1.AlternateBackColor = System.Drawing.Color.Empty;
            this.treeList1.Columns.AddRange(new Fireasy.Windows.Forms.TreeListColumn[] {
            this.treeListColumn1,
            this.treeListColumn2});
            this.treeList1.ContextMenuStrip = this.contextMenuStrip2;
            this.treeList1.DataSource = null;
            this.treeList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeList1.Footer = null;
            this.treeList1.GroupFont = new System.Drawing.Font("Consolas", 12F);
            this.treeList1.HandCursor = false;
            this.treeList1.ImageList = this.imageList1;
            this.treeList1.Location = new System.Drawing.Point(3, 3);
            this.treeList1.Name = "treeList1";
            this.treeList1.NoneItemText = "没有可显示的数据";
            this.treeList1.RowNumberIndex = 0;
            this.treeList1.ShowGridLines = false;
            this.treeList1.ShowPlusMinus = true;
            this.treeList1.Size = new System.Drawing.Size(813, 377);
            this.treeList1.SortKey = null;
            this.treeList1.SortOrder = System.Windows.Forms.SortOrder.None;
            this.treeList1.TabIndex = 1;
            this.treeList1.TabStop = false;
            this.treeList1.ItemClick += new Fireasy.Windows.Forms.TreeListItemClickEventHandler(this.treeList1_ItemClick);
            this.treeList1.ItemSelectionChanged += new Fireasy.Windows.Forms.TreeListItemSelectionChangedEventHandler(this.treeList1_ItemSelectionChanged);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "n1.png");
            this.imageList1.Images.SetKeyName(1, "n2.png");
            this.imageList1.Images.SetKeyName(2, "n3.png");
            // 
            // plnStatus
            // 
            this.plnStatus.Controls.Add(this.lblStatus);
            this.plnStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.plnStatus.Location = new System.Drawing.Point(3, 380);
            this.plnStatus.Name = "plnStatus";
            this.plnStatus.Size = new System.Drawing.Size(813, 27);
            this.plnStatus.TabIndex = 7;
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.SystemColors.Window;
            this.lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblStatus.Location = new System.Drawing.Point(3, 6);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.ReadOnly = true;
            this.lblStatus.Size = new System.Drawing.Size(475, 14);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "未选中内容";
            // 
            // plnFind
            // 
            this.plnFind.Controls.Add(this.btnTreeClose);
            this.plnFind.Controls.Add(this.btnTreeFind);
            this.plnFind.Controls.Add(this.txtTreeKeyword);
            this.plnFind.Controls.Add(this.label1);
            this.plnFind.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.plnFind.Location = new System.Drawing.Point(3, 407);
            this.plnFind.Name = "plnFind";
            this.plnFind.Size = new System.Drawing.Size(813, 40);
            this.plnFind.TabIndex = 0;
            this.plnFind.Visible = false;
            // 
            // btnTreeClose
            // 
            this.btnTreeClose.Location = new System.Drawing.Point(380, 6);
            this.btnTreeClose.Name = "btnTreeClose";
            this.btnTreeClose.Size = new System.Drawing.Size(92, 28);
            this.btnTreeClose.TabIndex = 6;
            this.btnTreeClose.Text = "关闭(&X)";
            this.btnTreeClose.UseVisualStyleBackColor = true;
            this.btnTreeClose.Click += new System.EventHandler(this.btnTreeClose_Click);
            // 
            // btnTreeFind
            // 
            this.btnTreeFind.Location = new System.Drawing.Point(282, 6);
            this.btnTreeFind.Name = "btnTreeFind";
            this.btnTreeFind.Size = new System.Drawing.Size(92, 28);
            this.btnTreeFind.TabIndex = 4;
            this.btnTreeFind.Text = "查找(&F)";
            this.btnTreeFind.UseVisualStyleBackColor = true;
            this.btnTreeFind.Click += new System.EventHandler(this.btnTreeFind_Click);
            // 
            // txtTreeKeyword
            // 
            this.txtTreeKeyword.Location = new System.Drawing.Point(62, 8);
            this.txtTreeKeyword.Name = "txtTreeKeyword";
            this.txtTreeKeyword.Size = new System.Drawing.Size(207, 21);
            this.txtTreeKeyword.TabIndex = 3;
            this.txtTreeKeyword.WaterMarkText = "可使用 Key==Value 进行查找";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "关键字:";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txtResult);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(819, 450);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Json 美化";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtResult
            // 
            this.txtResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResult.IsReadOnly = false;
            this.txtResult.Location = new System.Drawing.Point(3, 3);
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(813, 444);
            this.txtResult.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(819, 450);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "帮助说明";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(19, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(629, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "在 Json 视图中可以使用右键菜单进行查找或标记。查找时，可以使用 Key==Value 方式进行键值匹配（两个等号）。\r\n";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(19, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(473, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "将 Json 文本粘贴到上面的文本框中，自动解析及美化，可使用右键菜单载入历史记录。";
            // 
            // mnuExpand
            // 
            this.mnuExpand.Name = "mnuExpand";
            this.mnuExpand.Size = new System.Drawing.Size(168, 24);
            this.mnuExpand.Text = "展开所有节点";
            this.mnuExpand.Click += new System.EventHandler(this.mnuExpand_Click);
            // 
            // mnuCollapse
            // 
            this.mnuCollapse.Name = "mnuCollapse";
            this.mnuCollapse.Size = new System.Drawing.Size(168, 24);
            this.mnuCollapse.Text = "折叠所有节点";
            this.mnuCollapse.Click += new System.EventHandler(this.mnuCollapse_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // frmJsonFormatter
            // 
            this.ClientSize = new System.Drawing.Size(827, 645);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.txtSource);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "frmJsonFormatter";
            this.Text = "Json 格式化";
            this.contextMenuStrip2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.plnStatus.ResumeLayout(false);
            this.plnStatus.PerformLayout();
            this.plnFind.ResumeLayout(false);
            this.plnFind.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Splitter splitter1;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn1;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn2;
        private System.Windows.Forms.RichTextBox txtSource;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuHistory;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem mnuFind;
        private System.Windows.Forms.ToolStripMenuItem mnuCopy;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private Fireasy.Windows.Forms.TreeList treeList1;
        private System.Windows.Forms.Panel plnFind;
        private System.Windows.Forms.Button btnTreeClose;
        private System.Windows.Forms.Button btnTreeFind;
        private Fireasy.Windows.Forms.ComplexTextBox txtTreeKeyword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage2;
        private ICSharpCode.TextEditor.TextEditorControl txtResult;
        private System.Windows.Forms.ToolStripMenuItem mnuCopyKey;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripMenuItem mnuMark;
        private System.Windows.Forms.ToolStripMenuItem mnuFromWeb;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel plnStatus;
        private System.Windows.Forms.TextBox lblStatus;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tRed;
        private System.Windows.Forms.ToolStripMenuItem tLime;
        private System.Windows.Forms.ToolStripMenuItem tAquamarine;
        private System.Windows.Forms.ToolStripMenuItem tDsBlue;
        private System.Windows.Forms.ToolStripMenuItem tCyan;
        private System.Windows.Forms.ToolStripMenuItem tOrange;
        private System.Windows.Forms.ToolStripMenuItem tGold;
        private System.Windows.Forms.ToolStripMenuItem tHotPink;
        private System.Windows.Forms.ToolStripMenuItem mnuClearMark;
        private System.Windows.Forms.ToolStripMenuItem mnuView;
        private System.Windows.Forms.ToolStripMenuItem tGreenYellow;
        private System.Windows.Forms.ToolStripMenuItem tYellow;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuExpand;
        private System.Windows.Forms.ToolStripMenuItem mnuCollapse;
    }
}