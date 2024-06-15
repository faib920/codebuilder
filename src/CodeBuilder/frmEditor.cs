// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using CodeBuilder.Core.Forms;
using CodeBuilder.Core.Template;
using Fireasy.Common.Extensions;
using ICSharpCode.TextEditor.Actions;
using ICSharpCode.TextEditor.Document;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CodeBuilder
{
    public partial class frmEditor : ChangedFormBase, IContextMenuManager, IMainMenuManager
    {
        private string _caption = "未命名";
        private readonly frmFindAndReplace _findForm;
        private readonly IDevHosting _hosting;

        public frmEditor(IDevHosting hosting, CodeCategory category = CodeCategory.None)
        {
            InitializeComponent();
            Category = category;
            _hosting = hosting;
            _findForm = new frmFindAndReplace(_hosting);

            SetFontSize(Config.Instance.FontSize);
        }

        public frmEditor(IDevHosting hosting, string fileName, CodeCategory category = CodeCategory.None)
            : this(hosting, category)
        {
            FileName = fileName;
        }

        public string FileName { get; set; }

        public CodeCategory Category { get; set; }

        public TemplateFile TemplateFile { get; set; }

        public ProjectTemplate Template { get; set; }

        public GeneratePartitionResult GenerateResult { get; set; }

        public Action SaveAct { get; set; }

        public string Language { get; set; }

        private void frmEditor_Load(object sender, EventArgs e)
        {
            txtEditor.TextEditorProperties.CaretLine = true;
            txtEditor.TextEditorProperties.ConvertTabsToSpaces = true;

            if (!string.IsNullOrEmpty(FileName))
            {
                OpenFile();
            }
            else if (TemplateFile != null)
            {
                FromTemplateItem();
                contextMenuStrip1.Items.Add(new ToolStripSeparator());
                var item = new FieldCacheMenuItem(_hosting) { Name = "mnuInsert", Text = "插入", Image = Properties.Resources.insert };
                item.OnFieldInsert += (s) =>
                    {
                        txtEditor.ActiveTextAreaControl.TextArea.InsertString(s);
                    };
                contextMenuStrip1.Items.Add(item);

                if (!TemplateFile.IsPublic)
                {
                    contextMenuStrip1.Items.Add(new ToolStripSeparator());
                    var item1 = new ToolStripMenuItem("校验...", Properties.Resources.check, ValidateTemplate) { Name = "Test" };
                    contextMenuStrip1.Items.Add(item1);
                }
            }
            else if (Template != null)
            {
                FromTemplate();
            }
            else if (GenerateResult != null)
            {
                FromGenerateResult();
            }

            txtEditor.TextChanged += new System.EventHandler(this.txtEditor_TextChanged);
        }

        private void OpenFile()
        {
            var info = new FileInfo(FileName);
            if (!string.IsNullOrWhiteSpace(Language))
            {
                SetSyntax(Language);
            }
            else
            {
                var language = GetLanguageByExtension(info.Extension);
                SetSyntax(language);
            }

            if (!File.Exists(FileName))
            {
                File.Create(FileName).Close();
            }

            txtEditor.Text = File.ReadAllText(FileName);

            _caption = Text = info.Name;
        }

        private void FromTemplate()
        {
            SetSyntax(Template.Syntax);
            txtEditor.Text = File.ReadAllText(Template.FileName);
        }

        private void FromTemplateItem()
        {
            FileName = TemplateFile.FilePath;
            var info = new FileInfo(FileName);
            SetSyntax(TemplateFile.Language);
            txtEditor.Text = File.ReadAllText(FileName);

            _caption = Text = info.Name;
        }

        private void FromGenerateResult()
        {
            txtEditor.Text = GenerateResult.Content;
            SetSyntax(GenerateResult.Partition.Syntax);
            _caption = Text = "生成：" + GenerateResult.Partition.Name;
        }

        public override bool SaveChanges(bool notify = true)
        {
            if (!IsChanged && !string.IsNullOrEmpty(FileName))
            {
                return false;
            }

            if (string.IsNullOrEmpty(FileName))
            {
                if (!ShowSaveDialog())
                {
                    return false;
                }
            }

            try
            {
                Util.TryOperateFile(FileName, () => File.WriteAllText(FileName, txtEditor.Text, Encoding.UTF8));
                var info = new FileInfo(FileName);
                _caption = Text = info.Name;

                if (notify)
                {
                    SaveAct?.Invoke();
                }

                ProcessChanged(false);
                return true;
            }
            catch (Exception exp)
            {
                _hosting.ShowError("保存文件失败。" + exp.Message);
            }

            return false;
        }

        public void SaveAs()
        {
            if (!ShowSaveDialog())
            {
                return;
            }

            var info = new FileInfo(FileName);
            try
            {
                Util.TryOperateFile(FileName, () => File.WriteAllText(FileName, txtEditor.Text, Encoding.UTF8));
                _caption = Text = info.Name;
                ProcessChanged(false);
            }
            catch (Exception exp)
            {
                _hosting.ShowError("保存文件失败。" + exp.Message);
            }
        }

        /// <summary>
        /// 初始插入菜单
        /// </summary>
        public void InitializeInsertMenus()
        {
            var menuItem = contextMenuStrip1.Items.Find("mnuInsert", false).FirstOrDefault() as FieldCacheMenuItem;
            menuItem?.InitializeMenus();
        }

        private bool ShowSaveDialog()
        {
            using (var dialog = new SaveFileDialog())
            {
                if (Template != null)
                {
                    dialog.Filter = FileTypeHelper.GetFilter(GetExtensionByLanguage(Template.Syntax));
                }
                else if (!string.IsNullOrEmpty(FileName))
                {
                    var info1 = new FileInfo(FileName);
                    dialog.InitialDirectory = info1.DirectoryName;
                    dialog.Filter = FileTypeHelper.GetFilter(info1.Extension);
                }

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return false;
                }

                FileName = dialog.FileName;
                return true;
            }
        }

        private string GetLanguageByExtension(string extension)
        {
            foreach (DictionaryEntry kvp in HighlightingManager.Manager.HighlightingDefinitions)
            {
                if (kvp.Value is IHighlightingStrategy strategy)
                {
                    if (strategy.Extensions.Contains(extension.ToLower()))
                    {
                        return strategy.Name;
                    }
                }
                else
                {
                    var entry = (DictionaryEntry)kvp.Value;
                    if (entry.Key is SyntaxMode mode && mode.Extensions.Contains(extension.ToLower()))
                    {
                        return mode.Name;
                    }
                }
            }

            return "Default";
        }

        private string GetExtensionByLanguage(string language)
        {
            var strategy = HighlightingManager.Manager.FindHighlighter(language);
            if (strategy != null)
            {
                return strategy.Extensions[0];
            }

            return string.Empty;
        }

        private void SetSyntax(string language)
        {
            txtEditor.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy(language);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (IsChanged)
            {
                var handler = _hosting.ServiceProvider.TryGetService<FormCloseHandler>();
                if (handler.TryAddClosingForm(this))
                {
                    e.Cancel = true;
                }
            }

            base.OnClosing(e);
        }

        private void txtEditor_TextChanged(object sender, EventArgs e)
        {
            ProcessChanged(true);
        }

        private bool HaveSelection()
        {
            return txtEditor.ActiveTextAreaControl.TextArea.SelectionManager.HasSomethingSelected;
        }

        private void DoEditAction(IEditAction action)
        {
            if (action != null)
            {
                var area = txtEditor.ActiveTextAreaControl.TextArea;
                txtEditor.BeginUpdate();
                try
                {
                    lock (txtEditor.Document)
                    {
                        action.Execute(area);
                        if (area.SelectionManager.HasSomethingSelected && area.AutoClearSelection /*&& caretchanged*/)
                        {
                            if (area.Document.TextEditorProperties.DocumentSelectionMode == DocumentSelectionMode.Normal)
                            {
                                area.SelectionManager.ClearSelection();
                            }
                        }
                    }
                }
                finally
                {
                    txtEditor.EndUpdate();
                    area.Caret.UpdateCaretPosition();
                }
            }
        }

        private void mnuUndo_Click(object sender, EventArgs e)
        {
            DoEditAction(new Undo());
        }

        private void mnuRedo_Click(object sender, EventArgs e)
        {
            DoEditAction(new Redo());
        }

        private void mnuCopy_Click(object sender, EventArgs e)
        {
            if (HaveSelection())
            {
                DoEditAction(new Copy());
            }
        }

        private void mnuCut_Click(object sender, EventArgs e)
        {
            if (HaveSelection())
            {
                DoEditAction(new Cut());
            }
        }

        private void mnuPaste_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                DoEditAction(new Paste());
            }
        }

        private void mnuFind_Click(object sender, EventArgs e)
        {
            _findForm.ShowFor(txtEditor, false);
        }

        private void mnuReplace_Click(object sender, EventArgs e)
        {
            _findForm.ShowFor(txtEditor, true);
        }

        private void LocationFile()
        {
            Process.Start("explorer", "/select,\"" + FileName + "\"");
        }

        string IMainMenuManager.MenuText => "编辑(&E)";

        string IMainMenuManager.InsertedMainMenuKey => MainMenuKeys.Source;

        IEnumerable<ToolStripItem> IMainMenuManager.GetMenuItems()
        {
            yield return new ToolStripMenuItem("撤消", Properties.Resources.undo, mnuUndo_Click) { ShortcutKeys = Keys.Control | Keys.Z };
            yield return new ToolStripMenuItem("重做", Properties.Resources.undo, mnuRedo_Click) { ShortcutKeys = Keys.Control | Keys.Y };
            yield return new ToolStripSeparator();
            yield return new ToolStripMenuItem("复制", Properties.Resources.copy, mnuCopy_Click) { ShortcutKeys = Keys.Control | Keys.C };
            yield return new ToolStripMenuItem("剪切", Properties.Resources.cut, mnuCut_Click) { ShortcutKeys = Keys.Control | Keys.X };
            yield return new ToolStripMenuItem("粘贴", Properties.Resources.cut, mnuPaste_Click) { ShortcutKeys = Keys.Control | Keys.P };
            yield return new ToolStripSeparator();
            yield return new ToolStripMenuItem("查找", Properties.Resources.find, mnuFind_Click) { ShortcutKeys = Keys.Control | Keys.F };
            yield return new ToolStripMenuItem("替换", Properties.Resources.replace, mnuReplace_Click) { ShortcutKeys = Keys.Control | Keys.R };

            if (TemplateFile != null)
            {
                yield return new ToolStripSeparator();
                var item = new FieldCacheMenuItem(_hosting) { Text = "插入", Image = Properties.Resources.insert };
                item.OnFieldInsert += (s) =>
                {
                    txtEditor.ActiveTextAreaControl.TextArea.InsertString(s);
                };
                yield return item;

                if (!TemplateFile.IsPublic)
                {
                    yield return new ToolStripSeparator();
                    var item1 = new ToolStripMenuItem("校验...", Properties.Resources.check, ValidateTemplate);
                    yield return item1;
                }
            }
        }

        IEnumerable<ToolStripItem> IContextMenuManager.GetContextMenuItems()
        {
            if (Category == CodeCategory.None)
            {
                if (!string.IsNullOrEmpty(FileName))
                {
                    yield return new ToolStripMenuItem("保存", Properties.Resources.save1, (o, e) => SaveChanges(true)) { Name = "mnuSave" };
                    yield return new ToolStripMenuItem("打开所在的文件夹", null, (o, e) => LocationFile()) { Name = "mnuLocation" };
                }

                yield break;
            }

            yield return new ToolStripMenuItem("保存", Properties.Resources.save1, (o, e) => SaveChanges(true)) { Name = "mnuSave" };
            if (GenerateResult == null)
            {
                yield return new ToolStripMenuItem("打开所在的文件夹", null, (o, e) => LocationFile()) { Name = "mnuLocation" };
            }
            yield return new ToolStripMenuItem("帮助", Properties.Resources.help, (o, e) => GotoHelp(), Keys.F1) { Name = "mnuHelp" };
        }

        DisplayStyle IMainMenuManager.DisplayStyle => DisplayStyle.Actived;

        private void GotoHelp()
        {
            switch (Category)
            {
                case CodeCategory.TemplateFile:
                    Process.Start(WebHelper.GetRedirectUrl(_hosting, "/docs/codebuilder-template-edit"));
                    break;
                case CodeCategory.TemplateDefnition:
                    Process.Start(WebHelper.GetRedirectUrl(_hosting, "/docs/codebuilder-template-definition-edit"));
                    break;
                case CodeCategory.CommonExtension:
                case CodeCategory.ProfileExtension:
                case CodeCategory.SchemaExtension:
                    Process.Start(WebHelper.GetRedirectUrl(_hosting, "/docs/codebuilder-extension-edit"));
                    break;
            }
        }

        private void ValidateTemplate(object o, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            _hosting.ValidateAsync(TemplateFile, txtEditor.Text).GetAwaiter().GetResult();

            Cursor = Cursors.Default;
        }

        public void SetFontSize(int fontSize)
        {
            txtEditor.Font = new System.Drawing.Font(txtEditor.Font.FontFamily, fontSize);
        }
    }
}
