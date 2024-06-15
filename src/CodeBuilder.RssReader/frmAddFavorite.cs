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
using Fireasy.Common.Security;
using Fireasy.Windows.Forms;
using System;
using System.Collections.Generic;

namespace CodeBuilder.RssReader
{
    public partial class frmAddFavorite : FormBase
    {
        private readonly IDevHosting _hosting;
        private readonly RssConfig _config;
        private readonly Action<TreeList> _saveCategory;

        public frmAddFavorite(IDevHosting hosting, RssConfig config, Action<TreeList> saveCategory)
        {
            InitializeComponent();
            _hosting = hosting;
            _config = config;
            _saveCategory = saveCategory;
        }

        public string Title { get; set; }

        public string Url { get; set; }

        public string CategoryId { get; set; }

        public string Description { get; set; }

        public bool CategoryIsChanged { get; set; }

        private void frmAddFavorite_Load(object sender, System.EventArgs e)
        {
            textBox1.Text = Title;

            LoadCategory(lstCategory.Items, _config.Favorite);
        }

        private void LoadCategory(TreeListItemCollection items, List<RssFavCategory> categories)
        {
            foreach (var category in categories)
            {
                var item = new TreeListItem(category.Name);
                items.Add(item);
                item.ImageIndex = 0;
                item.Tag = category;

                if (CategoryId == category.Id)
                {
                    item.Selected = true;
                }

                LoadCategory(item.Items, category.Children);

                item.Expended = true;
            }
        }

        private void tlbAdd_Click(object sender, System.EventArgs e)
        {
            var item = lstCategory.SelectedItems.Count == 0 ? lstCategory.Items[0] : lstCategory.SelectedItems[0];
            var newitem = item.Items.Add(string.Empty);
            item.Expended = true;
            newitem.ImageIndex = 0;
            newitem.Flags = 1;
            newitem.EnsureVisible();
            lstCategory.BeginEdit(newitem.Cells[0]);
        }

        private void lstCategory_AfterCellEditCanceled(object sender, TreeListAfterCellEditCanceledEventArgs e)
        {
            if (e.Cell.Item.Flags == 1)
            {
                var items = e.Cell.Item.Parent == null ? lstCategory.Items : e.Cell.Item.Parent.Items;
                items.Remove(e.Cell.Item);
            }
        }

        private void lstCategory_BeforeCellEditing(object sender, TreeListBeforeCellEditingEventArgs e)
        {
            if (e.Cell.Item.Level == 0)
            {
                e.Cancel = true;
            }
        }

        private void lstCategory_AfterCellEdited(object sender, TreeListAfterCellEditedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Cell.Text) && e.Cell.Item.Flags == 1)
            {
                lstCategory.Items.Remove(e.Cell.Item);
            }
            else
            {
                if (e.Cell.Item.Flags == 1)
                {
                    var category = new RssFavCategory { Id = RandomGenerator.Create(), Name = e.Cell.Text };
                    e.Cell.Item.Tag = category;
                    (e.Cell.Item.Parent.Tag as RssFavCategory).Children.Add(category);
                    RssConfig.SaveConfig(_config);
                    CategoryIsChanged = true;
                }
                else
                {
                    var category = e.Cell.Item.Tag as RssFavCategory;
                    if (category.Name != e.Cell.Text)
                    {
                        category.Name = e.Cell.Text;
                        RssConfig.SaveConfig(_config);
                        CategoryIsChanged = true;
                    }
                }
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            var item = lstCategory.SelectedItems.Count == 0 ? lstCategory.Items[0] : lstCategory.SelectedItems[0];
            var category = item.Tag as RssFavCategory;
            RssFavoriteConfig.AddFavorite(category.Id, textBox1.Text, Url, Description);
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lstCategory_AfterItemDragDown(object sender, TreeListAfterItemDragDownEventArgs e)
        {
            _saveCategory(lstCategory);
            CategoryIsChanged= true;
        }

        private void lstCategory_ItemDragOver(object sender, TreeListItemDragOverEventArgs e)
        {
            if (e.Item.Level == 0)
            {
                e.Dragable = false;
            }
        }

        private void tlbRemove_Click(object sender, EventArgs e)
        {
            if (lstCategory.SelectedItems.Count == 0 || lstCategory.SelectedItems[0].Level == 0)
            {
                return;
            }

            if (_hosting.ShowConfirm($"是否删除栏目\"{lstCategory.SelectedItems[0].Text}\"?") == ShowMsgButton.No)
            {
                return;
            }

            var item = lstCategory.SelectedItems[0];
            var category = item.Parent.Tag as RssFavCategory;
            category.Children.RemoveAt(item.Index);
            item.Parent.Items.Remove(item);
            RssConfig.SaveConfig(_config);
            CategoryIsChanged = true;
        }

        private void lstCategory_ItemSelectionChanged(object sender, TreeListItemSelectionEventArgs e)
        {
            tlbRemove.Enabled = lstCategory.SelectedItems.Count > 0 && lstCategory.SelectedItems[0].Level > 0;
        }
    }
}
