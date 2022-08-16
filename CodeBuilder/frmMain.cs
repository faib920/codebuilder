// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using CodeBuilder.Core.Source;
using CodeBuilder.Core.Template;
using CodeBuilder.Core.Variable;
using CodeBuilder.Core.Forms;
using Fireasy.Common.Composition;
using Fireasy.Common.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace CodeBuilder
{
    public partial class frmMain : FormBase
    {
        private frmTable _frmTable;
        private frmProperty _frmProperty;
        private frmTemplate _frmTemplate;
        private frmProfile _frmProfile;
        private frmExtension _frmExtension;
        private frmOutput _frmOutput;
        private DevHosting _hosting;
        private Dictionary<string, ToolStripMenuItem> _menuCache = new Dictionary<string, ToolStripMenuItem>();

        public Action OnLoaded { get; set; }

        public Action<int> OnLoading { get; set; }

        public frmMain(Action<int> onLading)
        {
            OnLoading = onLading;

            InitializeComponent();

            var rect = Screen.GetWorkingArea(this);
            Width = rect.Width / 4 * 3;
            Height = rect.Height / 4 * 3;
            Left = (rect.Width - Width) / 2;
            Top = (rect.Height - Height) / 2; ;

            mnuPropertyWnd.Image = Properties.Resources.property.ToBitmap();
            mnuProfileWnd.Image = Properties.Resources.profile.ToBitmap();
            mnuExtensionWnd.Image = Properties.Resources.extension.ToBitmap();
            mnuTemplateWnd.Image = Properties.Resources.template.ToBitmap();
            mnuOutputWnd.Image = Properties.Resources.output.ToBitmap();
            Icon = Util.GetIcon();

            _hosting = new DevHosting().Hold();
            _hosting.MainWindow = dockMgr;
            _hosting.DockContainer = dockMgr;

            OnLoading(20);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            StaticUnity.Encoding = string.IsNullOrWhiteSpace(Config.Instance.Encoding) ?
                Encoding.Default : Encoding.GetEncoding(Config.Instance.Encoding);

            InitializeSourceMenus();
            OnLoading(30);

            InitializeTemplateMenus();
            OnLoading(40);

            InitializeToolMenus();
            OnLoading(50);

            ReBuild();

            OnLoading(60);

            OpenOutputForm();
            OnLoading(70);

            GetTableForm();
            OnLoading(80);

            OpenPeopertyForm();
            OpenProfileForm();
            OnLoading(90);

            OpenExtensionForm();
            OnLoading(100);

            OpenTemplateForm();

            _frmProperty.Activate();

            ThreadHelper.Start(CheckPluginUpdate);
            ThreadHelper.Start(CheckTemplateUpdate);

            _hosting.Hit("Main");

            OnLoaded();
        }

        private frmTable GetTableForm()
        {
            if (_frmTable == null)
            {
                _frmTable = new frmTable(_hosting);
                _frmTable.SelectItemAct = o =>
                    {
                        _hosting.ViewInPropGrid(o);
                    };
                _frmTable.CloseAct = () =>
                    {
                        _frmTable = null;
                        _hosting.ViewInPropGrid(null);
                    };
                _frmTable.CheckItemsAct = c =>
                    {
                        spCount.Text = "选择了 " + c + "个";
                    };
                _hosting.GetTablesFunc = () => _frmTable.GetTables(true);
                _frmTable.InitializeBuildMenu();
                _frmTable.Show(dockMgr, DockState.Document);
            }

            return _frmTable;
        }

        private void OpenPeopertyForm()
        {
            if (_frmProperty == null)
            {
                _frmProperty = new frmProperty((DevHosting)_hosting);
                _frmProperty.CloseAct = () => _frmProperty = null;
                _frmProperty.Show(dockMgr, DockState.DockRight);
            }
            else
            {
                _frmProperty.Activate();
            }
        }

        private void OpenTemplateForm()
        {
            if (_frmTemplate == null)
            {
                _frmTemplate = new frmTemplate(_hosting);
                _frmTemplate.OpenAct = t =>
                    {
                        OpenFileName(t);
                    };
                _frmTemplate.OpenTemplateAct = t =>
                    {
                        OpenTemplateFileName(t, () => _frmTemplate.Reload());
                    };
                _frmTemplate.CloseAct = () => _frmTemplate = null;
                _frmTemplate.TemplateAct = () =>
                {
                    ReInitializeTemplateSubMenus();
                    if (_frmExtension != null)
                    {
                        _frmExtension.Reload();
                    }
                };

                _frmTemplate.Show(dockMgr, DockState.DockRight);
            }
            else
            {
                _frmTemplate.Activate();
            }
        }

        private void OpenProfileForm()
        {
            if (_frmProfile == null)
            {
                _frmProfile = new frmProfile(_hosting);
                _frmProfile.PropertyChangeAct = () =>
                {
                    if (_frmTable != null)
                    {
                        _frmTable.ApplyProfile();
                    }
                };
                _frmProfile.CloseAct = () => _frmProfile = null;
                _frmProfile.Show(dockMgr, DockState.DockRight);
            }
            else
            {
                _frmProfile.Activate();
            }
        }

        private void OpenExtensionForm()
        {
            if (_frmExtension == null)
            {
                _frmExtension = new frmExtension(_hosting);
                _frmExtension.OpenAct = (t, c) =>
                    {
                        OpenExtensionFileName(t, c, () =>
                        {
                            if ((c == CodeCategory.CommonExtension || c == CodeCategory.ProfileExtension) && _hosting.ShowConfirm("变量扩展文件已改变，是否立即编译变量?") == ShowMsgButton.Yes)
                            {
                                ReBuildProfile();
                            }
                            if ((c == CodeCategory.CommonExtension || c == CodeCategory.SchemaExtension) && _hosting.ShowConfirm("架构扩展文件已改变，是否立即编译架构?") == ShowMsgButton.Yes)
                            {
                                ReBuildSchema();
                            }
                        });
                    };
                _frmExtension.Show(dockMgr, DockState.DockRight);
            }
            else
            {
                _frmExtension.Activate();
            }
        }

        private void OpenOutputForm()
        {
            if (_frmOutput == null)
            {
                _frmOutput = new frmOutput((DevHosting)_hosting);
                _frmOutput.CloseAct = () => _frmOutput = null;
            }

            _frmOutput.Show(dockMgr, DockState.DockBottom);
            _frmOutput.DockState = DockState.DockBottomAutoHide;
        }

        private void InitializeSourceMenus()
        {
            var sources = Imports.GetServices<ISourceProvider>();
            foreach (var p in sources)
            {
                p.Initialize(_hosting);
                var sItem = new ToolStripMenuItem();
                sItem.Text = p.Name;
                sItem.Name = p.Name;
                sItem.Tag = p;
                sItem.Click += sourceProvider_Click;

                mnuSource.DropDownItems.Add(sItem);
            }
        }

        private void InitializeTemplateMenus()
        {
            mnuTemplate.DropDownItems.Clear();

            TemplateUnity.Clear();

            var providers = Imports.GetServices<ITemplateProvider>();
            foreach (var p in providers)
            {
                p.Initialize(_hosting);
                var sItem = new ToolStripMenuItem();
                sItem.Text = p.Name;
                sItem.Name = p.Name;
                sItem.Tag = p;

                var current = Config.Instance.TemplateProvider == p.Name;
                if (current)
                {
                    sItem.ForeColor = Color.Blue;
                    _hosting.TemplateProvider = p;
                }

                var templates = p.GetTemplates();
                foreach (var f in templates)
                {
                    TemplateUnity.Add(p.Name, f);

                    var fItem = new ToolStripMenuItem();
                    fItem.Text = f.Name;
                    fItem.Tag = f;
                    if (current && Config.Instance.TemplateFileName.Equals(f.Id, StringComparison.CurrentCultureIgnoreCase))
                    {
                        _hosting.Template = f;
                        fItem.Checked = true;
                    }

                    fItem.Click += templateProvider_Click;

                    sItem.DropDownItems.Add(fItem);
                }

                mnuTemplate.DropDownItems.Add(sItem);
            }

            mnuTemplate.DropDownItems.Add(new ToolStripSeparator());
            var mnuReload = new ToolStripMenuItem("重新载入模板");
            mnuTemplate.DropDownItems.Add(mnuReload);
            mnuReload.Click += (o1, e1) =>
            {
                InitializeTemplateMenus();
                ReBuild();
            };

            var mnuImport = new ToolStripMenuItem("导入模板...");
            mnuTemplate.DropDownItems.Add(mnuImport);
            mnuImport.Click += (o1, e1) =>
            {
                using (var dialog = new OpenFileDialog()
                {
                    Filter = "模板定义包(*.tdp)|*.tdp",
                    Title = "导入模板"
                })
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        TemplateHelper.UnzipPackage(_hosting, _hosting.TemplateProvider, dialog.FileName);
                        InitializeTemplateMenus();

                        _hosting.ShowInfo("已成功从文件 " + dialog.FileName + " 导入模板。");
                    }
                }
            };

            var mnuOnline = new ToolStripMenuItem("在线模板商店...");
            mnuTemplate.DropDownItems.Add(mnuOnline);
            mnuOnline.Click += (o1, e1) =>
            {
                using (var form = new frmTemplateShop(_hosting))
                {
                    form.ShowDialog();
                    if (form.ChangedTemplates.Count > 0)
                    {
                        InitializeTemplateMenus();

                        if (_hosting.Template != null && form.ChangedTemplates.Any(s => s.Equals(_hosting.Template.Id, StringComparison.OrdinalIgnoreCase)))
                        {
                            ReBuild();
                        }
                    }
                }
            };
        }

        private void ReInitializeTemplateSubMenus()
        {
            if (!(mnuTemplate.DropDownItems.Find(_hosting.TemplateProvider.Name, false).FirstOrDefault() is ToolStripMenuItem root))
            {
                return;
            }

            root.DropDownItems.Clear();

            var templates = _hosting.TemplateProvider.GetTemplates();
            foreach (var t in templates)
            {
                var fItem = new ToolStripMenuItem();
                fItem.Text = t.Name;
                fItem.Tag = t;
                if (Config.Instance.TemplateFileName.Equals(t.Id, StringComparison.CurrentCultureIgnoreCase))
                {
                    _hosting.Template = t;
                    fItem.Checked = true;
                }

                fItem.Click += templateProvider_Click;

                root.DropDownItems.Add(fItem);
            }
        }

        private void ReBuild()
        {
            ReBuildProfile();
            ReBuildSchema();
        }

        private void ReBuildProfile()
        {
            try
            {
                _hosting.Profile = ProfileUnity.LoadProfile(_hosting, _hosting.Template);
            }
            catch (CompileException exp)
            {
                _hosting.ShowError("Profile 扩展代码编译失败。" + exp.FileName + "\r\n" + exp.Message);
                return;
            }

            if (_frmProfile != null)
            {
                _frmProfile.ReloadProfile();
            }
        }

        private void ReBuildSchema()
        {
            try
            {
                SchemaExtensionManager.Initialize(_hosting.Template);
            }
            catch (CompileException exp)
            {
                _hosting.ShowError("Schema 扩展代码编译失败。" + exp.FileName + "\r\n" + exp.Message);
                return;
            }

            if (_frmTable != null)
            {
                _frmTable.ReBuildSchema();
            }
        }

        private void InitializeToolMenus()
        {
            var providers = Imports.GetServices<IToolProvider>();
            foreach (var p in providers)
            {
                p.Initialize(_hosting);
                var sItem = new ToolStripMenuItem();
                sItem.Text = p.Name;
                sItem.Name = p.Name;
                sItem.Tag = p;

                if (p is IMultipleToolProvider mprovider)
                {
                    var subItems = mprovider.SubItems;
                    if (subItems != null)
                    {
                        foreach (var i in subItems)
                        {
                            if (i is ToolMenuItem tmi)
                            {
                                var iItem = new ToolStripMenuItem();
                                iItem.Text = tmi.Name;
                                iItem.Name = tmi.Name;
                                iItem.Tag = Tuple.Create(mprovider, tmi.Name, tmi.Parameter);
                                iItem.Click += toolProvider_Click;

                                sItem.DropDownItems.Add(iItem);
                            }
                            else if (i is ToolMenuSeparator)
                            {
                                sItem.DropDownItems.Add(new ToolStripSeparator());
                            }
                        }
                    }
                }
                else
                {
                    sItem.Click += toolProvider_Click;
                }

                mnuTool.DropDownItems.Add(sItem);
            }
        }

        private void LoadSourceStruct(ISourceProvider provider)
        {
            var option = new SourceOption { View = Config.Instance.Source_View };

            option.Selected = _frmTable.GetTableNames();

            var tables = provider.Preview(option);
            if (tables == null)
            {
                return;
            }

            GetSchemaAsync(provider, tables, option);
        }

        private void FillTables(IEnumerable<IObject> tables)
        {
            GetTableForm().FillTables(tables);
        }

        /// <summary>
        /// 使用异步方式加载表的构架。
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="source"></param>
        private void GetSchemaAsync(ISourceProvider provider, List<Table> source, SourceOption option)
        {
            spbar.Value = 0;
            spbar.Visible = true;

            List<Table> tables = null;

            if (option.SkipSchema)
            {
                tables = source;
            }
            else
            {
                Processor.Run(this, () =>
                {
                    tables = provider.GetSchema(source, (t, p) =>
                    {
                        Invoke(new Action(() =>
                        {
                            spState.Text = string.Format("{0}%，正在获取表 {1} 的结构...", p, t);
                            spbar.Value = p;
                        }));
                    });
                });
            }

            Invoke(new Action(() =>
            {
                FillTables(tables);
                spState.Text = "就绪";
                spbar.Value = 0;
                spbar.Visible = false;
            }));
        }

        private void NewEditor()
        {
            using (var frm = new frmNewCode(_hosting))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    var editform = new frmEditor(_hosting, CodeCategory.None) { Template = frm.Templage };
                    editform.Show(dockMgr, DockState.Document);
                }
            }
        }

        private void OpenFileName(TemplateFile fileItem)
        {
            foreach (DockContent content in dockMgr.Documents)
            {
                if (content is frmEditor)
                {
                    if ((content as frmEditor).FileName == fileItem.FilePath)
                    {
                        content.Activate();
                        return;
                    }
                }
            }

            var editform = new frmEditor(_hosting, CodeCategory.TemplateFile) { TemplateFile = fileItem };
            editform.Show(dockMgr, DockState.Document);
        }

        private void OpenTemplateFileName(TemplateDefinition template, Action saveAct)
        {
            foreach (DockContent content in dockMgr.Documents)
            {
                if (content is frmEditor)
                {
                    if ((content as frmEditor).FileName == template.ConfigFileName)
                    {
                        content.Activate();
                        return;
                    }
                }
            }

            var editform = new frmEditor(_hosting, CodeCategory.TemplateDefnition) { FileName = template.ConfigFileName, Language = "JavaScript", SaveAct = saveAct };
            editform.Show(dockMgr, DockState.Document);
        }

        private void OpenExtensionFileName(string fileName, CodeCategory category, Action saveAct)
        {
            foreach (DockContent content in dockMgr.Documents)
            {
                if (content is frmEditor)
                {
                    if ((content as frmEditor).FileName == fileName)
                    {
                        content.Activate();
                        return;
                    }
                }
            }

            var editform = new frmEditor(_hosting, fileName, category) { SaveAct = saveAct };
            editform.Show(dockMgr, DockState.Document);
        }

        private void OpenFileName(string fileName)
        {
            foreach (DockContent content in dockMgr.Documents)
            {
                if (content is frmEditor editor)
                {
                    if (editor.FileName == fileName)
                    {
                        content.Activate();
                        return;
                    }
                }
            }

            var editform = new frmEditor(_hosting, fileName);
            editform.Show(dockMgr, DockState.Document);
        }

        void sourceProvider_Click(object sender, EventArgs e)
        {
            if (!(sender is ToolStripMenuItem item) || item.Tag == null)
            {
                return;
            }

            if (!(item.Tag is ISourceProvider provider))
            {
                return;
            }

            _hosting.SourceProvider = provider;

            LoadSourceStruct(provider);
        }

        void templateProvider_Click(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            var owner = menu.OwnerItem as ToolStripMenuItem;
            var parent = owner.OwnerItem as ToolStripMenuItem;
            foreach (ToolStripItem sub in parent.DropDownItems)
            {
                if (!(sub is ToolStripMenuItem menuItem) || sub.Tag == null)
                {
                    continue;
                }

                foreach (ToolStripMenuItem sub1 in menuItem.DropDownItems)
                {
                    sub1.Checked = false;
                }

                sub.ForeColor = SystemColors.ControlText;
            }

            owner.ForeColor = Color.Blue;
            menu.Checked = true;

            var templateProvider = owner.Tag as ITemplateProvider;
            _hosting.Template = menu.Tag as TemplateDefinition;
            var providerChange = false;
            if (templateProvider != _hosting.TemplateProvider)
            {
                _hosting.TemplateProvider = templateProvider;
                providerChange = true;
            }

            if (providerChange || Config.Instance.TemplateFileName != _hosting.Template.Id)
            {
                Config.Instance.TemplateFileName = _hosting.Template.Id;
                Config.Instance.TemplateProvider = _hosting.TemplateProvider.Name;
                Config.Save();

                ReBuild();

                if (_frmTemplate != null)
                {
                    _frmTemplate.Reload();
                }

                if (_frmExtension != null)
                {
                    _frmExtension.Reload();
                }

                if (_frmTable != null)
                {
                    _frmTable.InitializeBuildMenu();
                }
            }
        }

        void toolProvider_Click(object sender, EventArgs e)
        {
            if (!(sender is ToolStripMenuItem item) || item.Tag == null)
            {
                return;
            }

            if (item.Tag is IToolProvider provider)
            {
                provider.Execute();
            }
            else if (item.Tag is Tuple<IMultipleToolProvider, string, object> mprovider)
            {
                mprovider.Item1.Execute(mprovider.Item2, mprovider.Item3);
            }
        }

        private void BuildCode()
        {
            if (_frmTable == null)
            {
                _hosting.ShowWarn("你还没有选择要生成的对象，请从【数据源】菜单中选择或配置。");
                return;
            }

            TemplateDefinition template = null;
            if (_hosting.TemplateProvider == null ||
                (template = _hosting.Template) == null)
            {
                _hosting.ShowWarn("你还没有选择生成模板，请从【模板】菜单中选择。");
                return;
            }

            var tables = _frmTable.GetTables(true);
            if (tables.Count == 0)
            {
                _hosting.ShowWarn("你还没有选择要生成的对象，请从【数据源】菜单中选择或配置。");
                return;
            }

            var ckResult = ValidationUnity.Validate(_hosting.Profile, tables);
            if (!string.IsNullOrEmpty(ckResult))
            {
                _hosting.ShowWarn(ckResult);
                return;
            }

            var option = new TemplateOption();
            option.Template = template;
            option.DynamicAssemblies.AddRange(StaticUnity.DynamicAssemblies);
            option.Profile = _hosting.Profile;
            option.WriteToDisk = true;
            option.SkipWhenFileExists = Config.Instance.SkipWhenFileExists;

            using (var frm = new frmPreBuild(_hosting) { Template = _hosting.Template })
            {
                if (frm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                option.Partitions = frm.Partitions;
                option.OutputDirectory = Config.Instance.OutputDirectory;
            }

            spbar.Value = 0;
            spbar.Visible = true;

            var time = Processor.Run(this, () =>
                {
                    _hosting.Hit("Build_" + template.TId);
                    _hosting.TemplateProvider.GenerateFiles(option, tables, (s, p) =>
                        {
                            Invoke(new Action(() =>
                                {
                                    spState.Text = string.Format("{0}%，正在生成 {1} 的代码文件...", p, s);
                                    spbar.Value = p;
                                }));
                        });

                    Invoke(new Action(() =>
                        {
                            spState.Text = "就绪";
                            spbar.Value = 0;
                            spbar.Visible = false;
                            Process.Start(Config.Instance.OutputDirectory);
                        }));
                });

            var client = new HttpClient();
            client.PostAsync(Config.Instance.TemplateServerUrl + "/use/" + template.Id, null);
        }

        #region 菜单事件
        private void mnuProfileWnd_Click(object sender, EventArgs e)
        {
            OpenProfileForm();
        }

        private void mnuPropertyWnd_Click(object sender, EventArgs e)
        {
            OpenPeopertyForm();
        }

        private void mnuTemplateWnd_Click(object sender, EventArgs e)
        {
            OpenTemplateForm();
        }

        private void mnuExtensionWnd_Click(object sender, EventArgs e)
        {
            OpenExtensionForm();
        }

        private void mnuOutputWnd_Click(object sender, EventArgs e)
        {
            OpenOutputForm();
        }

        private void mnuNew_Click(object sender, EventArgs e)
        {
            NewEditor();
        }

        private void mnuOpen_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = FileTypeHelper.GetAllFilters();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    OpenFileName(dialog.FileName);
                }
            }
        }

        private void mnuSave_Click(object sender, EventArgs e)
        {
            if (dockMgr.ActiveContent is frmEditor frm)
            {
                frm.SaveFile();
            }
            else if (dockMgr.ActiveContent is frmTable frm1)
            {
                frm1.SaveFile();
            }
        }

        private void mnuSaveAs_Click(object sender, EventArgs e)
        {
            if (dockMgr.ActiveContent is frmEditor frm)
            {
                frm.SaveAs();
            }
        }

        private void mnuSaveAll_Click(object sender, EventArgs e)
        {
            var isChangeProfile = false;
            var isChangeSchema = false;
            foreach (var c in dockMgr.Contents)
            {
                if (c is frmEditor frm)
                {
                    if (frm.SaveFile(false))
                    {
                        if (frm.Category == CodeCategory.ProfileExtension && !isChangeProfile)
                        {
                            isChangeProfile = true;
                        }
                        else if (frm.Category == CodeCategory.SchemaExtension && !isChangeSchema)
                        {
                            isChangeSchema = true;
                        }
                    }
                }
            }

            if (isChangeProfile && _hosting.ShowConfirm("变量扩展文件已改变，是否立即编译变量?") == ShowMsgButton.Yes)
            {
                ReBuildProfile();
            }
            if (isChangeSchema && _hosting.ShowConfirm("架构扩展文件已改变，是否立即编译架构?") == ShowMsgButton.Yes)
            {
                ReBuildSchema();
            }
        }

        private void mnuOption_Click(object sender, EventArgs e)
        {
            using (var frm = new frmOption(_hosting))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                }
            }
        }

        private void mnuBuild_Click(object sender, EventArgs e)
        {
            BuildCode();
        }

        private void mnuQuit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void mnuCloseCurrent_Click(object sender, EventArgs e)
        {
            if (dockMgr.ActiveDocument is DockContent dc && dc.CloseButtonVisible)
            {
                dc.DockHandler.Close();
            }
        }

        private void mnuCloseAll_Click(object sender, EventArgs e)
        {
            var documents = dockMgr.DocumentsToArray();

            foreach (IDockContent content in documents)
            {
                if (content is DockContent dc && dc.CloseButtonVisible)
                {
                    content.DockHandler.Close();
                }
            }
        }

        private void mnuCloseOther_Click(object sender, EventArgs e)
        {
            var documents = dockMgr.DocumentsToArray();

            foreach (IDockContent content in documents)
            {
                if (content is DockContent dc && dc.CloseButtonVisible && !content.Equals(dockMgr.ActiveDocument))
                {
                    content.DockHandler.Close();
                }
            }
        }

        private void mnuClosable_Click(object sender, EventArgs e)
        {
            toolStripMenuItem5.Checked = !toolStripMenuItem5.Checked;
            if (dockMgr.ActiveDocument is DockContent dc)
            {
                dc.CloseButtonVisible = !toolStripMenuItem5.Checked;
            }
        }

        private void mnuAbout_Click(object sender, EventArgs e)
        {
            using (var frm = new frmAbout())
            {
                frm.ShowDialog();
            }
        }

        private void mnuTopic_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.fireasy.cn/codebuilder/docs");
        }

        private void mnuUpdate_Click(object sender, EventArgs e)
        {
            if (!File.Exists("AutoUpdate.exe"))
            {
                _hosting.ShowError("无法进行更新，找不到程序 AutoUpdate.exe。");
                return;
            }

            Process.Start("AutoUpdate.exe", "/T");
        }

        private void mnuFeedback_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.fireasy.cn/codebuilder/feedback");
        }

        #endregion

        #region 工具栏事件
        private void tlbOpen_Click(object sender, EventArgs e)
        {
            mnuOpen_Click(null, null);
        }

        private void tlbSave_Click(object sender, EventArgs e)
        {
            mnuSave_Click(null, null);
        }

        private void tlbBuild_Click(object sender, EventArgs e)
        {
            BuildCode();
        }
        #endregion

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            var list = new List<IChangeManager>();
            foreach (IDockContent content in dockMgr.Contents)
            {
                if (content is IChangeManager chgMgr && chgMgr.IsChanged)
                {
                    list.Add(chgMgr);
                }
            }

            if (list.Count > 0)
            {
                var r = _hosting.ShowConfirm("有文档未已修改，是否保存后再退出?", 3);
                if (r == ShowMsgButton.Yes)
                {
                    list.ForEach(s => s.SaveFile(false));
                }
                else if (r == ShowMsgButton.Cancel)
                {
                    e.Cancel = true;
                }
            }

            base.OnClosing(e);
        }

        private void dockMgr_ContentAdded(object sender, DockContentEventArgs e)
        {
            if (e.Content is DockContent content && content is IClosableDockManaged)
            {
                content.TabPageContextMenuStrip = contextMenuStrip1;
            }
        }

        private void dockMgr_ActiveDocumentChanged(object sender, EventArgs e)
        {
            InitializeEditMenu(dockMgr.ActiveDocument);
            InitializeContextMenu(dockMgr.ActiveDocument);
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (dockMgr.ActiveDocument is DockContent content)
            {
                toolStripMenuItem5.Checked = !content.CloseButtonVisible;
            }
        }

        private void CloneMenuItems(ToolStripItemCollection target, ToolStripItemCollection source, Action<ToolStripItem> initializer = null)
        {
            foreach (ToolStripItem item in source)
            {
                if (item is ToolStripMenuItem menuItem)
                {
                    var newItem = new ToolStripMenuItem();
                    newItem.Text = item.Text;
                    newItem.Image = item.Image;
                    newItem.Name = item.Name;
                    newItem.ShortcutKeys = menuItem.ShortcutKeys;
                    newItem.ToolTipText = menuItem.ToolTipText;
                    newItem.Tag = menuItem.Tag;
                    newItem.Click += (sender, e) =>
                    {
                        var o = sender as ToolStripMenuItem;
                        if (!string.IsNullOrEmpty(o.Name))
                        {
                            (dockMgr.ActiveContent as IEditMenuManager).Invoke(o.Name.Replace("mnu", string.Empty), o.Tag);
                        }
                    };

                    initializer?.Invoke(newItem);

                    target.Add(newItem);

                    if (menuItem.HasDropDownItems)
                    {
                        CloneMenuItems(newItem.DropDownItems, menuItem.DropDownItems, initializer);
                    }
                }
                else if (item is ToolStripSeparator)
                {
                    var sep = new ToolStripSeparator();
                    initializer?.Invoke(sep);
                    target.Add(sep);
                }
            }
        }

        private void InitializeEditMenu(IDockContent content)
        {
            mnuEdit.DropDownItems.Clear();

            if (content is IEditMenuManager mgr)
            {
                ThreadHelper.Start(() =>
                {
                    this.Invoke(new Action(() =>
                    {
                        if (!_menuCache.TryGetValue(mgr.Key, out ToolStripMenuItem col))
                        {
                            col = new ToolStripMenuItem { Text = menuStrip1.Items[1].Text };
                            CloneMenuItems(col.DropDownItems, mgr.GetEditMenuItems());
                            _menuCache.Add(mgr.Key, col);
                        }

                        menuStrip1.Items.RemoveAt(1);
                        menuStrip1.Items.Insert(1, col);
                        menuStrip1.Items[1].Visible = col.HasDropDownItems;
                    }));
                });
            }
            else
            {
                menuStrip1.Items[1].Visible = false;
            }
        }

        private void InitializeContextMenu(IDockContent content)
        {
            for (var i = contextMenuStrip1.Items.Count - 1; i >= 0; i--)
            {
                if (contextMenuStrip1.Items[i].Tag is string str && str == "Cust")
                {
                    contextMenuStrip1.Items.RemoveAt(i);
                }
            }

            if (content is IContextMenuManager mgr)
            {
                ThreadHelper.Start(() =>
                {
                    this.Invoke(new Action(() =>
                    {
                        var items = mgr.GetContextMenuItems().ToArray();
                        if (items.Length > 0)
                        {
                            var sep = new ToolStripSeparator { Tag = "Cust" };
                            contextMenuStrip1.Items.Add(sep);

                            CloneMenuItems(contextMenuStrip1.Items, new ToolStripItemCollection(contextMenuStrip1, items), s => s.Tag = "Cust");
                        }
                    }));
                });
            }
        }

        /// <summary>
        /// 检查插件是否可更新
        /// </summary>
        private async void CheckPluginUpdate()
        {
            var client = new HttpClient();
            var page = 0;
            var updateCount = 0;

            var response = await client.GetAsync(Config.PluginServerUrl + "?page=" + page + "&rows=20&filter=source|template|tool");
            while (true)
            {
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return;
                }

                var content = await response.Content.ReadAsStringAsync();
                var data = new JsonSerializer().Deserialize<OnlinePluginData>(content);
                foreach (var plug in data.Items)
                {
                    var fileName = Path.Combine(Application.StartupPath, plug.Code + ".dll");
                    if (File.Exists(fileName))
                    {
                        if (VersionHelper.CompareVersion(fileName, plug.Version) > 0)
                        {
                            updateCount++;
                        }
                    }
                }

                if (data.LastPage)
                {
                    break;
                }
            }

            if (updateCount > 0)
            {
                var ss = new ToolStripStatusLabel();
                ss.IsLink = true;
                ss.Visible = true;
                ss.LinkBehavior = LinkBehavior.HoverUnderline;
                ss.Text = "插件更新(" + updateCount + ")";
                ss.ToolTipText = "检查到 " + updateCount + " 个插件可更新";
                ss.Click += (o, e) =>
                {
                    var frm = new frmPluginShop(_hosting);
                    frm.ShowDialog();
                };

                this.Invoke(new Action(() =>
                {
                    statusStrip1.Items.Add(ss);
                }));
            }
        }

        /// <summary>
        /// 检查模板是否可更新
        /// </summary>
        private async void CheckTemplateUpdate()
        {
            var client = new HttpClient();
            var page = 0;
            var updateCount = 0;

            var response = await client.GetAsync(Config.Instance.TemplateServerUrl + "?page=" + page + "&rows=20");
            while (true)
            {
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return;
                }

                var content = await response.Content.ReadAsStringAsync();
                var data = new JsonSerializer().Deserialize<OnlineTemplateData>(content);
                foreach (var tmp in data.Items)
                {
                    if (TemplateUnity.TryGet(tmp.Category, tmp.Code, out TemplateDefinition definition))
                    {
                        if (definition.Version < tmp.Version)
                        {
                            updateCount++;
                        }
                    }
                }

                if (data.LastPage)
                {
                    break;
                }
            }

            if (updateCount > 0)
            {
                var ss = new ToolStripStatusLabel();
                ss.IsLink = true;
                ss.Visible = true;
                ss.LinkBehavior = LinkBehavior.HoverUnderline;
                ss.Text = "模板更新(" + updateCount + ")";
                ss.ToolTipText = "检查到 " + updateCount + " 个模板可更新";
                ss.Click += (o, e) =>
                {
                    var frm = new frmTemplateShop(_hosting);
                    frm.ShowDialog();
                };

                this.Invoke(new Action(() =>
                {
                    statusStrip1.Items.Add(ss);
                }));
            }
        }

        private class MyToolStripDropDown : ToolStripDropDown
        {
            private readonly ToolStripItemCollection _collection;

            public MyToolStripDropDown(ToolStripItemCollection collection)
            {
                _collection = collection;
            }

            public override ToolStripItemCollection Items => _collection;
        }
    }
}
