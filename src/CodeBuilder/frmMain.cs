// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using CodeBuilder.Core.EventBus;
using CodeBuilder.Core.Forms;
using CodeBuilder.Core.Source;
using CodeBuilder.Core.Template;
using CodeBuilder.Core.Tool;
using CodeBuilder.Core.Validations;
using CodeBuilder.Core.Variable;
using Fireasy.Common.Compiler;
using Fireasy.Common.Extensions;
using Fireasy.Composition;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using static CodeBuilder.frmTable;

namespace CodeBuilder
{
    public partial class frmMain : FormBase, INotifyWindow
    {
        private frmTable _frmTable;
        private frmProperty _frmProperty;
        private frmTemplate _frmTemplate;
        private frmProfile _frmProfile;
        private frmExtension _frmExtension;
        private frmOutput _frmOutput;
        private frmResource _frmResource;
        private frmGuide _frmGuide;
        private DevHosting _hosting;
        private Dictionary<string, ToolStripMenuItem> _menuCache = new Dictionary<string, ToolStripMenuItem>();
        private int _generateCount;
        private bool _useGuide = false;
        private bool _isRebuiding;
        private readonly string _fileName;
        private const string READYING = "就绪";
        private Dictionary<UpdateFlag, bool> _filterOpts = new Dictionary<UpdateFlag, bool>
        {
            { UpdateFlag.Added, false },
            { UpdateFlag.Modified, false },
            { UpdateFlag.Removed, false },
        };

        public Action OnLoaded { get; set; }

        public Action<int> OnLoading { get; set; }

        public frmMain(Action<int> onLading, string fileName)
        {
            OnLoading = onLading;
            _fileName = fileName;
            InitializeComponent();

            var rect = Screen.GetWorkingArea(this);
            Width = rect.Width / 4 * 3;
            Height = rect.Height / 4 * 3;
            Left = (rect.Width - Width) / 2;
            Top = (rect.Height - Height) / 2;

            mnuPropertyWnd.Image = Properties.Resources.property.ToBitmap();
            mnuProfileWnd.Image = Properties.Resources.profile.ToBitmap();
            mnuExtensionWnd.Image = Properties.Resources.extension.ToBitmap();
            mnuTemplateWnd.Image = Properties.Resources.template.ToBitmap();
            mnuOutputWnd.Image = Properties.Resources.output.ToBitmap();
            mnuResWnd.Image = Properties.Resources.resource.ToBitmap();

            Icon = Util.GetIcon();

            #region 初始化 devhosting
            _hosting = new DevHosting().Hold();
            _hosting.MainWindow = dockMgr;
            _hosting.DockContainer = dockMgr;
            _hosting.OnSourceHistoryChanged = () =>
            {
                if (_frmResource != null)
                {
                    _frmResource.LoadResources();
                }
            };
            _hosting.ProgressAct = (s, p) =>
            {
                Invoke(new Action(() =>
                {
                    if (!spbar.Visible && p >= 0 && p <= 100)
                    {
                        spbar.Value = 0;
                        spbar.Visible = true;
                    }

                    if (!string.IsNullOrEmpty(s))
                    {
                        spState.Text = s;
                    }

                    if (notifyIcon1.Visible)
                    {
                        notifyIcon1.Text = $"已完成 {Math.Min(p, 100)}%";
                    }

                    if (p >= 0 && p <= 100)
                    {
                        spbar.Value = Math.Min(p, 100);
                    }
                    if (p == 100)
                    {
                        spbar.Visible = false;
                        spState.Text = READYING;
                    }
                }));
            };
            _hosting.OpenFileAct = (f, l) => OpenFileName(f, l);
            _hosting.ShowAboutAct = () => mnuAbout_Click(null, null);
            _hosting.ShowDonateAct = () => mnuDonate_Click(null, null);
            #endregion

            OnLoading(20);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            StaticUnity.Encoding = string.IsNullOrWhiteSpace(Config.Instance.Encoding) ?
                Encoding.Default : Util.GetEncoding(Config.Instance.Encoding);

            InitializeSourceMenus();
            OnLoading(20);

            InitializeTemplateMenus();
            OnLoading(30);

            InitializeToolMenus();
            OnLoading(40);


            if (Config.Instance.Windows.Contains(frmOutput.WndName))
            {
                OpenOutputForm();
                OnLoading(50);
            }

            OpenTableForm();
            OnLoading(60);

            OpenPropertyForm();
            OpenProfileForm();
            OnLoading(80);

            if (Config.Instance.Windows.Contains(frmExtension.WndName))
            {
                OpenExtensionForm();
            }

            if (Config.Instance.Windows.Contains(frmTemplate.WndName))
            {
                OpenTemplateForm();
            }

            if (Config.Instance.Windows.Contains(frmResource.WndName))
            {
                OpenResourceForm();
            }

            OnLoading(100);

            _frmProperty.Activate();

            ReBuildSchemaAndProfile(true, clearExpired: true);

            ThreadHelper.Start(CheckPluginUpdate);
            ThreadHelper.Start(CheckTemplateUpdate);
            ThreadHelper.Start(CheckVisitData);

#if !DEBUG
            _hosting.Hit("Main");
#endif

            OnLoaded();

            var providers = _hosting.ServiceProvider.GetExportedServices<IToolProvider>();
            foreach (var p in providers)
            {
                if (p is IBootstrapTool bt)
                {
                    bt.Run();
                }
            }

            notifyIcon1.Icon = Icon;
            notifyIcon1.Visible = false;

            if (!Config.Instance.Guided)
            {
                ShowSourceGuide(true);
            }

            ValidAccessToken();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_generateCount >= 2)
            {
                mnuDonate_Click(null, null);
            }
        }

        private frmTable OpenTableForm()
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
                        spCount.Text = $"选择了 {c} 个对象";
                    };
                _frmTable.ShowValidationAct = ShowValidateResult;
                _frmTable.ShowSynchronizedAct = ShowSyncStatusPanels;

                _hosting.GetTablesFunc = () => _frmTable.GetTables(true);
                _frmTable.InitializeBuildMenu();
                _frmTable.Show(dockMgr, DockState.Document);
                if (!string.IsNullOrEmpty(_fileName))
                {
                    _frmTable.OpenFile(_fileName);
                }
            }
            else
            {
                _frmTable.Activate();
            }

            return _frmTable;
        }

        private void OpenPropertyForm()
        {
            if (_frmProperty == null)
            {
                _frmProperty = new frmProperty(_hosting);
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
                _frmTemplate.TemplateCodeFileChangeAct = () =>
                {
                    ReBuildSchemaAndProfile();
                };
                _frmTemplate.CloseAct = () => _frmTemplate = null;
                _frmTemplate.TemplateAct = () =>
                {
                    _frmTable.InitializeBuildMenu();
                    ReInitializeTemplateSubMenus();
                    _frmExtension?.Reload();
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
                _frmExtension.CloseAct = () => _frmExtension = null;
                _frmExtension.OpenAct = (t, c) =>
                    {
                        OpenExtensionFileName(t, c, () =>
                        {
                            if (c == CodeCategory.CommonExtension && _hosting.ShowConfirm("公共扩展文件已改变，是否立即编译所有变量和架构?") != ShowMsgButton.Yes)
                            {
                                return;
                            }
                            else if (c == CodeCategory.ProfileExtension && _hosting.ShowConfirm("变量扩展文件已改变，是否立即编译变量?") != ShowMsgButton.Yes)
                            {
                                return;
                            }
                            else if (c == CodeCategory.SchemaExtension && _hosting.ShowConfirm("架构扩展文件已改变，是否立即编译架构?") != ShowMsgButton.Yes)
                            {
                                return;
                            }

                            ReBuildSchemaAndProfile(forceBuild: true);
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
                _frmOutput = new frmOutput(_hosting);
                _frmOutput.CloseAct = () => _frmOutput = null;
                _frmOutput.Show(dockMgr, DockState.DockBottomAutoHide);
            }
            else
            {
                _frmOutput.Activate();
            }
        }

        private void OpenResourceForm()
        {
            if (_frmResource == null)
            {
                _frmResource = new frmResource(_hosting);
                _frmResource.LoadHistoryAct = GetSchem;
                _frmResource.CloseAct = () => _frmResource = null;
                _frmResource.Show(dockMgr, DockState.DockLeftAutoHide);
            }
            else
            {
                _frmResource.Activate();
            }
        }

        private void InitializeSourceMenus()
        {
            var sources = _hosting.ServiceProvider.GetExportedServices<ISourceProvider>();
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

            var providers = _hosting.ServiceProvider.GetExportedServices<ITemplateProvider>();
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
                if (Config.Instance.TemplateGroup > 0)
                {
                    if (templates.GroupBy(s => Config.Instance.TemplateGroup == Config.TemplateGroupStyle.Language ? s.Language : s.Category ?? string.Empty).Count() == 1)
                    {
                        foreach (var temp in templates)
                        {
                            TemplateUnity.Add(p.Name, temp);

                            FillTemplateMenus(current, temp, sItem);
                        }
                    }
                    else
                    {
                        foreach (var group in templates
                            .GroupBy(s => Config.Instance.TemplateGroup == Config.TemplateGroupStyle.Language ? s.Language : s.Category ?? string.Empty)
                            .OrderBy(s => string.IsNullOrEmpty(s.Key) ? 9 : 1))
                        {
                            var gItem = new ToolStripMenuItem();
                            gItem.Text = string.IsNullOrEmpty(group.Key) ? "其他" : group.Key;

                            foreach (var temp in group)
                            {
                                TemplateUnity.Add(p.Name, temp);
                                if (FillTemplateMenus(current, temp, gItem))
                                {
                                    gItem.ForeColor = Color.Blue;
                                }
                            }

                            sItem.DropDownItems.Add(gItem);
                        }
                    }
                }
                else
                {
                    foreach (var temp in templates)
                    {
                        TemplateUnity.Add(p.Name, temp);

                        FillTemplateMenus(current, temp, sItem);
                    }
                }

                mnuTemplate.DropDownItems.Add(sItem);
            }

            mnuTemplate.DropDownItems.Add(new ToolStripSeparator());
            var mnuReload = new ToolStripMenuItem("重新载入模板");
            mnuTemplate.DropDownItems.Add(mnuReload);
            mnuReload.Click += (o1, e1) =>
            {
                InitializeTemplateMenus();
                ReBuildSchemaAndProfile();
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
                    form.OnUpdated = () => ThreadHelper.Start(CheckTemplateUpdate);
                    form.ShowDialog();
                    if (form.ChangedTemplates.Count > 0)
                    {
                        InitializeTemplateMenus();

                        if (_hosting.Template != null && form.ChangedTemplates.Any(s => s.Equals(_hosting.Template.Id, StringComparison.OrdinalIgnoreCase)))
                        {
                            ReBuildSchemaAndProfile();
                        }
                    }
                }
            };
        }

        private bool FillTemplateMenus(bool current, TemplateDefinition definition, ToolStripMenuItem parent)
        {
            var fItem = new ToolStripMenuItem();
            fItem.Text = definition.Name;
            fItem.Tag = definition;
            var selected = false;

            if (current && Config.Instance.TemplateFileName.Equals(definition.Id, StringComparison.OrdinalIgnoreCase))
            {
                _hosting.Template = definition;
                fItem.Checked = true;
                selected = true;
            }

            fItem.Click += templateProvider_Click;

            parent.DropDownItems.Add(fItem);

            return selected;
        }

        private void ReInitializeTemplateSubMenus()
        {
            if (!(mnuTemplate.DropDownItems.Find(_hosting.TemplateProvider.Name, false).FirstOrDefault() is ToolStripMenuItem root))
            {
                return;
            }

            root.DropDownItems.Clear();

            var templates = _hosting.TemplateProvider.GetTemplates();
            if (Config.Instance.TemplateGroup > 0)
            {
                if (templates.GroupBy(s => Config.Instance.TemplateGroup == Config.TemplateGroupStyle.Language ? s.Language : s.Category ?? string.Empty).Count() == 1)
                {
                    foreach (var temp in templates)
                    {
                        FillTemplateMenus(true, temp, root);
                    }
                }
                else
                {
                    foreach (var group in templates
                        .GroupBy(s => Config.Instance.TemplateGroup == Config.TemplateGroupStyle.Language ? s.Language : s.Category ?? string.Empty)
                        .OrderBy(s => string.IsNullOrEmpty(s.Key) ? 9 : 1))
                    {
                        var gItem = new ToolStripMenuItem();
                        gItem.Text = string.IsNullOrEmpty(group.Key) ? "其他" : group.Key;

                        foreach (var temp in group)
                        {
                            if (FillTemplateMenus(true, temp, gItem))
                            {
                                gItem.ForeColor = Color.Blue;
                            }
                        }

                        root.DropDownItems.Add(gItem);
                    }
                }
            }
            else
            {
                foreach (var temp in templates)
                {
                    FillTemplateMenus(true, temp, root);
                }
            }
        }

        private void ReBuildSchemaAndProfile(bool initEditorInsertMenus = false, bool forceBuild = false, bool clearExpired = false)
        {
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
            {
                while (_isRebuiding && !cts.IsCancellationRequested)
                {
                    Thread.Sleep(100);
                    Application.DoEvents();
                }

                _isRebuiding = true;

                this.Invoke(new Action(() =>
                {
                    Cursor = Cursors.WaitCursor;
                    spState.Text = "正在加载扩展，请稍候...";
                }));

                try
                {
                    var compileManager = _hosting.ServiceProvider.GetService<ICompileManager>();

                    if (clearExpired)
                    {
                        compileManager.ClearExpiredFiles();
                    }

                    compileManager.Compile(_hosting.Template, forceBuild);

                    _hosting.Profile = ProfileUnity.LoadProfile(_hosting, _hosting.Template);

                    _frmProfile?.Invoke(new Action(() =>
                    {
                        _frmProfile.ReloadProfile();
                    }));

                    var schemaExtManager = _hosting.ServiceProvider.TryGetService<ISchemaExtensionManager>();
                    schemaExtManager.Initialize(_hosting.Template);

                    _frmTable?.Invoke(new Action(() =>
                    {
                        _frmTable.ReBuildSchema();
                    }));
                }
                catch (CodeCompileException exp)
                {
                    this.Invoke(new Action(() =>
                    {
                        _hosting.ShowError(new TemplateLoadException($"加载模板 {_hosting.Template.Name} 时报错了，请检查相关扩展代码。", exp));
                    }));
                }
                catch (Exception exp)
                {
                    this.Invoke(new Action(() =>
                    {
                        _hosting.ShowError(new TemplateLoadException($"加载模板 {_hosting.Template.Name} 时报错了。", exp));
                    }));
                }
                finally
                {
                    this.Invoke(new Action(() =>
                    {
                        Cursor = Cursors.Default;
                    }));
                }

                if (initEditorInsertMenus)
                {
                    foreach (DockContent content in dockMgr.Documents)
                    {
                        if (content is frmEditor editor)
                        {
                            editor.InitializeInsertMenus();
                        }
                    }
                }

                this.Invoke(new Action(() =>
                {
                    spState.Text = READYING;
                }));

                _isRebuiding = false;
            }
        }

        private void InitializeToolMenus()
        {
            var providers = _hosting.ServiceProvider.GetExportedServices<IToolProvider>();
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

        private async Task LoadSourceStruct(ISourceProvider provider)
        {
            var option = new SourceOption { View = Config.Instance.Source_View };

            option.Selected = _frmTable.GetTableNames();

            Cursor = Cursors.WaitCursor;
            var tables = await provider.PreviewAsync(option);
            Cursor = Cursors.Default;

            if (tables == null)
            {
                return;
            }

            _frmGuide?.Hide();
            _frmTable.Activate();

            if (option.Selected?.Count > 0 && _hosting.ShowConfirm("是否以同步的方式加载并更新到列表中?") == ShowMsgButton.Yes)
            {
                option.Synchronize = true;
            }

            GetSchem(provider, tables, option);
        }

        private void FillTables(IEnumerable<Table> tables, LoadMode loadMode)
        {
            if (tables == null)
            {
                return;
            }

            var form = OpenTableForm();
            form.FillTables(tables, loadMode);
        }

        /// <summary>v
        /// 使用异步方式加载表的构架。
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="source"></param>
        private void GetSchem(ISourceProvider provider, List<Table> source, SourceOption option)
        {
            List<Table> tables = null;
            var isCancellation = false;

            if (option.SkipSchema)
            {
                tables = source;

                FillTables(tables, GetLoadMode(option));
                Text = Text.Split('-')[0].Trim();
            }
            else
            {
                var time = Processor.Run(this, async calcelToken =>
                {
                    tables = await provider.GetSchemaAsync(source, (t, p) =>
                    {
                        _hosting.ShowProgress($"{p}% 正在获取表 {t} 的结构...", p);
                    }, calcelToken);
                }, () => isCancellation = true, true);

                if (!isCancellation && tables != null)
                {
                    _hosting.ConsoleInfo($"表、字段及外键信息已获取完毕，共耗时 {time.ToStringEx()}");

                    FillTables(tables, GetLoadMode(option));

                    if (!option.Synchronize)
                    {
                        Text = Text.Split('-')[0].Trim();
                    }

                    if (_useGuide)
                    {
                        ShowTemplateGuide();
                    }
                }

                _hosting.HideProgress();
            }
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
                if (content is frmEditor editor)
                {
                    if (editor.FileName == fileItem.FilePath)
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
                if (content is frmEditor editor)
                {
                    if (editor.FileName == template.ConfigFileName)
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
                if (content is frmEditor editor)
                {
                    if (editor.FileName == fileName)
                    {
                        content.Activate();
                        return;
                    }
                }
            }

            var editform = new frmEditor(_hosting, fileName, category) { SaveAct = saveAct };
            editform.Show(dockMgr, DockState.Document);
        }

        private void OpenFileName(string fileName, string language = null)
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

            if (fileName.EndsWith(".dss", StringComparison.OrdinalIgnoreCase) ||
                fileName.EndsWith(".dso", StringComparison.OrdinalIgnoreCase) ||
                fileName.EndsWith(".dsr", StringComparison.OrdinalIgnoreCase))
            {
                _frmTable.Activate();

                Cursor = Cursors.WaitCursor;

                fileName = _frmTable.OpenFile(fileName);
                if (!string.IsNullOrEmpty(fileName))
                {
                    var title = Text.Split('-')[0].Trim();
                    Text = $"{title} - [{fileName}]";
                }

                Cursor = Cursors.Default;
            }
            else
            {
                var editform = new frmEditor(_hosting, fileName);
                if (!string.IsNullOrWhiteSpace(language))
                {
                    editform.Language = language;
                }
                editform.Show(dockMgr, DockState.Document);
            }
        }

        async void sourceProvider_Click(object sender, EventArgs e)
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

            await LoadSourceStruct(provider);
        }

        void templateProvider_Click(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            var parents = new List<ToolStripMenuItem>();

            var parent = menu.OwnerItem as ToolStripMenuItem;
            parents.Add(parent);

            while (parent != null && parent.OwnerItem != mnuTemplate)
            {
                parent = parent.OwnerItem as ToolStripMenuItem;
                parents.Add(parent);
            }

            RestoreTemplateMenuItem(mnuTemplate);

            parents.ForEach(p => p.ForeColor = Color.Blue);

            menu.Checked = true;

            var templateProvider = parent.Tag as ITemplateProvider;
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
                Config.Instance.Save();

                Cursor = Cursors.WaitCursor;

                ReBuildSchemaAndProfile();

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

                Cursor = Cursors.Default;
            }
        }

        private void RestoreTemplateMenuItem(ToolStripMenuItem item)
        {
            foreach (var child in item.DropDownItems)
            {
                if (child is ToolStripMenuItem subitem)
                {
                    subitem.Checked = false;

                    RestoreTemplateMenuItem(subitem);
                }
            }

            if (item.ForeColor != SystemColors.ControlText)
            {
                item.ForeColor = SystemColors.ControlText;
            }
        }

        void toolProvider_Click(object sender, EventArgs e)
        {
            if (!(sender is ToolStripMenuItem item) || item.Tag == null)
            {
                return;
            }

            if (item.Tag is IAsyncToolProvider asyncprovider)
            {
                if ((asyncprovider as IPreExecuteSupported)?.CanExecutable() ?? true)
                {
                    Processor.Run(this, c => asyncprovider.ExecuteAsync(c), onBackground: true);
                }
            }
            else if (item.Tag is IToolProvider provider)
            {
                if ((provider as IPreExecuteSupported)?.CanExecutable() ?? true)
                {
                    provider.Execute();
                }
            }
            else if (item.Tag is Tuple<IMultipleToolProvider, string, object> mprovider)
            {
                if ((mprovider.Item1 as IMultiplePreExecuteSupported)?.CanExecutable(mprovider.Item2, mprovider.Item3) ?? true)
                {
                    if (mprovider.Item1 is IAsyncMultipleToolProvider asyncmprovider)
                    {
                        Processor.Run(this, c => asyncmprovider.ExecuteAsync(mprovider.Item2, c, mprovider.Item3), onBackground: true);
                    }
                    else
                    {
                        mprovider.Item1.Execute(mprovider.Item2, mprovider.Item3);
                    }
                }
            }
        }

        private void BuildCode(List<PartitionDefinition> partitions = null)
        {
            if (_frmTable == null)
            {
                _hosting.ShowWarn("列表中空空如也，请从【数据源】菜单中选择或配置。");
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
            if (!tables.Any())
            {
                _hosting.ShowWarn("列表中空空如也，请从【数据源】菜单中选择或配置。");
                return;
            }

            var ckResult = Util.Validate(_hosting, tables);
            if (!ckResult.IsSuccess)
            {
                Util.ShowValidation(_hosting, ckResult, r => new frmShowValidation(r, ShowValidateResult).Show(_hosting.MainWindow));
                return;
            }

            var option = new TemplateOption();
            option.Template = template;
            option.DynamicAssemblies.AddRange(StaticUnity.DynamicAssemblies);
            option.Profile = _hosting.Profile;
            option.WriteToDisk = true;
            option.SkipWhenFileExists = Config.Instance.SkipWhenFileExists;
            option.Partitions = partitions;

            if (option.Partitions == null)
            {
                using (var frm = new frmPreBuild(_hosting) { Template = _hosting.Template })
                {
                    if (frm.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    option.Partitions = frm.Partitions;
                }
            }

            option.OutputDirectory = Config.Instance.OutputDirectory;

            var isCancellation = false;
            var time = Processor.Run(this, async calcelToken =>
                {
                    var result = await _hosting.TemplateProvider.GenerateFilesAsync(option, tables.ToList(), (s, p) =>
                        {
                            _hosting.ShowProgress($"{p}% 正在生成 {s} 的代码文件...", p);
                        }, calcelToken);

                    if (result?.HasError == true)
                    {
                        _hosting.ShowError("代码已生成完毕，但在生成过程中发生了错误，请到【输出】窗口查看。");
                    }
                    else if (result != null && !string.IsNullOrEmpty(Config.Instance.OutputDirectory))
                    {
                        Process.Start(Config.Instance.OutputDirectory);
                    }
                }, () => isCancellation = true, true);

            if (!isCancellation)
            {
                _hosting.ConsoleInfo($"代码已生成完毕，共耗时 {time.ToStringEx()}");

#if !DEBUG
                _hosting.Hit("Build_" + template.TId);
#endif

                var client = new HttpClient();
                var key = Uri.UnescapeDataString(Convert.ToBase64String(Encoding.ASCII.GetBytes(_hosting.TemplateProvider.Name + template.Id)));
                client.PostAsync(Consts.TemplateServerUrl + "/use/n/" + key, null);
                _generateCount++;
            }

            _hosting.HideProgress();
        }

        #region 菜单事件
        private void mnuProfileWnd_Click(object sender, EventArgs e)
        {
            OpenProfileForm();
        }

        private void mnuPropertyWnd_Click(object sender, EventArgs e)
        {
            OpenPropertyForm();
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

        private void mnuResWnd_Click(object sender, EventArgs e)
        {
            OpenResourceForm();
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
            if (dockMgr.ActiveContent is IChangeManager saveMgr)
            {
                saveMgr.SaveChanges();
            }
            else
            {
                Cursor = Cursors.WaitCursor;

                var fileName = _frmTable.SaveFile();
                if (!string.IsNullOrEmpty(fileName))
                {
                    var title = Text.Split('-')[0].Trim();
                    Text = $"{title} - [{fileName}]";
                }

                Cursor = Cursors.Default;
            }
        }

        private void mnuSaveAs_Click(object sender, EventArgs e)
        {
            if (dockMgr.ActiveContent is ISaveAsManager saveMgr)
            {
                saveMgr.SaveAs();
            }
            else
            {
                var fileName = _frmTable.SaveFile(true);
                if (!string.IsNullOrEmpty(fileName))
                {
                    var title = Text.Split('-')[0].Trim();
                    Text = $"{title} - [{fileName}]";
                }
            }
        }

        private void mnuSaveAll_Click(object sender, EventArgs e)
        {
            var isChangeCommon = false;
            var isChangeProfile = false;
            var isChangeSchema = false;

            foreach (DockContent content in dockMgr.Documents)
            {
                if (content is frmEditor editor)
                {
                    if (editor.SaveChanges(false))
                    {
                        if (editor.Category == CodeCategory.CommonExtension && !isChangeCommon)
                        {
                            isChangeCommon = true;
                        }
                        if (editor.Category == CodeCategory.ProfileExtension && !isChangeProfile)
                        {
                            isChangeProfile = true;
                        }
                        else if (editor.Category == CodeCategory.SchemaExtension && !isChangeSchema)
                        {
                            isChangeSchema = true;
                        }
                    }
                }
                else if (content is IChangeManager saveMgr)
                {
                    saveMgr.SaveChanges(false);
                }
            }

            if (isChangeCommon || isChangeProfile || isChangeSchema)
            {
                if (_hosting.ShowConfirm("代码文件已改变，是否立即编译?") != ShowMsgButton.Yes)
                {
                    return;
                }

                ReBuildSchemaAndProfile();
            }
        }

        private void mnuClose_Click(object sender, EventArgs e)
        {
            if (dockMgr.ActiveContent is ICloseManager form)
            {
                form.Close();
            }
            else
            {
                _frmTable?.CloseFile();
                Text = Text.Split('-')[0].Trim();
            }
        }

        private void mnuOption_Click(object sender, EventArgs e)
        {
            using (var frm = new frmOption(_hosting))
            {
                frm.OnPluginUpdated = () => ThreadHelper.Start(CheckPluginUpdate);

                frm.ShowDialog();

                if (frm.IsChanged(nameof(Config.TemplateGroup)))
                {
                    InitializeTemplateMenus();
                }
                if (frm.IsChanged(nameof(Config.FontSize)))
                {
                    var documents = dockMgr.DocumentsToArray();

                    foreach (IDockContent content in documents)
                    {
                        if (content is frmEditor editor)
                        {
                            editor.SetFontSize(Config.Instance.FontSize);
                        }
                    }
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
            using (var frm = new frmAbout(_hosting))
            {
                frm.ShowDialog();
            }
        }

        private void mnuDonate_Click(object sender, EventArgs e)
        {
            using (var frm = new frmDonate())
            {
                frm.ShowDialog();
            }
        }

        private void mnuTopic_Click(object sender, EventArgs e)
        {
            Process.Start(WebHelper.GetRedirectUrl(_hosting, "/codebuilder/docs"));
        }

        private void mnuGuide_Click(object sender, EventArgs e)
        {
            ShowSourceGuide();
        }

        private void mnuUpdate_Click(object sender, EventArgs e)
        {
            if (!File.Exists("AutoUpdate.exe"))
            {
                _hosting.ShowError("无法进行更新，找不到程序 AutoUpdate.exe。");
                return;
            }

            var startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = true;
            startInfo.WorkingDirectory = Environment.CurrentDirectory;
            startInfo.FileName = "AutoUpdate.exe";
            startInfo.Arguments = "/T";
            startInfo.Verb = "runas";

            Process.Start(startInfo);
        }

        private void mnuFeedback_Click(object sender, EventArgs e)
        {
            var version = GetType().Assembly.GetName().Version;

            Process.Start(WebHelper.GetRedirectUrl(_hosting, "/codebuilder/feedback?version=" + version.ToString()));
        }

        private void mnuTemplateRops_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.github.com/faib920/codebuilder-templates");
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
            var handler = _hosting.ServiceProvider.TryGetService<FormCloseHandler>();
            foreach (var c in dockMgr.Contents)
            {
                if (c is IChangeManager changeMgr)
                {
                    handler.TryAddClosingForm((Form)c);
                }
            }

            if (handler.TryAddClosingForm(this))
            {
                e.Cancel = true;
            }

            base.OnClosing(e);
        }

        private void dockMgr_ContentAdded(object sender, DockContentEventArgs e)
        {
            if (e.Content is DockContent content && content is IContextMenuManager)
            {
                content.TabPageContextMenuStrip = contextMenuStrip1;
            }

            InitializeAddedMainMenuItems(e.Content);
        }

        private void dockMgr_ContentRemoved(object sender, DockContentEventArgs e)
        {
            if (e.Content is IMainMenuManager menuMgr)
            {
                var count = dockMgr.Contents.Where(c => c != e.Content).OfType<IMainMenuManager>().Count(c => c.MenuText.Equals(menuMgr.MenuText));
                if (count == 0)
                {
                    RemoveMainMenuItems(menuMgr);
                }
            }

            RemoveAddedMainMenuItems(e.Content);
        }

        private void dockMgr_ActiveContentChanged(object sender, EventArgs e)
        {
            InitializeActivedMainMenuItems(dockMgr.ActiveDocument);
            InitializeContextMenu(dockMgr.ActiveDocument);
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (dockMgr.ActiveDocument is DockContent content)
            {
                toolStripMenuItem5.Checked = !content.CloseButtonVisible;
            }
        }

        private void ShowValidateResult(ValidateEntry ve)
        {
            if (ve.Object is Profile profile)
            {
                OpenProfileForm();
                _frmProfile?.SelectProperty(ve.PropertyName);
            }
            else if (ve.Object is Table table)
            {
                _frmTable.SelectTable(table);
                OpenPropertyForm();
                _frmProperty.SelectProperty(ve.PropertyName);
            }
            else if (ve.Object is Column column)
            {
                _frmTable.SelectColumn(column);
                OpenPropertyForm();
                _frmProperty.SelectProperty(ve.PropertyName);
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
                this.Invoke(new Action(() =>
                {
                    var items = mgr.GetContextMenuItems();
                    if (items.Any())
                    {
                        var sep = new ToolStripSeparator { Tag = "Cust" };
                        contextMenuStrip1.Items.Add(sep);

                        foreach (var c in items)
                        {
                            c.Tag = "Cust";
                            contextMenuStrip1.Items.Add(c);
                        }
                    }
                }));
            }
        }

        private void InitializeActivedMainMenuItems(IDockContent content)
        {
            for (var i = menuStrip1.Items.Count - 1; i >= 0; i--)
            {
                if (menuStrip1.Items[i] is CustomMenuItem m && m.DisplayStyle == DisplayStyle.Actived)
                {
                    menuStrip1.Items.RemoveAt(i);
                }
            }

            if (content is IMainMenuManager mgr && mgr.DisplayStyle == DisplayStyle.Actived)
            {
                var index = FindMainMenuItem(mgr.InsertedMainMenuKey);
                if (index != -1)
                {
                    var menuItem = new CustomMenuItem(mgr.MenuText) { DisplayStyle = DisplayStyle.Actived };
                    menuStrip1.Items.Insert(index, menuItem);

                    foreach (var item in mgr.GetMenuItems())
                    {
                        menuItem.DropDownItems.Add(item);
                    }
                }
            }
        }

        private void InitializeAddedMainMenuItems(IDockContent content)
        {
            if (content is IMainMenuManager mgr && mgr.DisplayStyle == DisplayStyle.Added)
            {
                for (var i = menuStrip1.Items.Count - 1; i >= 0; i--)
                {
                    if (menuStrip1.Items[i] is CustomMenuItem m && m.DisplayStyle == DisplayStyle.Added && m.Text == mgr.MenuText)
                    {
                        return;
                    }
                }

                var index = FindMainMenuItem(mgr.InsertedMainMenuKey);
                if (index != -1)
                {
                    var menuItem = new CustomMenuItem(mgr.MenuText) { DisplayStyle = DisplayStyle.Added };
                    menuStrip1.Items.Insert(index, menuItem);

                    foreach (var item in mgr.GetMenuItems())
                    {
                        menuItem.DropDownItems.Add(item);
                    }
                }
            }
        }

        private void RemoveAddedMainMenuItems(IDockContent content)
        {
            if (content is IMainMenuManager mgr && mgr.DisplayStyle == DisplayStyle.Added)
            {
                for (var i = menuStrip1.Items.Count - 1; i >= 0; i--)
                {
                    if (menuStrip1.Items[i] is CustomMenuItem m && m.DisplayStyle == DisplayStyle.Added && m.Text == mgr.MenuText)
                    {
                        menuStrip1.Items.RemoveAt(i);
                        return;
                    }
                }
            }
        }

        private int FindMainMenuItem(string key)
        {
            for (var i = menuStrip1.Items.Count - 1; i >= 0; i--)
            {
                if ((key == MainMenuKeys.File && menuStrip1.Items[i] == mnuFile) ||
                    (key == MainMenuKeys.Source && menuStrip1.Items[i] == mnuSource) ||
                    (key == MainMenuKeys.Template && menuStrip1.Items[i] == mnuSource) ||
                    (key == MainMenuKeys.Tool && menuStrip1.Items[i] == mnuTool) ||
                    (key == MainMenuKeys.Window && menuStrip1.Items[i] == mnuWindow) ||
                    (key == MainMenuKeys.Help && menuStrip1.Items[i] == mnuHelp))
                {
                    return i;
                }
            }

            return -1;
        }

        private void RemoveMainMenuItems(IMainMenuManager mgr)
        {
            for (var i = menuStrip1.Items.Count - 1; i >= 0; i--)
            {
                if (menuStrip1.Items[i].Tag is string str && str == mgr.MenuText)
                {
                    menuStrip1.Items.RemoveAt(i);
                }
            }
        }

        private void ShowSyncStatusPanels(int a, int m, int r)
        {
            statusStrip1.Items.RemoveByKey("SyncA");
            statusStrip1.Items.RemoveByKey("SyncM");
            statusStrip1.Items.RemoveByKey("SyncR");

            if (r > 0)
            {
                var label = new ToolStripStatusLabel
                {
                    Name = "SyncR",
                    Text = "移除:" + r.ToString(),
                    ToolTipText = "移除项",
                    Visible = true,
                    Width = 30,
                    BackColor = _filterOpts[frmTable.UpdateFlag.Removed] ? Consts.RemovedColor1 : Consts.RemovedColor,
                };
                label.Click += (o, e) =>
                {
                    var l = (ToolStripStatusLabel)o;
                    if (l.BackColor == Consts.RemovedColor)
                    {
                        _filterOpts[frmTable.UpdateFlag.Removed] = true;
                        l.BackColor = Consts.RemovedColor1;
                        _frmTable?.SetFilterFlag(frmTable.UpdateFlag.Removed, true);
                    }
                    else
                    {
                        _filterOpts[frmTable.UpdateFlag.Removed] = false;
                        l.BackColor = Consts.RemovedColor;
                        _frmTable?.SetFilterFlag(frmTable.UpdateFlag.Removed, false);
                    }
                };
                statusStrip1.Items.Insert(0, label);
            }
            else
            {
                _filterOpts[frmTable.UpdateFlag.Removed] = false;
            }
            if (m > 0)
            {
                var label = new ToolStripStatusLabel
                {
                    Name = "SyncM",
                    Text = "修改:" + m.ToString(),
                    ToolTipText = "修改项",
                    Visible = true,
                    Width = 30,
                    BackColor = _filterOpts[frmTable.UpdateFlag.Modified] ? Consts.ModifiedColor1 : Consts.ModifiedColor,
                };
                label.Click += (o, e) =>
                {
                    var l = (ToolStripStatusLabel)o;
                    if (l.BackColor == Consts.ModifiedColor)
                    {
                        _filterOpts[frmTable.UpdateFlag.Modified] = true;
                        l.BackColor = Consts.ModifiedColor1;
                        _frmTable?.SetFilterFlag(frmTable.UpdateFlag.Modified, true);
                    }
                    else
                    {
                        _filterOpts[frmTable.UpdateFlag.Modified] = false;
                        l.BackColor = Consts.ModifiedColor;
                        _frmTable?.SetFilterFlag(frmTable.UpdateFlag.Modified, false);
                    }
                };
                statusStrip1.Items.Insert(0, label);
            }
            else
            {
                _filterOpts[frmTable.UpdateFlag.Modified] = false;
            }
            if (a > 0)
            {
                var label = new ToolStripStatusLabel
                {
                    Name = "SyncA",
                    Text = "新增:" + a.ToString(),
                    ToolTipText = "新增项",
                    Visible = true,
                    Width = 30,
                    BackColor = _filterOpts[frmTable.UpdateFlag.Added] ? Consts.AddedColor1 : Consts.AddedColor,
                };
                label.Click += (o, e) =>
                {
                    var l = (ToolStripStatusLabel)o;
                    if (l.BackColor == Consts.AddedColor)
                    {
                        _filterOpts[frmTable.UpdateFlag.Added] = true;
                        l.BackColor = Consts.AddedColor1;
                        _frmTable?.SetFilterFlag(frmTable.UpdateFlag.Added, true);
                    }
                    else
                    {
                        _filterOpts[frmTable.UpdateFlag.Added] = false;
                        l.BackColor = Consts.AddedColor;
                        _frmTable?.SetFilterFlag(frmTable.UpdateFlag.Added, false);
                    }
                };
                statusStrip1.Items.Insert(0, label);
            }
            else
            {
                _filterOpts[frmTable.UpdateFlag.Added] = false;
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

            while (true)
            {
                HttpResponseMessage response = null;
                try
                {
                    response = await client.GetAsync(Consts.PluginServerUrl + "?page=" + page + "&rows=20&filter=source|template|tool");
                }
                catch
                {
                    break;
                }

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return;
                }

                var content = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<OnlinePluginData>(content);
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

                page++;
            }

            this.Invoke(new Action(() =>
            {
                statusStrip1.Items.RemoveByKey("ssPlugin");
            }));

            if (updateCount > 0)
            {
                var label = new ToolStripStatusLabel();
                label.IsLink = true;
                label.Name = "ssPlugin";
                label.Visible = true;
                label.LinkBehavior = LinkBehavior.HoverUnderline;
                label.Text = "插件更新(" + updateCount + ")";
                label.ToolTipText = "检查到 " + updateCount + " 个插件可更新";
                label.Click += (o, e) =>
                {
                    var frm = new frmPluginShop(_hosting);
                    frm.OnUpdated = () => ThreadHelper.Start(CheckPluginUpdate);
                    frm.ShowDialog();
                };

                this.Invoke(new Action(() =>
                {
                    statusStrip1.Items.Add(label);
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
            var updateItems = new List<string>();

            while (true)
            {
                HttpResponseMessage response = null;
                try
                {
                    response = await client.GetAsync(Consts.TemplateServerUrl + "?order=0&page=" + page + "&rows=20");
                }
                catch
                {
                    break;
                }

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return;
                }

                var content = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<OnlineTemplateData>(content);
                foreach (var tmp in data.Items)
                {
                    if (TemplateUnity.TryGet(tmp.Category, tmp.Code, out TemplateDefinition definition))
                    {
                        if (definition.Version < tmp.Version)
                        {
                            updateItems.Add(tmp.Code);
                        }
                    }
                }

                if (data.LastPage)
                {
                    break;
                }

                page++;
            }

            this.Invoke(new Action(() =>
            {
                statusStrip1.Items.RemoveByKey("ssTemp");
            }));

            if (updateItems.Count > 0)
            {
                var label = new ToolStripStatusLabel();
                label.IsLink = true;
                label.Name = "ssTemp";
                label.Visible = true;
                label.Tag = updateItems;
                label.LinkBehavior = LinkBehavior.HoverUnderline;
                label.Text = "模板更新(" + updateItems.Count + ")";
                label.ToolTipText = "检查到 " + updateItems.Count + " 个模板可更新";
                label.Click += (o, e) =>
                {
                    var frm = new frmTemplateShop(_hosting, (o as ToolStripStatusLabel).Tag as List<string>);
                    frm.OnUpdated = () => ThreadHelper.Start(CheckTemplateUpdate);

                    frm.ShowDialog();
                };

                this.Invoke(new Action(() =>
                {
                    statusStrip1.Items.Add(label);
                }));
            }
        }

        private void ShowSourceGuide(bool first = false)
        {
            _frmGuide = new frmGuide(_hosting, first);
            _frmGuide.CloseAct = () =>
            {
                _useGuide = false;
                _frmGuide = null;
            };
            _frmGuide.SourceAct = (p) =>
            {
                foreach (ToolStripMenuItem item in mnuSource.DropDownItems)
                {
                    if (item.Name == p)
                    {
                        sourceProvider_Click(item, new EventArgs());
                    }
                }
            };
            _frmGuide.TemplateAct = (p, t) =>
            {
                foreach (ToolStripItem item in mnuTemplate.DropDownItems)
                {
                    if (item.Tag != null && item.Name == p)
                    {
                        foreach (ToolStripMenuItem sitem in ((ToolStripMenuItem)item).DropDownItems)
                        {
                            if (sitem.Tag is TemplateDefinition tmp && tmp.TId == t)
                            {
                                templateProvider_Click(sitem, new EventArgs());
                            }

                            if (Config.Instance.TemplateGroup != Config.TemplateGroupStyle.None)
                            {
                                foreach (ToolStripMenuItem sitem1 in sitem.DropDownItems)
                                {
                                    if (sitem1.Tag is TemplateDefinition tmp1 && tmp1.TId == t)
                                    {
                                        templateProvider_Click(sitem1, new EventArgs());
                                    }
                                }
                            }
                        }
                    }
                }
            };
            _frmGuide.PropertyChangeAct += () =>
            {
                if (_frmTable != null)
                {
                    _frmTable.ApplyProfile();
                }
            };
            _frmGuide.BuildAct = (pl) =>
            {
                _frmGuide.Close();
                _frmGuide = null;

                BuildCode(pl);

                if (first)
                {
                    mnuDonate_Click(null, null);
                }
            };

            _frmGuide.Show(1, this);

            if (!Config.Instance.Guided)
            {
                Config.Instance.Guided = true;
                Config.Instance.Save();
            }

            _useGuide = true;
        }

        private void ShowTemplateGuide()
        {
            if (_frmGuide == null)
            {
                return;
            }
            _frmGuide.Show(2, this);
        }

        private void CheckVisitData()
        {
            byte[] WriteBinaryData(int offset, DateTime date)
            {
                var bytes = BitConverter.GetBytes(date.AddDays(Math.Pow(2, offset)).ToBinary());
                var wbytes = new byte[bytes.Length + 4];
                Array.Copy(bytes, 0, wbytes, 4, bytes.Length);
                bytes = BitConverter.GetBytes(offset);
                Array.Copy(bytes, 0, wbytes, 0, bytes.Length);
                RegistryHelper.SetValue("VisitData", wbytes, RegistryValueKind.Binary);
                return wbytes;
            }

            (int Offset, DateTime Date) ParseBinaryData()
            {
                try
                {
                    var bytes = RegistryHelper.GetValue<byte[]>("VisitData");
                    var tbytes = new byte[4];
                    Array.Copy(bytes, tbytes, 4);
                    var offset = BitConverter.ToInt32(tbytes, 0);
                    var datebits = BitConverter.ToInt64(bytes, 4);
                    var date = DateTime.FromBinary(datebits);
                    return (offset, date);
                }
                catch
                {
                    return (-1, DateTime.MinValue);
                }
            }

            if (!RegistryHelper.ContainsKey("VisitData"))
            {
                WriteBinaryData(2, DateTime.Today);
            }
            else
            {
                var b = ParseBinaryData();

                if (b.Offset == -1)
                {
                    WriteBinaryData(1, DateTime.Today);
                }
                else if (DateTime.Today >= b.Date)
                {
                    WriteBinaryData(b.Offset + 1, DateTime.Today);

                    if (b.Offset == 3)
                    {
                        Invoke(new Action(() =>
                        {
                            using (var frm = new frmQuestionnaire())
                            {
                                frm.ShowDialog();
                            }
                        }));
                    }
                    else
                    {
                        Invoke(new Action(() =>
                        {
                            mnuDonate_Click(null, null);
                        }));
                    }
                }
            }
        }

        void INotifyWindow.ShowOnNotifyIcon()
        {
            notifyIcon1.Text = "CodeBuilder";
            notifyIcon1.Visible = true;
            notifyIcon1.ShowBalloonTip(5000, "提示", "任务已转到后台处理，可双击托盘图标恢复主界面。", ToolTipIcon.Info);
            Hide();
        }

        void INotifyWindow.RestoreWindow()
        {
            Show();
            Activate();
            Processor.TryRestore();
            notifyIcon1.Visible = false;
            _hosting.HideProgress();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            (this as INotifyWindow).RestoreWindow();
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

        private void mnuLogin_Click(object sender, EventArgs e)
        {
            if (_hosting.IsAuthorized)
            {
                return;
            }

            using (var frm = new frmLogin(_hosting))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    mnuLogin.Text = frm.UserName;

                    Config.Instance.AccessToken = frm.AccessToken;
                    Config.Instance.UserAccount = frm.UserAccount;
                    Config.Instance.Save();
                    SetLoginMenu(frm.UserName);

                    var eventBus = _hosting.ServiceProvider.TryGetService<IEventBusHandler>();
                    if (eventBus != null)
                    {
                        eventBus.Publish("Login");
                    }
                }
            }
        }

        private async void ValidAccessToken()
        {
            if (!string.IsNullOrEmpty(Config.Instance.AccessToken))
            {
                LoginData identity = null;
                try
                {
                    identity = await IdentityHelper.ValidateAsync(Config.Instance.AccessToken);
                }
                catch
                {
                    return;
                }

                if (identity != null)
                {
                    if (identity.access_token != Config.Instance.AccessToken)
                    {
                        Config.Instance.AccessToken = identity.access_token;
                        Config.Instance.Save();
                    }

                    SetLoginMenu(identity.name);
                }
                else
                {
                    Config.Instance.AccessToken = string.Empty;
                    Config.Instance.Save();
                }
            }
        }

        private void SetLoginMenu(string name)
        {
            mnuLogin.Text = name;
            _hosting.IsAuthorized = true;

            mnuLogin.DropDownItems.Add(new ToolStripMenuItem
            {
                Text = "个人中心"
            });
            mnuLogin.DropDownItems.Add(new ToolStripMenuItem
            {
                Text = "退出账号"
            });
            mnuLogin.DropDownItems[0].Click += (o, e) =>
            {
                Process.Start($"{WebHelper.HomeUrl}/user/accept?token={Uri.EscapeDataString(WebHelper.Encrypt(Config.Instance.AccessToken))}");
            };
            mnuLogin.DropDownItems[1].Click += (o, e) =>
            {
                Config.Instance.AccessToken = string.Empty;
                Config.Instance.Save();
                mnuLogin.Text = "登录...";
                mnuLogin.DropDownItems.Clear();
                _hosting.IsAuthorized = false;

                var eventBus = _hosting.ServiceProvider.TryGetService<IEventBusHandler>();
                if (eventBus != null)
                {
                    eventBus.Publish("Logout");
                }
            };
        }

        private LoadMode GetLoadMode(SourceOption option)
        {
            if (option.Synchronize)
            {
                return LoadMode.Synchronize;
            }
            else if (option.Append)
            {
                return LoadMode.Append;
            }

            return LoadMode.Default;
        }
    }
}
