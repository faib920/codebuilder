// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using CodeBuilder.Core.Template;
using CodeBuilder.Core.Forms;
using CodeBuilder.Properties;
using Fireasy.Common.Extensions;
using Fireasy.Common.Serialization;
using Fireasy.Windows.Forms;
using ICSharpCode.TextEditor.Document;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CodeBuilder
{
    public partial class frmTemplateEditor : FormBase
    {
        private List<string> _removeResources;
        private readonly IDevHosting _hosting;

        public frmTemplateEditor(IDevHosting hosting)
        {
            InitializeComponent();
            Icon = Util.GetIcon();
            _hosting = hosting;
        }

        public TemplateDefinition Template { get; set; }

        private void frmTemplateEditor_Load(object sender, EventArgs e)
        {
            var editor = new TreeListComboBoxEditor();
            editor.Inner.DropDownStyle = ComboBoxStyle.DropDownList;
            editor.Inner.Items.Add("None");
            editor.Inner.Items.Add("Tables");
            editor.Inner.Items.Add("References");
            treeListColumn4.SetEditor(editor);

            editor = new TreeListComboBoxEditor();
            editor.Inner.DropDownStyle = ComboBoxStyle.DropDownList;
            foreach (DictionaryEntry kvp in HighlightingManager.Manager.HighlightingDefinitions)
            {
                editor.Inner.Items.Add(kvp.Key);
            }

            treeListColumn5.SetEditor(editor);

            if (Template != null)
            {
                txtId.Text = Template.Id;
                txtId.ReadOnly = true;
                txtName.Text = Template.Name;
                cboLanguage.Text = Template.Language;

                lstPart.BeginUpdate();
                FillItems(lstPart.Items, Template.Groups, Template.Partitions);
                lstPart.EndUpdate();

                lstRes.BeginUpdate();
                foreach (var res in Template.Resources)
                {
                    var item = lstRes.Items.Add(res);
                    item.Image = Resources.fileR;
                }

                lstRes.EndUpdate();
            }
        }

        private void FillItems(TreeListItemCollection items, List<GroupDefinition> groups, List<PartitionDefinition> partitions)
        {
            foreach (var group in groups)
            {
                var item = new TreeListItem(group.Name);
                items.Add(item);

                item.Tag = 0;
                item.Image = Properties.Resources.category;

                FillItems(item.Items, group.Groups, group.Partitions);

                item.Expended = true;
            }

            foreach (var part in partitions)
            {
                var item = new TreeListItem(part.Name);
                items.Add(item);
                item.Tag = 1;
                item.Cells[1].Value = part.FileName;
                item.Cells[2].Value = part.Output;
                item.Cells[3].Value = part.Loop.ToString();
                item.Cells[4].Value = part.Syntax;
                item.Image = Properties.Resources.file;
            }
        }

        private TreeListItem GetGroupItem()
        {
            var item = lstPart.SelectedItems[0];
            if ((int)item.Tag == 1)
            {
                return item.Parent;
            }

            return item;
        }

        private void mnuAddRoot_Click(object sender, EventArgs e)
        {
            var item = lstPart.Items.Add(string.Empty);
            item.Image = Properties.Resources.category;
            item.Tag = 0;
            lstPart.BeginEdit(item.Cells[0]);
        }

        private void mnuAddGroup_Click(object sender, EventArgs e)
        {
            TreeListItem item;
            if (lstPart.SelectedItems.Count != 0)
            {
                var parent = GetGroupItem();
                if (parent != null)
                {
                    item = parent.Items.Add(string.Empty);
                    parent.Expended = true;
                }
                else
                {
                    item = lstPart.Items.Add(string.Empty);
                }
            }
            else
            {
                item = lstPart.Items.Add(string.Empty);
            }

            item.Image = Properties.Resources.category;
            item.Tag = 0;
            lstPart.BeginEdit(item.Cells[0]);
        }

        private void mnuAdd_Click(object sender, EventArgs e)
        {
            TreeListItem item;
            if (lstPart.SelectedItems.Count != 0)
            {
                var parent = GetGroupItem();
                item = parent.Items.Add(string.Empty);
                parent.Expended = true;
            }
            else
            {
                item = lstPart.Items.Add(string.Empty);
            }

            item.Cells[3].Value = "None";
            item.Image = Properties.Resources.file;
            item.Tag = 1;
            lstPart.BeginEdit(item.Cells[0]);
        }

        private void mnuDelete_Click(object sender, EventArgs e)
        {
            if (lstPart.SelectedItems.Count == 0)
            {
                return;
            }

            lstPart.EndEdit();

            var item = lstPart.SelectedItems[0];
            if (item.Parent == null)
            {
                lstPart.Items.Remove(item);
            }
            else
            {
                item.Parent.Items.Remove(item);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            lstPart.EndEdit();

            if (string.IsNullOrEmpty(txtId.Text.Trim()))
            {
                _hosting.ShowWarn("模板标识不能为空。");
                return;
            }

            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                _hosting.ShowWarn("模板名称不能为空。");
                return;
            }

            var root = Path.Combine(_hosting.TemplateProvider.WorkDir, txtId.Text);
            if (Template == null && Directory.Exists(root))
            {
                _hosting.ShowWarn("已经存在 \"" + txtId.Text + "\" 的模板。");
                return;
            }

            if (Template == null)
            {
                Template = new TemplateDefinition { Id = txtId.Text };
            }

            Template.Name = txtName.Text;
            Template.Language = cboLanguage.Text;
            Template.Groups.Clear();
            Template.Partitions.Clear();

            InitPartitions(lstPart.Items, Template.Groups, Template.Partitions);
            InitResources(lstRes.Items, Template.Resources);
            SaveTemplate(root);

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void lstPart_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                mnuDelete_Click(null, null);
            }
        }

        private void lstPart_BeforeCellEditing(object sender, TreeListBeforeCellEditingEventArgs e)
        {
            if ((int)e.Cell.Item.Tag == 0 && e.Cell.Column.Index > 0)
            {
                e.Cancel = true;
            }
        }

        private void lstPart_AfterCellUpdated(object sender, TreeListAfterCellUpdatedEventArgs e)
        {
            if ((int)e.Cell.Item.Tag == 0 && e.EnterKey)
            {
                e.EnterKey = false;
            }
        }

        private void btnLocation_Click(object sender, EventArgs e)
        {
            var root = Path.Combine(_hosting.TemplateProvider.WorkDir, txtId.Text);
            Process.Start(root);
        }

        private void InitPartitions(TreeListItemCollection items, List<GroupDefinition> groups, List<PartitionDefinition> partitions)
        {
            foreach (var item in items)
            {
                if ((int)item.Tag == 0)
                {
                    var group = new GroupDefinition { Name = item.Cells[0].Value.ToStringSafely() };
                    groups.Add(group);
                    InitPartitions(item.Items, group.Groups, group.Partitions);
                }
                else if ((int)item.Tag == 1)
                {
                    var part = new PartitionDefinition
                    {
                        Name = item.Cells[0].Value.ToStringSafely(),
                        FileName = item.Cells[1].Value.ToStringSafely(),
                        Output = item.Cells[2].Value.ToStringSafely(),
                        Loop = item.Cells[3].Value.ToStringSafely() == string.Empty ? PartitionLoop.None : (PartitionLoop)Enum.Parse(typeof(PartitionLoop), item.Cells[3].Value.ToStringSafely()),
                        Syntax = item.Cells[4].Value.ToStringSafely()
                    };

                    if (string.IsNullOrEmpty(part.Name) ||
                        string.IsNullOrEmpty(part.FileName) ||
                        string.IsNullOrEmpty(part.Output))
                    {
                        continue;
                    }

                    part.FilePath = Path.Combine(_hosting.TemplateProvider.WorkDir, Template.Id, part.FileName);
                    if (!File.Exists(part.FilePath))
                    {
                        var path = Path.GetDirectoryName(part.FilePath);
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        File.WriteAllText(part.FilePath, string.Empty, Encoding.UTF8);
                    }

                    partitions.Add(part);
                }
            }
        }

        private void InitResources(TreeListItemCollection items, List<string> resources)
        {
            var root = Path.Combine(_hosting.TemplateProvider.WorkDir, Template.Id, "Resources");

            if (_removeResources != null)
            {
                foreach (var r in _removeResources)
                {
                    File.Delete(Path.Combine(root, r));
                }
            }

            foreach (var item in items)
            {
                if (item.Tag != null)
                {
                    var c = item.Text.LastIndexOf(Path.DirectorySeparatorChar);
                    var path = c == -1 ? root : Path.Combine(root, item.Text.Substring(0, c));
                    if (c != -1 && !Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    File.Copy(item.Tag.ToString(), Path.Combine(root, item.Text));
                }

                resources.Add(item.Text);
            }
        }

        private void SaveTemplate(string root)
        {
            var option = new JsonSerializeOption { Indent = true };
            option.Converters.Add(new PartLoopConverter());
            var json = new JsonSerializer(option);
            var content = json.Serialize(Template);

            var path = Path.Combine(_hosting.TemplateProvider.WorkDir, Template.Id + ".template");
            File.WriteAllText(path, content, Encoding.UTF8);

            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            foreach (var part in Template.Partitions)
            {
                if (!File.Exists(part.FilePath))
                {
                    File.WriteAllText(part.FilePath, string.Empty, Encoding.UTF8);
                }
            }

            foreach (var part in Template.Partitions)
            {
                if (!File.Exists(part.FilePath))
                {
                    File.WriteAllText(part.FilePath, string.Empty, Encoding.UTF8);
                }
            }
        }

        private void lstRes_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.All : DragDropEffects.None;
        }

        private void lstRes_DragDrop(object sender, DragEventArgs e)
        {
            var root = Path.Combine(_hosting.TemplateProvider.WorkDir, Template.Id, "Resources");

            var files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            foreach (var file in files)
            {
                if (File.Exists(file))
                {
                    var fileName = new FileInfo(file).Name;
                    if (lstRes.Items.Any(s => s.Text == fileName))
                    {
                        continue;
                    }

                    var item = lstRes.Items.Add(fileName);
                    item.Tag = file;
                    item.Image = Resources.fileR;
                }
                else if (Directory.Exists(file))
                {
                    var dir = new DirectoryInfo(file);
                    GetSubDirectories(root, file, dir.Name);
                }
            }
        }

        private void GetSubDirectories(string rootPath, string absPath, string path)
        {
            foreach (var file in Directory.GetFiles(absPath))
            {
                var fileName = new FileInfo(file).Name;
                var filePath = Path.Combine(path, fileName);
                if (lstRes.Items.Any(s => s.Text == filePath))
                {
                    continue;
                }

                var item = lstRes.Items.Add(filePath);
                item.Tag = file;
                item.Image = Resources.fileR;
            }

            foreach (var sub in Directory.GetDirectories(absPath))
            {
                var dir = new DirectoryInfo(sub);

                GetSubDirectories(rootPath, sub, Path.Combine(path, dir.Name));
            }
        }

        private void lstRes_KeyUp(object sender, KeyEventArgs e)
        {
            if (lstRes.SelectedItems.Count > 0 && e.KeyCode == Keys.Delete)
            {
                if (_removeResources == null)
                {
                    _removeResources = new List<string>();
                }

                _removeResources.Add(lstRes.SelectedItems[0].Text);
                lstRes.Items.Remove(lstRes.SelectedItems[0]);
            }
        }
    }

    public class PartLoopConverter : JsonConverter
    {
        public override bool CanConvert(Type type)
        {
            return type == typeof(PartitionLoop);
        }

        public override void WriteJson(JsonSerializer serializer, JsonWriter writer, object obj)
        {
            writer.WriteRaw("\"" + ((PartitionLoop)obj).ToString() + "\"");
        }
    }
}
