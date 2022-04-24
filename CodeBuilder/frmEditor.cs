// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using CodeBuilder.Core.Template;
using ICSharpCode.TextEditor.Actions;
using ICSharpCode.TextEditor.Document;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace CodeBuilder
{
    public partial class frmEditor : DockContent, IClosableDockManaged, IChangeManager
    {
        private string _caption = "未命名";
        private bool _isChanged = false;
        private readonly frmFindAndReplace _findForm;
        private readonly IDevHosting _hosting;

        public frmEditor(IDevHosting hosting)
        {
            InitializeComponent();
            _hosting = hosting;
            _findForm = new frmFindAndReplace(_hosting);
        }

        public frmEditor(IDevHosting hosting, string fileName)
            : this(hosting)
        {
            FileName = fileName;
        }

        public string FileName { get; set; }

        public TemplateFile TemplateFile { get; set; }

        public ProjectTemplate Template { get; set; }

        public GenerateResult GenerateResult { get; set; }

        public Action SaveAct { get; set; }

        public string Language { get; set; }

        bool IChangeManager.IsChanged => _isChanged;

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
                var item = new FieldCacheMenuItem { Text = "插入" };
                item.OnFieldInsert += (s) =>
                    {
                        txtEditor.ActiveTextAreaControl.TextArea.InsertString(s);
                    };
                contextMenuStrip1.Items.Add(item);
            }
            else if (Template != null)
            {
                FromTemplate();
            }
            else if (GenerateResult != null)
            {
                FromGenerateResult();
                panel1.Visible = false;
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

            txtEditor.Text = File.ReadAllText(FileName);

            _caption = Text = info.Name;
            lblFileName.Text = FileName;
        }

        private void FromTemplate()
        {
            SetSyntax(Template.Syntax);
            txtEditor.Text = File.ReadAllText(Template.FileName);
        }

        private void FromTemplateItem()
        {
            FileName = TemplateFile.FileName;
            var info = new FileInfo(FileName);
            SetSyntax(TemplateFile.Language);
            txtEditor.Text = File.ReadAllText(FileName);

            _caption = Text = info.Name;
            lblFileName.Text = FileName;
        }

        private void FromGenerateResult()
        {
            txtEditor.Text = GenerateResult.Content;
            SetSyntax(GenerateResult.Partition.Syntax);
            _caption = Text = GenerateResult.Partition.Name;
        }

        public void SaveFile()
        {
            if (!_isChanged && !string.IsNullOrEmpty(FileName))
            {
                return;
            }

            if (string.IsNullOrEmpty(FileName))
            {
                if (!ShowSaveDialog())
                {
                    return;
                }
            }

            try
            {
                File.WriteAllText(FileName, txtEditor.Text, Encoding.UTF8);
                var info = new FileInfo(FileName);
                _caption = Text = info.Name;
                lblFileName.Text = FileName;

                SaveAct?.Invoke();

                _isChanged = false;
            }
            catch (Exception exp)
            {
                _hosting.ShowError("保存文件失败。" + exp.Message);
            }
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
                File.WriteAllText(FileName, txtEditor.Text, Encoding.UTF8);
                _caption = Text = info.Name;
                lblFileName.Text = FileName;
                _isChanged = false;
            }
            catch (Exception exp)
            {
                _hosting.ShowError("保存文件失败。" + exp.Message);
            }
        }

        private bool ShowSaveDialog()
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = FileTypeHelper.GetAllFilters();
                if (Template != null)
                {
                    dialog.FilterIndex = FileTypeHelper.GetFilterIndex(GetExtensionByLanguage(Template.Syntax));
                }
                else if (!string.IsNullOrEmpty(FileName))
                {
                    var info1 = new FileInfo(FileName);
                    dialog.InitialDirectory = info1.DirectoryName;
                    dialog.FilterIndex = FileTypeHelper.GetFilterIndex(info1.Extension);
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
            if (_isChanged)
            {
                var r = _hosting.ShowConfirm("文档已修改，是否保存?", 3);
                if (r == ShowMsgButton.Cancel)
                {
                    e.Cancel = true;
                }
                else if (r == ShowMsgButton.Yes)
                {
                    SaveFile();
                }
            }

            base.OnClosing(e);
        }

        private void txtEditor_TextChanged(object sender, EventArgs e)
        {
            if (!_isChanged)
            {
                _isChanged = true;
                Text = _caption + " *";
            }
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

        private void btnOpen_Click(object sender, EventArgs e)
        {
            Process.Start("explorer", "/select,\"" + lblFileName.Text + "\"");
        }
    }
}
