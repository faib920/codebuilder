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
using CodeBuilder.Properties;
using Fireasy.Windows.Forms;
using ICSharpCode.TextEditor.Document;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
        private readonly Popup _popup1;
        private readonly Popup _popup2;
        private readonly Popup _popup3;
        private TreeListItem _resRootNode;
        private TreeListItem _newItem;

        public frmTemplateEditor(IDevHosting hosting)
        {
            InitializeComponent();
            Icon = Util.GetIcon();
            _hosting = hosting;
            _popup1 = new Popup(panel3) { DropShadowEnabled = false, Font = Font, Resizable = false };
            _popup2 = new Popup(panel4) { DropShadowEnabled = false, Font = Font, Resizable = false };
            _popup3 = new Popup(panel5) { DropShadowEnabled = false, Font = Font, Resizable = false };
        }

        public TemplateDefinition Template { get; set; }

        public bool IsCodeFileChanged { get; set; }

        private void frmTemplateEditor_Load(object sender, EventArgs e)
        {
            var editor = new TreeListComboBoxEditor();
            editor.Inner.DropDownStyle = ComboBoxStyle.DropDownList;
            editor.Inner.Items.Add("None");
            editor.Inner.Items.Add("Tables");
            editor.Inner.Items.Add("References");
            treeListColumn4.SetEditor(editor);

            var syntaxs = new SortedSet<string>();
            editor = new TreeListComboBoxEditor();
            editor.Inner.DropDownHeight = 400;
            editor.Inner.DropDownStyle = ComboBoxStyle.DropDownList;
            foreach (var key in HighlightingManager.Manager.HighlightingDefinitions.Keys)
            {
                syntaxs.Add((string)key);
            }

            editor.Inner.Items.AddRange(syntaxs.ToArray());

            treeListColumn5.SetEditor(editor);

            _resRootNode = lstRes.Items.Add("Resources");
            _resRootNode.Image = Properties.Resources.category;
            _resRootNode.Expended = true;

            cboCategory.Items.AddRange(TemplateHelper.GetTemplateCategories(_hosting).ToArray());
            cboLanguage.Items.AddRange(TemplateHelper.GetTemplateLanguages(_hosting).ToArray());

            if (Template != null)
            {
                txtId.Text = Template.Id;
                txtId.ReadOnly = true;
                txtVer.Decimal = Template.Version;
                txtName.Text = Template.Name;
                txtDesc.Text = Template.Description;
                cboLanguage.Text = Template.Language;
                cboCategory.Text = Template.Category;

                lstPart.BeginUpdate();
                FillItems(lstPart.Items, Template.Groups, Template.Partitions);
                lstPart.EndUpdate();

                lstRes.BeginUpdate();
                LoadResources(_resRootNode.Items, Template.Resources);
                lstRes.EndUpdate();
            }

            tlbUseBase.Checked = Template?.Extension?.UseBase ?? true;

            FillExtensions(lstExt.Items.Add("common"), Template?.Extension?.Common);
            FillExtensions(lstExt.Items.Add("profile"), Template?.Extension?.Profile);
            FillExtensions(lstExt.Items.Add("schema"), Template?.Extension?.Schema);
        }

        private void LoadResources(TreeListItemCollection items, List<string> resources)
        {
            var cache = new Dictionary<string, TreeListItem>();

            foreach (var res in resources)
            {
                var path = res.Substring(0, res.LastIndexOf('\\'));
                if (!cache.TryGetValue(path, out var node))
                {
                    var paths = path.Split('\\');
                    node = FindPathItem(paths);
                    cache.Add(path, node);
                }

                var item = node.Items.Add(res.Substring(res.LastIndexOf("\\") + 1));
                item.Tag = res;
                item.Image = Resources.fileR;
            }
        }

        private TreeListItem FindPathItem(string[] paths)
        {
            var items = _resRootNode.Items;
            TreeListItem item = null;

            for (var i = 0; i < paths.Length; i++)
            {
                item = items.FirstOrDefault(s => s.Text == paths[i]);
                if (item == null)
                {
                    item = items.Add(paths[i]);
                    item.Image = Properties.Resources.category;
                    item.Expended = true;
                }

                items = item.Items;
            }

            return item;
        }

        private List<string> GetChildResources(TreeListItem item)
        {
            if (item.Tag is string fileName)
            {
                return new List<string> { fileName };
            }
            else
            {
                var result = new List<string>();

                foreach (var child in item.Items)
                {
                    result.AddRange(GetChildResources(child));
                }

                return result;
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

                if (TryParseColor(group.Color, out var color))
                {
                    item.ForeColor = color;
                }

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
                item.Image = Properties.Resources.fileT;

                if (TryParseColor(part.Color, out var color))
                {
                    item.ForeColor = color;
                }
            }
        }

        private void FillExtensions(TreeListItem item, List<string> files)
        {
            if (files != null)
            {
                foreach (var s in files)
                {
                    var subitem = item.Items.Add(s);
                    subitem.Image = Properties.Resources.codefile;
                }
            }

            item.Expended = true;
            item.Image = Properties.Resources.category;
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
            item.EnsureVisible();
            _newItem = item;
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
            item.EnsureVisible();
            _newItem = item;
            lstPart.BeginEdit(item.Cells[0]);
        }

        private void mnuAdd_Click(object sender, EventArgs e)
        {
            TreeListItem item;
            if (lstPart.SelectedItems.Count != 0)
            {
                var parent = GetGroupItem();
                item = (parent == null ? lstPart.Items : parent.Items).Add(string.Empty);
                if (parent != null)
                {
                    parent.Expended = true;
                }
            }
            else
            {
                item = lstPart.Items.Add(string.Empty);
            }

            item.Cells[3].Value = "None";
            item.Image = Properties.Resources.file;
            item.Tag = 1;
            item.EnsureVisible();
            _newItem = item;
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

            errorProvider1.Clear();

            if (string.IsNullOrEmpty(txtId.Text.Trim()))
            {
                errorProvider1.SetError(txtId, "模板标识不能为空");
                return;
            }

            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                errorProvider1.SetError(txtName, "模板名称不能为空");
                return;
            }

            var root = Path.Combine(_hosting.TemplateProvider.WorkDir, txtId.Text);
            if (Template == null && Directory.Exists(root))
            {
                errorProvider1.SetError(txtId, $"已经存在 {txtId.Text} 的模板");
                return;
            }

            if (Template == null)
            {
                Template = new TemplateDefinition { Id = txtId.Text };
            }

            Template.Name = txtName.Text;
            Template.Language = cboLanguage.Text;
            Template.Category = cboCategory.Text;
            Template.Version = txtVer.Decimal.Value;
            Template.Description = txtDesc.Text;
            Template.Groups.Clear();
            Template.Partitions.Clear();

            InitPartitions(lstPart.Items, Template.Groups, Template.Partitions);
            InitExtensions(lstExt.Items, Template.Extension);
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

        private void lstPart_AfterCellEditCanceled(object sender, TreeListAfterCellEditCanceledEventArgs e)
        {
            if (e.Cell.Item == _newItem)
            {
                var items = e.Cell.Item.Parent == null ? lstPart.Items : e.Cell.Item.Parent.Items;
                items.Remove(e.Cell.Item);
                _newItem = null;
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
                    var group = new GroupDefinition { Name = item.Cells[0].Value?.ToString() };
                    groups.Add(group);
                    InitPartitions(item.Items, group.Groups, group.Partitions);

                    if (item.ForeColor != Color.Empty)
                    {
                        group.Color = $"{item.ForeColor.R},{item.ForeColor.G},{item.ForeColor.B}";
                    }
                }
                else if ((int)item.Tag == 1)
                {
                    var part = new PartitionDefinition
                    {
                        Name = item.Cells[0].Value?.ToString(),
                        FileName = item.Cells[1].Value?.ToString(),
                        Output = item.Cells[2].Value?.ToString(),
                        Loop = string.IsNullOrEmpty(item.Cells[3].Value?.ToString()) ? PartitionLoop.None : (PartitionLoop)Enum.Parse(typeof(PartitionLoop), item.Cells[3].Value?.ToString()),
                        Syntax = item.Cells[4].Value?.ToString()
                    };

                    if (string.IsNullOrEmpty(part.Name) ||
                        string.IsNullOrEmpty(part.FileName) ||
                        string.IsNullOrEmpty(part.Output))
                    {
                        continue;
                    }

                    if (item.ForeColor != Color.Empty)
                    {
                        part.Color = $"{item.ForeColor.R},{item.ForeColor.G},{item.ForeColor.B}";
                    }

                    part.FilePath = Path.Combine(_hosting.TemplateProvider.WorkDir, Template.Id, part.FileName);
                    if (!File.Exists(part.FilePath))
                    {
                        var path = Path.GetDirectoryName(part.FilePath);
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        Util.TryOperateFile(part.FilePath, () => File.WriteAllText(part.FilePath, string.Empty, Encoding.UTF8));
                    }

                    partitions.Add(part);
                }
            }
        }

        private void InitExtensions(TreeListItemCollection items, TemplateExtension ext)
        {
            ext.UseBase = tlbUseBase.Checked;

            foreach (var item in items)
            {
                if (item.Text == "common")
                {
                    ext.Common = GetExtensions(item.Items, ext.Common);
                }
                else if (item.Text == "profile")
                {
                    ext.Profile = GetExtensions(item.Items, ext.Profile);
                }
                else if (item.Text == "schema")
                {
                    ext.Schema = GetExtensions(item.Items, ext.Schema);
                }
            }
        }

        private List<string> GetExtensions(TreeListItemCollection items, List<string> original)
        {
            var list = items.Where(s => !string.IsNullOrWhiteSpace(s.Text)).Select(s => s.Text).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            if (!IsCodeFileChanged && original != null)
            {
                IsCodeFileChanged = !list.SequenceEqual(original);
            }

            return list;
        }

        private void InitResources(TreeListItemCollection items, List<string> resources)
        {
            var root = Path.Combine(_hosting.TemplateProvider.WorkDir, Template.Id, "Resources");

            if (_removeResources != null)
            {
                foreach (var r in _removeResources)
                {
                    File.Delete(Path.Combine(root, r));
                    resources.Remove(r);
                }
            }

            CopyNewFiles(_resRootNode.Items, resources, root, string.Empty);
        }

        private void CopyNewFiles(TreeListItemCollection items, List<string> resources, string root, string subpath)
        {
            foreach (var item in items)
            {
                if (item.Tag is FileCopyRel rel)
                {
                    var path = Path.Combine(root, subpath);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    File.Copy(rel.FileName, Path.Combine(path, item.Text));
                    resources.Add(Path.Combine(subpath, item.Text));
                }
                else
                {
                    CopyNewFiles(item.Items, resources, root, Path.Combine(subpath, item.Text));
                }
            }
        }

        private void SaveTemplate(string root)
        {
            TemplateUnity.SaveTemplateDefinition(_hosting, Template);

            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            foreach (var part in Template.Partitions)
            {
                if (!File.Exists(part.FilePath))
                {
                    Util.TryOperateFile(part.FilePath, () => File.WriteAllText(part.FilePath, string.Empty, Encoding.UTF8));
                }
            }

            foreach (var part in Template.Partitions)
            {
                if (!File.Exists(part.FilePath))
                {
                    Util.TryOperateFile(part.FilePath, () => File.WriteAllText(part.FilePath, string.Empty, Encoding.UTF8));
                }
            }
        }

        private void lstRes_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Move : DragDropEffects.None;
        }

        private void lstRes_DragDrop(object sender, DragEventArgs e)
        {
            var selectedItem = lstRes.SelectedItems.FirstOrDefault();

            var items = selectedItem == null ? _resRootNode.Items : (selectedItem.Tag != null ? selectedItem.Parent.Items : selectedItem.Items);

            var root = Path.Combine(_hosting.TemplateProvider.WorkDir, Template.Id, "Resources");

            var files = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            foreach (var file in files)
            {
                if (File.Exists(file))
                {
                    var fileName = new FileInfo(file).Name;
                    if (items.Any(s => s.Text == fileName))
                    {
                        continue;
                    }

                    var item = items.Add(fileName);
                    item.Tag = file;
                    item.Image = Resources.fileR;
                }
                else if (Directory.Exists(file))
                {
                    var dir = new DirectoryInfo(file);
                    GetSubDirectories(items, root, file, dir.Name);
                }
            }
        }

        private void GetSubDirectories(TreeListItemCollection items, string rootPath, string subPath, string path)
        {
            var dirItem = items.Add(path);
            dirItem.Expended = true;
            dirItem.Image = Resources.category;

            foreach (var file in Directory.GetFiles(subPath))
            {
                var fileName = new FileInfo(file).Name;
                if (dirItem.Items.Any(s => s.Text == fileName))
                {
                    continue;
                }

                var item = dirItem.Items.Add(fileName);
                item.Tag = new FileCopyRel(file);
                item.Image = Resources.fileR;
            }

            foreach (var sub in Directory.GetDirectories(subPath))
            {
                var dir = new DirectoryInfo(sub);

                GetSubDirectories(dirItem.Items, rootPath, sub, dir.Name);
            }
        }

        private void lstRes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                tlbDeleteRes_Click(null, null);
            }
        }

        private void mnuAddExt_Click(object sender, EventArgs e)
        {
            if (lstExt.SelectedItems.Count == 0)
            {
                return;
            }

            var item = lstExt.SelectedItems[0];
            if (item.Level == 1)
            {
                item = item.Parent;
            }

            var subitem = item.Items.Add("");
            subitem.Image = Properties.Resources.codefile;
            _newItem = subitem;
            lstExt.BeginEdit(subitem.Cells[0]);
        }

        private void lstExt_BeforeCellEditing(object sender, TreeListBeforeCellEditingEventArgs e)
        {
            if (e.Cell.Item.Level == 0)
            {
                e.Cancel = true;
            }
        }

        private void lstExt_AfterCellEdited(object sender, TreeListAfterCellEditedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Cell.Text) && e.Cell.Item == _newItem)
            {
                var items = e.Cell.Item.Parent == null ? lstExt.Items : e.Cell.Item.Parent.Items;
                items.Remove(e.Cell.Item);
            }

            _newItem = null;
        }

        private void lstExt_AfterCellEditCanceled(object sender, TreeListAfterCellEditCanceledEventArgs e)
        {
            if (e.Cell.Item == _newItem)
            {
                var items = e.Cell.Item.Parent == null ? lstExt.Items : e.Cell.Item.Parent.Items;
                items.Remove(e.Cell.Item);
            }
        }

        private void lstExt_ItemDragOver(object sender, TreeListItemDragOverEventArgs e)
        {
            e.Dragable = e.Item.Level == 1 && e.Source.Parent == e.Item.Parent && e.Position != DragPosition.Children;
        }

        private void lstExt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                mnuDelExt_Click(null, null);
            }
        }

        private void mnuDelExt_Click(object sender, EventArgs e)
        {
            if (lstExt.SelectedItems.Count == 0)
            {
                return;
            }

            lstExt.EndEdit();

            var item = lstExt.SelectedItems[0];
            if (item.Level == 0)
            {
                return;
            }

            item.Parent.Items.Remove(item);
        }

        private void tlbUseBase_Click(object sender, EventArgs e)
        {
            tlbUseBase.Checked = !tlbUseBase.Checked;
        }

        private void mnuSelect_Click(object sender, EventArgs e)
        {
            if (lstExt.SelectedItems.Count == 0)
            {
                return;
            }

            var item = lstExt.SelectedItems[0];
            if (item.Level == 1)
            {
                item = item.Parent;
            }

            using (var dialog = new OpenFileDialog()
            {
                InitialDirectory = Path.Combine(_hosting.WorkPath, "extensions", item.Text),
                Filter = "C#代码文件(*.cs)|*.cs|VB代码文件(*.vb)|*.vb"
            })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var name = new FileInfo(dialog.FileName).Name;
                    if (!item.Items.Any(s => s.Text.Equals(name, StringComparison.OrdinalIgnoreCase)))
                    {
                        var subitem = item.Items.Add(name);
                        subitem.Image = Properties.Resources.codefile;
                    }
                }
            }
        }

        private void lstExt_ItemSelectionChanged(object sender, TreeListItemSelectionEventArgs e)
        {
            mnuAddExt.Enabled = mnuSelect.Enabled = tlbAddExt.Enabled = tlbSelect.Enabled = lstExt.SelectedItems.Count > 0;
            mnuDelExt.Enabled = tlbDelExt.Enabled = lstExt.SelectedItems.Count > 0 && lstExt.SelectedItems[0].Level == 1;
        }

        private void lstPart_ItemDragOver(object sender, TreeListItemDragOverEventArgs e)
        {
            if ((int)e.Source.Tag == 0 && (int)e.Item.Tag == 1)
            {
                e.Dragable = false;
            }
            else if ((int)e.Source.Tag == 1 && (int)e.Item.Tag == 1 && e.Position == DragPosition.Children)
            {
                e.Dragable = false;
            }
        }

        private bool TryParseColor(string strColor, out Color color)
        {
            if (!string.IsNullOrEmpty(strColor))
            {
                var d = strColor.Split(',');
                color = Color.FromArgb(int.Parse(d[0]), int.Parse(d[1]), int.Parse(d[2]));
                return true;
            }

            color = default(Color);
            return false;
        }

        private void mnuColor_Click(object sender, EventArgs e)
        {
            if (lstPart.SelectedItems.Count == 0)
            {
                return;
            }

            var item = sender as ToolStripMenuItem;
            lstPart.SelectedItems[0].ForeColor = item.Text == "无" ? Color.Empty : item.BackColor;
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            if (panel2.Visible)
            {
                panel2.Visible = false;
                panel1.BackgroundImage = Properties.Resources.down1;
                tabControl1.Top = panel2.Top + 4;
                tabControl1.Height += panel2.Height;
            }
            else
            {
                panel2.Visible = true;
                panel1.BackgroundImage = Properties.Resources.up1;
                tabControl1.Top = panel2.Bottom + 4;
                tabControl1.Height -= panel2.Height;
            }
        }

        private void tlbAddResFolder_Click(object sender, EventArgs e)
        {
            var selectedItem = lstRes.SelectedItems.FirstOrDefault();

            var items = selectedItem == null ? _resRootNode.Items : (selectedItem.Tag != null ? selectedItem.Parent.Items : selectedItem.Items);

            var item = items.Add(string.Empty);
            item.EnsureVisible();
            item.Image = Properties.Resources.category;
            lstRes.Columns[0].Editable = true;
            lstRes.BeginEdit(item.Cells[0]);
        }

        private void tlbAddResFiles_Click(object sender, EventArgs e)
        {
            var items = lstRes.SelectedItems.Count > 0 ? lstRes.SelectedItems[0].Items : lstRes.Items;

            using (var dialog = new OpenFileDialog { Filter = "所有文件(*.*)|*.*", Multiselect = true })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (var file in dialog.FileNames)
                    {
                        var fileName = new FileInfo(file).Name;
                        if (items.Any(s => s.Text == fileName))
                        {
                            continue;
                        }

                        var item = items.Add(fileName);
                        item.Tag = new FileCopyRel(file);
                        item.Image = Resources.fileR;
                    }
                }
            }
        }

        private void lstRes_AfterCellEdited(object sender, TreeListAfterCellEditedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Cell.Text))
            {
                var items = e.Cell.Item.Parent == null ? lstRes.Items : e.Cell.Item.Parent.Items;
                items.Remove(e.Cell.Item);
            }
            else
            {
                lstRes.Columns[0].Editable = false;
                e.Cell.Item.Expended = true;
            }
        }

        private void lstRes_AfterCellEditCanceled(object sender, TreeListAfterCellEditCanceledEventArgs e)
        {
            var items = e.Cell.Item.Parent == null ? lstRes.Items : e.Cell.Item.Parent.Items;
            items.Remove(e.Cell.Item);
            lstRes.Columns[0].Editable = false;
        }

        private void tlbDeleteRes_Click(object sender, EventArgs e)
        {
            if (lstRes.SelectedItems.Count == 0)
            {
                return;
            }

            if (_removeResources == null)
            {
                _removeResources = new List<string>();
            }

            var item = lstRes.SelectedItems[0];
            if (item.Level == 0 && _hosting.ShowConfirm("是否删除所有资源?") != ShowMsgButton.Yes)
            {
                return;
            }

            var resources = GetChildResources(item);
            if (resources.Count > 0)
            {
                _removeResources.AddRange(resources);
            }

            if (item != _resRootNode)
            {
                var items = item.Parent == null ? lstRes.Items : item.Parent.Items;
                items.Remove(item);
            }
            else
            {
                for (var i = _resRootNode.Items.Count - 1; i >= 0; i--)
                {
                    _resRootNode.Items.RemoveAt(i);
                }
            }
        }

        private void tlbTip1_Click(object sender, EventArgs e)
        {
            _popup1.Show(tabControl1, toolStrip1.Right - panel3.Width + 4, 56);
        }

        private void tlbTip2_Click(object sender, EventArgs e)
        {
            _popup2.Show(tabControl1, toolStrip2.Right - panel4.Width + 4, 56);
        }

        private void tlbTip3_Click(object sender, EventArgs e)
        {
            _popup3.Show(tabControl1, toolStrip3.Right - panel5.Width + 4, 56);
        }

        private class FileCopyRel
        {
            public FileCopyRel(string fileName)
            {
                FileName = fileName;
            }

            public string FileName { get; }
        }
    }

    public class PartLoopConverter : JsonConverter
    {
        public override bool CanConvert(Type type)
        {
            return type == typeof(PartitionLoop);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRaw("\"" + ((PartitionLoop)value).ToString() + "\"");
        }
    }
}
