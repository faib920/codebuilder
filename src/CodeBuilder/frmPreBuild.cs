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
using Fireasy.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CodeBuilder
{
    public partial class frmPreBuild : FormBase
    {
        private List<string> _partitions;
        private readonly IDevHosting _hosting;
        private Popup _popup;
        private static Dictionary<PartitionDefinition, string> _outputCache = new Dictionary<PartitionDefinition, string>();

        public frmPreBuild(IDevHosting hosting)
        {
            InitializeComponent();
            Icon = Util.GetIcon();
            _hosting = hosting;
            _popup = new Popup(panel3) { DropShadowEnabled = false, Font = Font, Resizable = false };
        }

        public TemplateDefinition Template { get; set; }

        public List<PartitionDefinition> Partitions { get; private set; }

        private void frmPreBuild_Load(object sender, EventArgs e)
        {
            _partitions = Config.GetPartitionConfig(Template.TId);

            lstPart.BeginUpdate();

            FillGroups(lstPart.Items, Template.Groups);
            FillPartitions(lstPart.Items, Template.Partitions);

            lstPart.EndUpdate();

            if (Config.Instance.OutputDirectoryHistory.Count > 0)
            {
                foreach (var path in Config.Instance.OutputDirectoryHistory)
                {
                    cboPath.Items.Add(path);
                }
            }

            cboPath.Text = Config.Instance.OutputDirectory;
        }

        private void btnSelectPath_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择代码生成后输出的目录:";
                dialog.SelectedPath = cboPath.Text;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    cboPath.Text = dialog.SelectedPath;
                }
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (cboPath.Text.Length == 0)
            {
                _hosting.ShowWarn("请选择代码输出的目录。");
                return;
            }

            _partitions = new List<string>();
            Partitions = new List<PartitionDefinition>();
            var messages = new List<string>();
            GetPartitions(lstPart.Items, messages);

            if (Partitions.Count == 0 || messages.Count > 0)
            {
                _hosting.ShowWarn(messages.Count == 0 ? "至少要选择一个部件。" : string.Join("；", messages) + "。");
                return;
            }

            var isChanged = false;
            if (Config.Instance.OutputDirectoryHistory.Contains(cboPath.Text))
            {
                if (Config.Instance.OutputDirectoryHistory.FirstOrDefault() != cboPath.Text)
                {
                    Config.Instance.OutputDirectoryHistory.Remove(cboPath.Text);
                    Config.Instance.OutputDirectoryHistory.Insert(0, cboPath.Text);
                    isChanged = true;
                }
            }
            else
            {
                Config.Instance.OutputDirectoryHistory.Insert(0, cboPath.Text);
                isChanged = true;
            }

            if (Config.Instance.OutputDirectory != cboPath.Text)
            {
                Config.Instance.OutputDirectory = cboPath.Text;
                isChanged = true;
            }

            if (isChanged)
            {
                Config.Instance.Save();
            }

            Config.SavePartitionConfig(Template.TId, _partitions);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void GetPartitions(TreeListItemCollection items, List<string> messages)
        {
            foreach (var item in items)
            {
                if (item.Tag is PartitionDefinition part && item.Checked)
                {
                    _partitions.Add(part.Name);

                    if (string.IsNullOrWhiteSpace(item.Cells[1].Text))
                    {
                        messages.Add(part.Name + "输出文件为空");
                    }

                    if (item.Cells[1].Text != part.Output)
                    {
                        _outputCache[part] = item.Cells[1].Text;
                    }
                    else if (_outputCache.ContainsKey(part))
                    {
                        _outputCache.Remove(part);
                    }

                    if (_outputCache.TryGetValue(part, out var output))
                    {
                        var newpart = new PartitionDefinition
                        {
                            Name = part.Name,
                            FilePath = part.FilePath,
                            FileName = part.FileName,
                            Loop = part.Loop,
                            Output = item.Cells[1].Text,
                            Syntax = part.Syntax
                        };
                        Partitions.Add(newpart);
                    }
                    else
                    {
                        Partitions.Add(part);
                    }
                }

                GetPartitions(item.Items, messages);
            }
        }

        private void FillGroups(TreeListItemCollection items, List<GroupDefinition> groups)
        {
            foreach (var group in groups)
            {
                var item = new TreeListItem(group.Name);
                item.Image = Properties.Resources.category;
                items.Add(item);

                FillGroups(item.Items, group.Groups);
                FillPartitions(item.Items, group.Partitions);

                if (item.Items.Any(s => s.Checked))
                {
                    item.Checked = true;
                }
                else if (lstPart.CheckAllChecked)
                {
                    lstPart.CheckAllChecked = false;
                }

                item.Expended = true;
            }
        }

        private void FillPartitions(TreeListItemCollection items, List<PartitionDefinition> partitions)
        {
            foreach (var part in partitions)
            {
                var item = new TreeListItem(part.Name);
                item.Image = Properties.Resources.fileT;
                item.Tag = part;
                items.Add(item);

                if (!_outputCache.TryGetValue(part, out var output))
                {
                    output = part.Output;
                }
                else
                {
                    item.ForeColor = Color.Blue;
                }

                item.Cells[1].Value = output;

                if (_partitions != null && _partitions.Contains(part.Name))
                {
                    item.Checked = true;
                }
                else if (lstPart.CheckAllChecked)
                {
                    lstPart.CheckAllChecked = false;
                }
            }
        }

        private void RefreshItems(TreeListItemCollection items)
        {
            foreach (var item in items)
            {
                if (item.Tag is PartitionDefinition part)
                {
                    item.Cells[1].Value = part.Output;
                    if (item.ForeColor != Color.Empty)
                    {
                        item.ForeColor = Color.Empty;
                    }
                }
                else
                {
                    RefreshItems(item.Items);
                }
            }
        }

        private void lstPart_AfterItemCheckChange(object sender, TreeListItemEventArgs e)
        {
            lstPart.EndEdit();

            CheckItems(e.Item.Items, e.Item.Checked);

            var p = e.Item.Parent;
            while (p != null)
            {
                p.Checked = true;
                p = p.Parent;
            }
        }

        private void CheckItems(TreeListItemCollection items, bool isChecked)
        {
            items.ForEach(s =>
            {
                s.Checked = isChecked;
                CheckItems(s.Items, isChecked);
            });
        }

        private void lstPart_CheckAllChanged(object sender, TreeListCheckAllEventArgs e)
        {
            CheckItems(lstPart.Items, e.Checked);
        }

        private void cboPath_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cboPath_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Left && e.KeyCode != Keys.Right &&
                e.KeyCode != Keys.Up && e.KeyCode != Keys.Down &&
                e.KeyCode != Keys.Home && e.KeyCode != Keys.End)
            {
                e.Handled = true;
            }
        }

        private void tlbReset_Click(object sender, EventArgs e)
        {
            _outputCache.Clear();
            RefreshItems(lstPart.Items);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            _popup.Show(this, toolStrip1.Right - panel3.Width, toolStrip1.Location.Y + 26);
        }

        private void lstPart_BeforeCellEditing(object sender, TreeListBeforeCellEditingEventArgs e)
        {
            e.Cancel = !e.Cell.Item.Checked;
        }

        private void lstPart_BeforeCellUpdating(object sender, TreeListBeforeCellUpdatingEventArgs e)
        {
            var changed = e.OldValue?.ToString() != e.NewValue?.ToString();
            e.Cell.Item.ForeColor = changed ? Color.Blue : Color.Empty;
        }
    }
}
