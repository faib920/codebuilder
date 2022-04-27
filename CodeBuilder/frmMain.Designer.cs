namespace CodeBuilder
{
    partial class frmMain
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNew = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuOption = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuBuild = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSource = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTemplate = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTool = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuProfileWnd = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPropertyWnd = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTemplateWnd = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOutputWnd = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuCloseCurrent = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCloseAll = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCloseOther = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTopic = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFeedback = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.spState = new System.Windows.Forms.ToolStripStatusLabel();
            this.spCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.spbar = new System.Windows.Forms.ToolStripProgressBar();
            this.treeListColumn1 = new Fireasy.Windows.Forms.TreeListColumn();
            this.treeListColumn2 = new Fireasy.Windows.Forms.TreeListColumn();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.tlbOpen = new System.Windows.Forms.ToolStripButton();
            this.tlbSave = new System.Windows.Forms.ToolStripButton();
            this.tlbBuild = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.dockMgr = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuSource,
            this.mnuTemplate,
            this.mnuTool,
            this.mnuWindow,
            this.mnuHelp});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1047, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNew,
            this.mnuOpen,
            this.mnuSave,
            this.mnuSaveAs,
            this.toolStripSeparator1,
            this.mnuOption,
            this.toolStripSeparator5,
            this.mnuBuild,
            this.toolStripSeparator3,
            this.mnuQuit});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(69, 24);
            this.mnuFile.Text = "文件(&F)";
            // 
            // mnuNew
            // 
            this.mnuNew.Name = "mnuNew";
            this.mnuNew.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.mnuNew.Size = new System.Drawing.Size(223, 24);
            this.mnuNew.Text = "新建";
            this.mnuNew.Click += new System.EventHandler(this.mnuNew_Click);
            // 
            // mnuOpen
            // 
            this.mnuOpen.Image = global::CodeBuilder.Properties.Resources.open1;
            this.mnuOpen.Name = "mnuOpen";
            this.mnuOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.mnuOpen.Size = new System.Drawing.Size(223, 24);
            this.mnuOpen.Text = "打开";
            this.mnuOpen.Click += new System.EventHandler(this.mnuOpen_Click);
            // 
            // mnuSave
            // 
            this.mnuSave.Image = global::CodeBuilder.Properties.Resources.save1;
            this.mnuSave.Name = "mnuSave";
            this.mnuSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.mnuSave.Size = new System.Drawing.Size(223, 24);
            this.mnuSave.Text = "保存";
            this.mnuSave.Click += new System.EventHandler(this.mnuSave_Click);
            // 
            // mnuSaveAs
            // 
            this.mnuSaveAs.Name = "mnuSaveAs";
            this.mnuSaveAs.Size = new System.Drawing.Size(223, 24);
            this.mnuSaveAs.Text = "另存为";
            this.mnuSaveAs.Click += new System.EventHandler(this.mnuSaveAs_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(220, 6);
            // 
            // mnuOption
            // 
            this.mnuOption.Name = "mnuOption";
            this.mnuOption.Size = new System.Drawing.Size(223, 24);
            this.mnuOption.Text = "首选项";
            this.mnuOption.Click += new System.EventHandler(this.mnuOption_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(220, 6);
            // 
            // mnuBuild
            // 
            this.mnuBuild.Image = global::CodeBuilder.Properties.Resources.build;
            this.mnuBuild.Name = "mnuBuild";
            this.mnuBuild.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
            this.mnuBuild.Size = new System.Drawing.Size(223, 24);
            this.mnuBuild.Text = "生成代码文件";
            this.mnuBuild.Click += new System.EventHandler(this.mnuBuild_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(220, 6);
            // 
            // mnuQuit
            // 
            this.mnuQuit.Name = "mnuQuit";
            this.mnuQuit.Size = new System.Drawing.Size(223, 24);
            this.mnuQuit.Text = "退出";
            this.mnuQuit.Click += new System.EventHandler(this.mnuQuit_Click);
            // 
            // mnuSource
            // 
            this.mnuSource.Name = "mnuSource";
            this.mnuSource.Size = new System.Drawing.Size(85, 24);
            this.mnuSource.Text = "数据源(&S)";
            // 
            // mnuTemplate
            // 
            this.mnuTemplate.Name = "mnuTemplate";
            this.mnuTemplate.Size = new System.Drawing.Size(70, 24);
            this.mnuTemplate.Text = "模板(&T)";
            // 
            // mnuTool
            // 
            this.mnuTool.Name = "mnuTool";
            this.mnuTool.Size = new System.Drawing.Size(73, 24);
            this.mnuTool.Text = "工具(&O)";
            // 
            // mnuWindow
            // 
            this.mnuWindow.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuProfileWnd,
            this.mnuPropertyWnd,
            this.mnuTemplateWnd,
            this.mnuOutputWnd,
            this.toolStripMenuItem1,
            this.mnuCloseCurrent,
            this.mnuCloseAll,
            this.mnuCloseOther});
            this.mnuWindow.Name = "mnuWindow";
            this.mnuWindow.Size = new System.Drawing.Size(76, 24);
            this.mnuWindow.Text = "窗口(&W)";
            // 
            // mnuProfileWnd
            // 
            this.mnuProfileWnd.Name = "mnuProfileWnd";
            this.mnuProfileWnd.Size = new System.Drawing.Size(198, 24);
            this.mnuProfileWnd.Text = "变量窗口";
            this.mnuProfileWnd.Click += new System.EventHandler(this.mnuProfileWnd_Click);
            // 
            // mnuPropertyWnd
            // 
            this.mnuPropertyWnd.Name = "mnuPropertyWnd";
            this.mnuPropertyWnd.Size = new System.Drawing.Size(198, 24);
            this.mnuPropertyWnd.Text = "属性窗口";
            this.mnuPropertyWnd.Click += new System.EventHandler(this.mnuPropertyWnd_Click);
            // 
            // mnuTemplateWnd
            // 
            this.mnuTemplateWnd.Name = "mnuTemplateWnd";
            this.mnuTemplateWnd.Size = new System.Drawing.Size(198, 24);
            this.mnuTemplateWnd.Text = "模板窗口";
            this.mnuTemplateWnd.Click += new System.EventHandler(this.mnuTemplateWnd_Click);
            // 
            // mnuOutputWnd
            // 
            this.mnuOutputWnd.Name = "mnuOutputWnd";
            this.mnuOutputWnd.Size = new System.Drawing.Size(198, 24);
            this.mnuOutputWnd.Text = "输出窗口";
            this.mnuOutputWnd.Click += new System.EventHandler(this.mnuOutputWnd_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(195, 6);
            // 
            // mnuCloseCurrent
            // 
            this.mnuCloseCurrent.Name = "mnuCloseCurrent";
            this.mnuCloseCurrent.Size = new System.Drawing.Size(198, 24);
            this.mnuCloseCurrent.Text = "关闭当前";
            this.mnuCloseCurrent.Click += new System.EventHandler(this.mnuCloseCurrent_Click);
            // 
            // mnuCloseAll
            // 
            this.mnuCloseAll.Name = "mnuCloseAll";
            this.mnuCloseAll.Size = new System.Drawing.Size(198, 24);
            this.mnuCloseAll.Text = "关闭所有";
            this.mnuCloseAll.Click += new System.EventHandler(this.mnuCloseAll_Click);
            // 
            // mnuCloseOther
            // 
            this.mnuCloseOther.Name = "mnuCloseOther";
            this.mnuCloseOther.Size = new System.Drawing.Size(198, 24);
            this.mnuCloseOther.Text = "除此之外全部关闭";
            this.mnuCloseOther.Click += new System.EventHandler(this.mnuCloseOther_Click);
            // 
            // mnuHelp
            // 
            this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuTopic,
            this.mnuFeedback,
            this.mnuUpdate,
            this.toolStripSeparator4,
            this.mnuAbout});
            this.mnuHelp.Name = "mnuHelp";
            this.mnuHelp.Size = new System.Drawing.Size(73, 24);
            this.mnuHelp.Text = "帮助(&H)";
            // 
            // mnuTopic
            // 
            this.mnuTopic.Name = "mnuTopic";
            this.mnuTopic.Size = new System.Drawing.Size(160, 24);
            this.mnuTopic.Text = "在线文档(&O)";
            this.mnuTopic.Click += new System.EventHandler(this.mnuTopic_Click);
            // 
            // mnuFeedback
            // 
            this.mnuFeedback.Name = "mnuFeedback";
            this.mnuFeedback.Size = new System.Drawing.Size(160, 24);
            this.mnuFeedback.Text = "问题反馈(&F)";
            this.mnuFeedback.Click += new System.EventHandler(this.mnuFeedback_Click);
            // 
            // mnuUpdate
            // 
            this.mnuUpdate.Name = "mnuUpdate";
            this.mnuUpdate.Size = new System.Drawing.Size(160, 24);
            this.mnuUpdate.Text = "检查更新(&U)";
            this.mnuUpdate.Click += new System.EventHandler(this.mnuUpdate_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(157, 6);
            // 
            // mnuAbout
            // 
            this.mnuAbout.Name = "mnuAbout";
            this.mnuAbout.Size = new System.Drawing.Size(160, 24);
            this.mnuAbout.Text = "关于(&A)";
            this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.spState,
            this.spCount,
            this.spbar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 575);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1047, 25);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // spState
            // 
            this.spState.Name = "spState";
            this.spState.Size = new System.Drawing.Size(1032, 20);
            this.spState.Spring = true;
            this.spState.Text = "就绪";
            this.spState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // spCount
            // 
            this.spCount.Name = "spCount";
            this.spCount.Size = new System.Drawing.Size(0, 20);
            // 
            // spbar
            // 
            this.spbar.Name = "spbar";
            this.spbar.Size = new System.Drawing.Size(200, 20);
            this.spbar.Visible = false;
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.CellForeColor = System.Drawing.Color.Empty;
            this.treeListColumn1.ForeColor = System.Drawing.Color.Empty;
            this.treeListColumn1.Formatter = null;
            this.treeListColumn1.Image = null;
            this.treeListColumn1.Text = "名称";
            this.treeListColumn1.Validator = null;
            this.treeListColumn1.Width = 200;
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
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "applications_16px_41384_easyicon.net.png");
            this.imageList2.Images.SetKeyName(1, "blue_property_16px_17017_easyicon.net.png");
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(1044, 59);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 516);
            this.splitter1.TabIndex = 4;
            this.splitter1.TabStop = false;
            // 
            // tlbOpen
            // 
            this.tlbOpen.Image = global::CodeBuilder.Properties.Resources.open1l;
            this.tlbOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbOpen.Name = "tlbOpen";
            this.tlbOpen.Size = new System.Drawing.Size(67, 28);
            this.tlbOpen.Text = "打开";
            this.tlbOpen.Click += new System.EventHandler(this.tlbOpen_Click);
            // 
            // tlbSave
            // 
            this.tlbSave.Image = global::CodeBuilder.Properties.Resources.save1l;
            this.tlbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbSave.Name = "tlbSave";
            this.tlbSave.Size = new System.Drawing.Size(67, 28);
            this.tlbSave.Text = "保存";
            this.tlbSave.Click += new System.EventHandler(this.tlbSave_Click);
            // 
            // tlbBuild
            // 
            this.tlbBuild.Image = global::CodeBuilder.Properties.Resources.buildl;
            this.tlbBuild.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbBuild.Name = "tlbBuild";
            this.tlbBuild.Size = new System.Drawing.Size(67, 28);
            this.tlbBuild.Text = "生成";
            this.tlbBuild.Click += new System.EventHandler(this.tlbBuild_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlbOpen,
            this.tlbSave,
            this.toolStripSeparator2,
            this.tlbBuild});
            this.toolStrip1.Location = new System.Drawing.Point(0, 28);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1047, 31);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 31);
            // 
            // dockMgr
            // 
            this.dockMgr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockMgr.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingWindow;
            this.dockMgr.Location = new System.Drawing.Point(0, 59);
            this.dockMgr.Name = "dockMgr";
            this.dockMgr.Size = new System.Drawing.Size(1044, 516);
            this.dockMgr.TabIndex = 5;
            this.dockMgr.ContentAdded += new System.EventHandler<WeifenLuo.WinFormsUI.Docking.DockContentEventArgs>(this.dockMgr_ContentAdded);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(199, 100);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(198, 24);
            this.toolStripMenuItem2.Text = "关闭当前";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.mnuCloseCurrent_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(198, 24);
            this.toolStripMenuItem3.Text = "关闭所有";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.mnuCloseAll_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(198, 24);
            this.toolStripMenuItem4.Text = "除此之外全部关闭";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.mnuCloseOther_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(198, 24);
            this.toolStripMenuItem5.Text = "不允许被关闭";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.mnuClosable_Click);
            // 
            // frmMain
            // 
            this.ClientSize = new System.Drawing.Size(1047, 600);
            this.Controls.Add(this.dockMgr);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "代码生成器(CodeBuilder)";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuSource;
        private System.Windows.Forms.ToolStripMenuItem mnuTemplate;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn1;
        private Fireasy.Windows.Forms.TreeListColumn treeListColumn2;
        private System.Windows.Forms.ToolStripStatusLabel spState;
        private System.Windows.Forms.ToolStripProgressBar spbar;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.ToolStripMenuItem mnuHelp;
        private System.Windows.Forms.ToolStripMenuItem mnuAbout;
        private System.Windows.Forms.ToolStripMenuItem mnuQuit;
        private System.Windows.Forms.ToolStripMenuItem mnuWindow;
        private System.Windows.Forms.ToolStripMenuItem mnuPropertyWnd;
        private System.Windows.Forms.ToolStripMenuItem mnuTemplateWnd;
        private System.Windows.Forms.ToolStripMenuItem mnuProfileWnd;
        private System.Windows.Forms.ToolStripMenuItem mnuNew;
        private System.Windows.Forms.ToolStripMenuItem mnuOpen;
        private System.Windows.Forms.ToolStripMenuItem mnuSave;
        private System.Windows.Forms.ToolStripMenuItem mnuOutputWnd;
        private System.Windows.Forms.ToolStripButton tlbOpen;
        private System.Windows.Forms.ToolStripButton tlbSave;
        private System.Windows.Forms.ToolStripButton tlbBuild;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockMgr;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveAs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mnuBuild;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mnuCloseCurrent;
        private System.Windows.Forms.ToolStripMenuItem mnuCloseAll;
        private System.Windows.Forms.ToolStripMenuItem mnuCloseOther;
        private System.Windows.Forms.ToolStripMenuItem mnuTopic;
        private System.Windows.Forms.ToolStripMenuItem mnuFeedback;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem mnuUpdate;
        private System.Windows.Forms.ToolStripMenuItem mnuTool;
        private System.Windows.Forms.ToolStripMenuItem mnuOption;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripStatusLabel spCount;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
    }
}

