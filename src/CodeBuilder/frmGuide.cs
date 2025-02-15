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
using CodeBuilder.Core.Source;
using CodeBuilder.Core.Template;
using Fireasy.Common.Extensions;
using Fireasy.Composition;
using Fireasy.Windows.Forms;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CodeBuilder
{
    public partial class frmGuide : FormBase
    {
        private readonly DevHosting _hosting;
        private RadioButton _radioButton;
        private List<string> _partitions;

        public frmGuide(DevHosting hosting, bool first = false)
        {
            InitializeComponent();
            Icon = Util.GetIcon();
            _hosting = hosting;
            panel3.Visible = first;
        }

        public Action<string> SourceAct { get; set; }

        public Action<string, string> TemplateAct { get; set; }

        public Action PropertyChangeAct { get; set; }

        public Action<List<PartitionDefinition>> BuildAct { get; set; }

        public Action CloseAct { get; set; }

        private void frmGuide_Load(object sender, System.EventArgs e)
        {
            lstTemplate.Renderer = new TemplateTreeListRenderer(_hosting);

            btnSkip.Visible = _hosting.GetTables().Any();
        }

        protected override void OnClosed(EventArgs e)
        {
            CloseAct?.Invoke();

            base.OnClosed(e);
        }

        private void btnNext1_Click(object sender, System.EventArgs e)
        {
            SourceAct?.Invoke(_radioButton.Text);
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            LoadTemplates();
        }

        private void btnNext2_Click(object sender, EventArgs e)
        {
            if (lstTemplate.HasSelectedItems)
            {
                var p = lstTemplate.SelectedItems[0].Group.Text;
                var tmp = lstTemplate.SelectedItems[0].Tag as TemplateDefinition;
                TemplateAct?.Invoke(p, tmp.TId);

                ShowProfileSetting();
            }
        }

        private void btnNext3_Click(object sender, EventArgs e)
        {
            var ckResult = Util.Validate(_hosting.Profile);
            if (!string.IsNullOrEmpty(ckResult))
            {
                _hosting.ShowWarn(ckResult);
                return;
            }

            ShowPreBuild();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择代码生成后输出的目录:";
                dialog.SelectedPath = Config.Instance.OutputDirectory;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _partitions = new List<string>();
                    var partitions = new List<PartitionDefinition>();
                    GetPartitions(lstPart.Items, partitions);

                    if (partitions.Count == 0)
                    {
                        _hosting.ShowWarn("至少要选择一个部件。");
                        return;
                    }

                    var isChanged = false;
                    if (Config.Instance.OutputDirectoryHistory.Contains(dialog.SelectedPath))
                    {
                        if (Config.Instance.OutputDirectoryHistory.FirstOrDefault() != dialog.SelectedPath)
                        {
                            Config.Instance.OutputDirectoryHistory.Remove(dialog.SelectedPath);
                            Config.Instance.OutputDirectoryHistory.Insert(0, dialog.SelectedPath);
                            isChanged = true;
                        }
                    }
                    else
                    {
                        Config.Instance.OutputDirectoryHistory.Insert(0, dialog.SelectedPath);
                        isChanged = true;
                    }

                    if (Config.Instance.OutputDirectory != dialog.SelectedPath)
                    {
                        Config.Instance.OutputDirectory = dialog.SelectedPath;
                        isChanged = true;
                    }

                    if (isChanged)
                    {
                        Config.Instance.Save();
                    }

                    Config.SavePartitionConfig(_hosting.Template.TId, _partitions);

                    BuildAct?.Invoke(partitions);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnPrev2_Click(object sender, EventArgs e)
        {
            pnlSource.Visible = true;
            pnlTemplate.Visible = false;
            btnSkip.Visible = _hosting.GetTables().Any();
        }

        private void btnPrev3_Click(object sender, EventArgs e)
        {
            pnlTemplate.Visible = true;
            pnlProfile.Visible = false;

            lstTemplate.Focus();
        }

        private void btnPrev4_Click(object sender, EventArgs e)
        {
            pnlProfile.Visible = true;
            pnlPreBuild.Visible = false;
        }

        public void Show(int step, IWin32Window parent)
        {
            Show(parent);

            if (step == 1)
            {
                LoadSources();
            }
            else if (step == 2)
            {
                LoadTemplates();
            }
            else if (step == 3)
            {
                ShowProfileSetting();
            }

            if (Visible)
            {
                return;
            }
        }

        private void LoadSources()
        {
            pnlSource.Visible = true;

            var x = 10;
            var y = 40;
            foreach (var source in _hosting.ServiceProvider.GetExportedServices<ISourceProvider>())
            {
                var rd = new RadioButton();
                rd.Text = source.Name;
                rd.BackgroundImage = source.Icon;
                rd.Tag = source;
                rd.Location = new System.Drawing.Point(x, y);
                rd.Size = new System.Drawing.Size(136, 160);
                rd.Appearance = System.Windows.Forms.Appearance.Button;
                rd.Text = source.Name;
                rd.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
                rd.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
                rd.TextImageRelation = TextImageRelation.ImageAboveText;
                rd.BackgroundImageLayout = ImageLayout.None;
                rd.UseVisualStyleBackColor = true;
                rd.Visible = true;
                x += 146;

                if (x > 146 * 4)
                {
                    x = 10;
                    y += 170;
                }

                rd.CheckedChanged += (o, e1) =>
                {
                    _radioButton = (RadioButton)o;
                    btnNext1.Enabled = true;
                    label2.Text = ((ISourceProvider)_radioButton.Tag).Description;
                };

                pnlSource.Controls.Add(rd);
            }
        }

        private void LoadTemplates()
        {
            pnlSource.Visible = false;
            pnlTemplate.Visible = true;

            if (lstTemplate.Groups.Count > 0)
            {
                return;
            }

            foreach (var p in _hosting.ServiceProvider.GetExportedServices<ITemplateProvider>())
            {
                var group = new Fireasy.Windows.Forms.TreeListGroup(p.Name);
                lstTemplate.Groups.Add(group);

                foreach (var template in p.GetTemplates())
                {
                    var item = group.Items.Add(template.Name);
                    item.Tag = template;

                    if (template.Equals(_hosting.Template))
                    {
                        item.EnsureVisible();
                        item.Selected = true;
                    }
                }
            }

            lstTemplate.Focus();
        }

        private void ShowProfileSetting()
        {
            pnlTemplate.Visible = false;
            pnlProfile.Visible = true;

            pgrid.SelectedObject = _hosting.Profile;
        }

        private void ShowPreBuild()
        {
            pnlProfile.Visible = false;
            pnlPreBuild.Visible = true;

            if (lstPart.Items.Count > 0)
            {
                return;
            }

            _partitions = Config.GetPartitionConfig(_hosting.Template.TId);

            lstPart.BeginUpdate();

            FillGroups(lstPart.Items, _hosting.Template.Groups);
            FillPartitions(lstPart.Items, _hosting.Template.Partitions);

            lstPart.EndUpdate();
        }

        private class TemplateTreeListRenderer : TreeListRenderer
        {
            private readonly IDevHosting _hosting;

            internal TemplateTreeListRenderer(IDevHosting hosting)
            {
                _hosting = hosting;
            }

            public override void DrawGroup(TreeListGroupRenderEventArgs e)
            {
                var tcolor = SystemColors.WindowText;
                var font = new Font("微软雅黑", e.Group.TreeList.GroupFont.Size);
                var sb = new LinearGradientBrush(e.Bounds, Color.FromArgb(240, 240, 240), Color.White, 0f);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.FillRectangle(sb, e.Bounds);
                var r = new Rectangle(0, e.Bounds.Bottom - 1, e.Bounds.Width, 1);
                sb = new LinearGradientBrush(r, Color.FromArgb(200, 200, 200), Color.FromArgb(240, 240, 240), 0f);
                e.Graphics.FillRectangle(sb, r);
                e.Graphics.DrawImage(Properties.Resources.engine, e.Bounds.X + 10, e.Bounds.Y + 8, 28, 28);
                e.Graphics.DrawString(e.Group.Text, font, new SolidBrush(tcolor), e.Bounds.X + 40, e.Bounds.Y + 11);

                font.Dispose();
                sb.Dispose();
            }

            public override void DrawCell(TreeListCellRenderEventArgs e)
            {
                var high = e.Cell.Item.Selected && e.Cell.Item.TreeList.Focused;
                var color = high ? SystemColors.HighlightText : SystemColors.WindowText;
                var tcolor = high ? SystemColors.HighlightText : Color.Blue;
                var font = new Font("微软雅黑", e.Cell.Item.TreeList.Font.Size + 2);
                var sb = new SolidBrush(color);
                var strFormat = new StringFormat();
                strFormat.LineAlignment = StringAlignment.Center;
                var tmp = e.Cell.Item.Tag as TemplateDefinition;

                if (tmp.Equals(_hosting.Template))
                {
                    e.Graphics.DrawImage(high ? Properties.Resources.down_ok1 : Properties.Resources.down_ok, e.Bounds.Width - 50, e.Bounds.Y + 22, 32, 32);
                }

                e.Graphics.DrawString(tmp.Name, font, new SolidBrush(tcolor), e.Bounds.X + 13, e.Bounds.Y + 5);
                e.Graphics.DrawString(string.IsNullOrEmpty(tmp.Description) ? "暂无说明。" : tmp.Description, e.Cell.Item.TreeList.Font, sb, new Rectangle(e.Bounds.X + 15, e.Bounds.Y + 30, e.Bounds.Width - 50, 45), strFormat);

                if (e.Cell.Item != e.Cell.Item.Group.Items.Last() && e.DrawState != DrawState.Selected)
                {
                    var pen = new Pen(Color.FromArgb(240, 240, 240), 1);
                    e.Graphics.DrawLine(pen, 0, e.Bounds.Bottom - 1, e.Bounds.Width, e.Bounds.Bottom - 1);
                    pen.Dispose();
                }

                font.Dispose();
                sb.Dispose();
            }
        }

        private void trlTemplate_ItemSelectionChanged(object sender, TreeListItemSelectionEventArgs e)
        {
            btnNext2.Enabled = lstTemplate.HasSelectedItems;
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

        private void lstPart_AfterItemCheckChange(object sender, TreeListItemEventArgs e)
        {
            CheckItems(e.Item.Items, e.Item.Checked);

            var p = e.Item.Parent;
            while (p != null)
            {
                p.Checked = true;
                p = p.Parent;
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
                item.Cells[1].Value = part.Output;

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

        private void GetPartitions(TreeListItemCollection items, List<PartitionDefinition> partitions)
        {
            foreach (var item in items)
            {
                if (item.Tag is PartitionDefinition && item.Checked)
                {
                    var part = item.Tag as PartitionDefinition;
                    _partitions.Add(part.Name);
                    partitions.Add(part);
                }

                GetPartitions(item.Items, partitions);
            }
        }

        private void tlbOpen_Click(object sender, EventArgs e)
        {
            var FILTER = "Profile Files(*.profile)|*.profile";
            var profileDir = Path.Combine(_hosting.WorkPath, "profiles");

            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = FILTER;
                dialog.InitialDirectory = profileDir;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _hosting.Profile = ProfileUnity.LoadProfile(_hosting, _hosting.Template, dialog.FileName);
                    pgrid.SelectedObject = _hosting.Profile;
                    PropertyChangeAct?.Invoke();
                }
            }
        }

        private void pgrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            PropertyChangeAct?.Invoke();
        }
    }
}
