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
using Fireasy.Common.Extensions;
using Fireasy.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CodeBuilder
{
    public partial class frmPreBuild : FormBase
    {
        private List<string> _partitions;
        private readonly IDevHosting _hosting;

        public frmPreBuild(IDevHosting hosting)
        {
            InitializeComponent();
            Icon = Util.GetIcon();
            _hosting = hosting;
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

            txtPath.Text = Config.Instance.OutputDirectory;
        }

        private void btnSelectPath_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择代码生成后输出的目录:";
                dialog.SelectedPath = txtPath.Text;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtPath.Text = dialog.SelectedPath;
                }
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtPath.Text.Length == 0)
            {
                _hosting.ShowWarn("请选择代码输出的目录。");
                return;
            }

            _partitions = new List<string>();
            Partitions = new List<PartitionDefinition>();
            GetPartitions(lstPart.Items);

            if (Partitions.Count == 0)
            {
                _hosting.ShowWarn("至少要选择一个部件。");
                return;
            }

            if (Config.Instance.OutputDirectory != txtPath.Text)
            {
                Config.Instance.OutputDirectory = txtPath.Text;
                Config.Save();
            }

            Config.SavePartitionConfig(Template.TId, _partitions);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void GetPartitions(TreeListItemCollection items)
        {
            foreach (var item in items)
            {
                if (item.Tag is PartitionDefinition && item.Checked)
                {
                    var part = item.Tag as PartitionDefinition;
                    _partitions.Add(part.Name);
                    Partitions.Add(part);
                }

                GetPartitions(item.Items);
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

                item.Expended = true;
            }
        }

        private void FillPartitions(TreeListItemCollection items, List<PartitionDefinition> partitions)
        {
            foreach (var part in partitions)
            {
                var item = new TreeListItem(part.Name);
                item.Image = Properties.Resources.file;
                item.Tag = part;
                items.Add(item);
                item.Cells[1].Value = part.Output;

                if (_partitions != null && _partitions.Contains(part.Name))
                {
                    item.Checked = true;
                }
            }
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

        private void CheckItems(TreeListItemCollection items, bool isChecked)
        {
            items.ForEach(s =>
            {
                s.Checked = isChecked;
                CheckItems(s.Items, isChecked);
            });
        }
    }
}
