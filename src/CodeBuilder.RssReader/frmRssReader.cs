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
using CodeBuilder.Core.Tool;
using CodeHollow.FeedReader;
using Fireasy.Windows.Forms;
using Fireasy.Windows.Forms.Theme;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeBuilder.RssReader
{
    public partial class frmRssReader : DockFormBase, IContextMenuManager
    {
        private RssConfig _rssConfig;
        private readonly IDevHosting _hosting;

        public frmRssReader(IDevHosting hosting)
        {
            InitializeComponent();
            _hosting = hosting;
        }

        private void frmRssReader_Load(object sender, System.EventArgs e)
        {
            _rssConfig = RssConfig.LoadConfig();

            var windowSetManager = _hosting.ServiceProvider.GetService<IWindowSetManager>();
            windowSetManager.SetFlag("RssReader", 1);

            lstItems.Renderer = lstFavItems.Renderer = new RssRenderer();
            LoadCategories();
            LoadFavCategories();

            LoadAllRss();
        }

        protected override void OnClosed(EventArgs e)
        {
            var windowSetManager = _hosting.ServiceProvider.GetService<IWindowSetManager>();
            windowSetManager.SetFlag("RssReader", 0);
            base.OnClosed(e);
        }

        private void LoadCategories()
        {
            lstCategory.Items.Clear();

            foreach (var category in _rssConfig.View.Where(s => s.IsSystem == null || s.IsSystem == false))
            {
                var root = new TreeListItem(category.Name);
                lstCategory.Items.Add(root);
                root.ImageIndex = 0;

                foreach (var item in category.Items)
                {
                    var rssItem = new TreeListItem(item.Name);
                    root.Items.Add(rssItem);
                    rssItem.Checked = item.Checked == true;
                    rssItem.ImageIndex = item.Checked == true ? 2 : 1;
                    rssItem.Tag = item.Url;
                }

                root.Expended = true;
            }
        }

        private void LoadFavCategories()
        {
            lstFavCategory.Items.Clear();

            LoadFavCategory(lstFavCategory.Items, _rssConfig.Favorite);
        }

        private void LoadFavCategory(TreeListItemCollection items, List<RssFavCategory> categories)
        {
            foreach (var category in categories)
            {
                var item = new TreeListItem(category.Name);
                items.Add(item);
                item.ImageIndex = 0;
                item.Tag = category;

                LoadFavCategory(item.Items, category.Children);

                item.Expended = true;
            }
        }

        private void SaveCategories()
        {
            _rssConfig.View.Clear();

            foreach (var l1 in lstCategory.Items)
            {
                var cat = new RssCategory { Name = l1.Text };
                foreach (var l2 in l1.Items)
                {
                    cat.Items.Add(new RssItem
                    {
                        Name = l2.Text,
                        Url = l2.Tag as string,
                        Checked = l2.Checked,
                    });
                }

                _rssConfig.View.Add(cat);
            }

            RssConfig.SaveConfig(_rssConfig);
        }

        private void LoadAllRss()
        {
            lstItems.Items.Clear();

            Cursor = Cursors.WaitCursor;

            var categories = _rssConfig.View;
            var urls = _rssConfig.View.SelectMany(s => s.Items.Where(c => c.Checked == true).Select(c => c.Url));

            var tasks = urls.Select(s => FeedReader.ReadAsync(s).ContinueWith(r => r.IsFaulted ? new Feed { Items = new List<FeedItem>() } : r.Result )).ToArray();
            var task = Task.WhenAll(tasks).ContinueWith(r =>
            {
                var items = r.Result.SelectMany(t => t.Items).OrderByDescending(s => s.PublishingDate).Take(100);

                Invoke(new Action(() =>
                {
                    lstItems.BeginUpdate();

                    foreach (var feedItem in items)
                    {
                        var item = lstItems.Items.Add(null);
                        item.Tag = new FeedTreeListCellData { FeedItem = feedItem };
                    }

                    if (lstItems.Items.Count == 0)
                    {
                        lstItems.NoneItemText = "没有可显示的数据";
                    }

                    lstItems.EndUpdate();
                    Cursor = Cursors.Default;
                }));
            });
        }

        private void LoadFavRss()
        {
            lstFavItems.Items.Clear();

            Cursor = Cursors.WaitCursor;

            var categoryIds = new List<string>();
            if (tlbCategory.Checked && lstFavCategory.SelectedItems.Count > 0 && lstFavCategory.SelectedItems[0].Level > 0)
            {
                GetCategoryIds(new List<TreeListItem> { lstFavCategory.SelectedItems[0] }, categoryIds);
            }

            var items = RssFavoriteConfig.LoadFavorites(categoryIds);

            lstFavItems.BeginUpdate();

            foreach (var feedItem in items)
            {
                var item = lstFavItems.Items.Add(null);
                item.Tag = new FeedTreeListCellData { CategoryId = feedItem.CategoryId, FeedItem = new FeedItem { Link = feedItem.Url, Title = feedItem.Name, Description = feedItem.Description } };
            }

            if (lstFavItems.Items.Count == 0)
            {
                lstFavItems.NoneItemText = "没有可显示的数据";
            }

            lstFavItems.EndUpdate();
            Cursor = Cursors.Default;
        }

        private void GetCategoryIds(IEnumerable<TreeListItem> items, List<string> categoryIds)
        {
            foreach (var item in items)
            {
                if (item.Tag is RssFavCategory category)
                {
                    categoryIds.Add(category.Id);
                    GetCategoryIds(item.Items, categoryIds);
                }
            }
        }

        private async Task LoadRss(string url)
        {
            lstItems.Items.Clear();
            Cursor = Cursors.WaitCursor;

            Feed res = null;

            try
            {
                res = await FeedReader.ReadAsync(url);
            }
            catch (Exception exp)
            {
                Cursor = Cursors.Default;
                _hosting.ShowError(new InvalidOperationException($"无法加载 {url}!", exp));
                return;
            }

            lstItems.BeginUpdate();

            foreach (var feedItem in res.Items)
            {
                var item = lstItems.Items.Add(null);
                item.Tag = new FeedTreeListCellData { FeedItem = feedItem };
            }

            if (lstItems.Items.Count == 0)
            {
                lstItems.NoneItemText = "没有可显示的数据";
            }

            lstItems.EndUpdate();
            Cursor = Cursors.Default;
        }

        private void lstItems_CellMouseMove(object sender, TreeListCellMouseMoveEventArgs e)
        {
            var cellData = e.Cell.Item.Tag as FeedTreeListCellData;
            var rect = new Rectangle(4, 4, cellData.TitleWidth + 4, 26);

            var savedCurr = Cursor;
            Cursor = rect.Contains(e.Point) ? Cursors.Hand : Cursors.Default;

            if (savedCurr != Cursor)
            {
                cellData.IsHover = Cursor == Cursors.Hand;
                (sender as TreeList).Invalidate(e.Bounds);
            }
        }

        private void lstItems_CellMouseUp(object sender, TreeListCellMouseUpEventArgs e)
        {
            var cellData = e.Cell.Item.Tag as FeedTreeListCellData;
            var rect = new Rectangle(4, 4, cellData.TitleWidth + 4, 26);
            if (rect.Contains(e.Point))
            {
                Process.Start(cellData.FeedItem.Link);
            }
        }

        private void lstItems_CellMouseLeave(object sender, TreeListCellMouseLeaveEventArgs e)
        {
            var cellData = e.Cell.Item.Tag as FeedTreeListCellData;
            cellData.IsHover = false;
            (sender as TreeList).Invalidate(e.Bounds);
        }

        private void lstItems_ItemDoubleClick(object sender, TreeListItemEventArgs e)
        {
            var feedItem = e.Item.Tag as FeedTreeListCellData;
            Process.Start(feedItem.FeedItem.Link);
        }

        private async void lstCategory_ItemClick(object sender, TreeListItemEventArgs e)
        {
            if (e.Item.Level == 1)
            {
                await LoadRss((string)e.Item.Tag);
            }
        }

        private void lstFavCategory_ItemClick(object sender, TreeListItemEventArgs e)
        {
            LoadFavRss();
        }

        private void tlbRefresh_Click(object sender, System.EventArgs e)
        {
            if (tlbFav.Checked)
            {
                LoadFavRss();
            }
            else
            {
                LoadAllRss();
            }
        }

        private void tlbCategory_Click(object sender, System.EventArgs e)
        {
            if (!tlbFav.Checked)
            {
                if (tlbCategory.Checked)
                {
                    lstCategory.Visible = false;
                    splitter1.Visible = false;
                    tlbCategory.Checked = false;
                }
                else
                {
                    lstCategory.Visible = true;
                    splitter1.Visible = true;
                    tlbCategory.Checked = true;
                }
            }
            else
            {
                if (tlbCategory.Checked)
                {
                    lstFavCategory.Visible = false;
                    splitter2.Visible = false;
                    tlbCategory.Checked = false;
                }
                else
                {
                    lstFavCategory.Visible = true;
                    splitter2.Visible = true;
                    tlbCategory.Checked = true;
                }
            }
        }

        private void tlbFav_Click(object sender, EventArgs e)
        {
            tlbFav.Checked = !tlbFav.Checked;
            panel1.Visible = !tlbFav.Checked;
            panel2.Visible = tlbFav.Checked;

            if (!tlbFav.Checked)
            {
                if (!tlbCategory.Checked)
                {
                    lstCategory.Visible = false;
                    splitter1.Visible = false;
                }
                else
                {
                    lstCategory.Visible = true;
                    splitter1.Visible = true;
                }
            }
            else
            {
                if (lstFavItems.Items.Count == 0)
                {
                    LoadFavRss();
                }

                if (!tlbCategory.Checked)
                {
                    lstFavCategory.Visible = false;
                    splitter2.Visible = false;
                }
                else
                {
                    lstFavCategory.Visible = true;
                    splitter2.Visible = true;
                }
            }
        }

        private void tlbOpen_Click(object sender, System.EventArgs e)
        {
            if (lstItems.SelectedItems.Count == 0)
            {
                return;
            }

            var cellData = lstItems.SelectedItems[0].Tag as FeedTreeListCellData;
            Process.Start(cellData.FeedItem.Link);
        }

        private void tlbOpen1_Click(object sender, EventArgs e)
        {
            if (lstFavItems.SelectedItems.Count == 0)
            {
                return;
            }

            var cellData = lstFavItems.SelectedItems[0].Tag as FeedTreeListCellData;
            Process.Start(cellData.FeedItem.Link);
        }

        private void tlbAddFav_Click(object sender, System.EventArgs e)
        {
            if (lstItems.SelectedItems.Count == 0)
            {
                return;
            }

            var cellData = lstItems.SelectedItems[0].Tag as FeedTreeListCellData;

            using (var frm = new frmAddFavorite(_hosting, _rssConfig, (s) => SaveFavCategories(s)) { Title = cellData.FeedItem.Title, Url = cellData.FeedItem.Link, Description = cellData.FeedItem.Description })
            {
                if (frm.ShowDialog() != DialogResult.OK)
                {
                    if (frm.CategoryIsChanged)
                    {
                        LoadFavCategories();
                    }

                    return;
                }

                if (frm.CategoryIsChanged)
                {
                    LoadFavCategories();
                }

                if (lstFavItems.Items.Count > 0)
                {
                    LoadFavRss();
                }
            }
        }

        private void tlbRemoveFav_Click(object sender, EventArgs e)
        {
            if (lstFavItems.SelectedItems.Count == 0)
            {
                return;
            }

            if (_hosting.ShowConfirm("真的要取消收藏吗?") == ShowMsgButton.No)
            {
                return;
            }

            var cellData = lstFavItems.SelectedItems[0].Tag as FeedTreeListCellData;
            RssFavoriteConfig.RemoveFavorite(cellData.FeedItem.Link);

            LoadFavRss();
        }

        private void tlbAddCat_Click(object sender, System.EventArgs e)
        {
            var item = lstCategory.Items.Add(string.Empty);
            item.ImageIndex = 0;
            item.Flags = 1;
            lstCategory.BeginEdit(item.Cells[0]);
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
            if (e.Cell.Item.Level != 0)
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
                SaveCategories();
            }
        }

        private void lstCategory_ItemDragOver(object sender, TreeListItemDragOverEventArgs e)
        {
            if (e.Source.Level == 0 && e.Item.Level == 1)
            {
                e.Dragable = false;
            }
            else if (e.Source.Level == 1 && e.Item.Level == 0 && e.Position != DragPosition.Children)
            {
                e.Dragable = false;
            }
            else if (e.Source.Level == 1 && e.Item.Level == 1 && e.Position == DragPosition.Children)
            {
                e.Dragable = false;
            }
        }

        private void lstCategory_AfterItemDragDown(object sender, TreeListAfterItemDragDownEventArgs e)
        {
            SaveCategories();
        }

        private void tlbAddRss_Click(object sender, System.EventArgs e)
        {
            if (lstCategory.SelectedItems.Count == 0)
            {
                return;
            }

            var item = lstCategory.SelectedItems[0];
            var items = item.Level == 0 ? item.Items : item.Parent.Items;

            using (var frm = new frmAddRss(true))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    item = items.Add(frm.RssName);
                    item.ImageIndex = 1;
                    item.Tag = frm.RssUrl;
                    item.Checked = frm.DefaultLoad;
                    item.ImageIndex = frm.DefaultLoad ? 2 : 1;

                    SaveCategories();
                }
            }
        }

        private void tlbEditRss_Click(object sender, System.EventArgs e)
        {
            if (lstCategory.SelectedItems.Count == 0)
            {
                return;
            }

            var item = lstCategory.SelectedItems[0];

            using (var frm = new frmAddRss { RssName = item.Text, RssUrl = item.Tag as string, DefaultLoad = item.Checked })
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    item.Text = frm.RssName;
                    item.Tag = frm.RssUrl;
                    item.Checked = frm.DefaultLoad;
                    item.ImageIndex = frm.DefaultLoad ? 2 : 1;

                    SaveCategories();
                }
            }
        }

        private void tlbDelete_Click(object sender, EventArgs e)
        {
            if (lstCategory.SelectedItems.Count == 0)
            {
                return;
            }

            var item = lstCategory.SelectedItems[0];
            var items = item.Level == 0 ? lstCategory.Items : item.Parent.Items;

            var msg = item.Level == 0 ? $"是否删除订阅栏目\"{item.Text}\"?" : $"是否删除订阅源\"{item.Text}\"?";

            if (_hosting.ShowConfirm(msg) == ShowMsgButton.No)
            {
                return;
            }

            items.Remove(item);

            SaveCategories();
        }

        internal class RssRenderer : TreeListRenderer
        {
            public override void DrawCell(TreeListCellRenderEventArgs e)
            {
                var cellData = e.Cell.Item.Tag as FeedTreeListCellData;

                var high = e.Cell.Item.Selected && e.Cell.Item.TreeList.Focused;
                var color = SystemColors.WindowText;
                var tcolor = cellData.IsHover ? Color.Red : Color.Blue;
                var font = new Font("微软雅黑", e.Cell.Item.TreeList.Font.Size + 2, cellData.IsHover ? FontStyle.Underline : FontStyle.Regular);
                var font1 = new Font("微软雅黑", 10);
                var sb = new SolidBrush(color);
                var strFormat = new StringFormat();
                strFormat.Trimming = StringTrimming.EllipsisCharacter;
                strFormat.FormatFlags = StringFormatFlags.NoWrap;
                e.Graphics.DrawString(cellData.FeedItem.Title, font, new SolidBrush(tcolor), new Rectangle(e.Bounds.X + 4, e.Bounds.Y + 4, e.Bounds.Width - 14, 26), strFormat);

                cellData.TitleWidth = (int)e.Graphics.MeasureString(cellData.FeedItem.Title, font, e.Bounds.Width - 20, strFormat).Width;

                strFormat.FormatFlags = (StringFormatFlags)0;
                e.Graphics.DrawString(cellData.FeedItem.Description, font1, new SolidBrush(color), new Rectangle(e.Bounds.X + 4, e.Bounds.Y + 30, e.Bounds.Width - 14, e.Bounds.Height - 40), strFormat);
                font.Dispose();
                font1.Dispose();
                sb.Dispose();
            }

            public override void DrawCellGridLines(TreeListCellRenderEventArgs e)
            {
                e.Graphics.DrawLine(new Pen(Color.FromArgb(240, 240, 250)), e.Bounds.X, e.Bounds.Bottom, e.Bounds.Width, e.Bounds.Bottom);
            }

            protected override IColorSkin GetItemBackgroundSkin(TreeListItemRenderEventArgs e)
            {
                if (e.DrawState == DrawState.Selected)
                {
                    return new SolidColorSkin(e.Item.TreeList.Focused ? Color.FromArgb(255, 228, 148) : Color.FromArgb(240, 240, 240));
                }

                return base.GetItemBackgroundSkin(e);
            }
        }

        private class FeedTreeListCellData
        {
            public FeedItem FeedItem { get; set; }

            public int TitleWidth { get; set; }

            public bool IsHover { get; set; }

            public string CategoryId { get; set; }
        }

        private void lstFavCategory_AfterItemDragDown(object sender, TreeListAfterItemDragDownEventArgs e)
        {
            SaveFavCategories(lstFavCategory);
        }

        private void lstFavCategory_ItemDragOver(object sender, TreeListItemDragOverEventArgs e)
        {
            if (e.Item.Level == 0)
            {
                e.Dragable = false;
            }
        }

        private void SaveFavCategories(TreeList treeList)
        {
            _rssConfig.Favorite.Clear();

            FillCategories(treeList.Items, _rssConfig.Favorite);

            RssConfig.SaveConfig(_rssConfig);
        }

        private void FillCategories(TreeListItemCollection items, List<RssFavCategory> categories)
        {
            foreach (var item in items)
            {
                var category = item.Tag as RssFavCategory;
                category.Children.Clear();
                categories.Add(category);
                FillCategories(item.Items, category.Children);
            }
        }

        private void tlbMove_Click(object sender, EventArgs e)
        {
            if (lstFavItems.SelectedItems.Count == 0)
            {
                return;
            }

            var cellData = lstFavItems.SelectedItems[0].Tag as FeedTreeListCellData;

            using (var frm = new frmAddFavorite(_hosting, _rssConfig, (s) => SaveFavCategories(s)) { Title = cellData.FeedItem.Title, CategoryId = cellData.CategoryId, Url = cellData.FeedItem.Link, Description = cellData.FeedItem.Description })
            {
                if (frm.ShowDialog() != DialogResult.OK)
                {
                    if (frm.CategoryIsChanged)
                    {
                        LoadFavCategories();
                    }

                    return;
                }

                if (frm.CategoryIsChanged)
                {
                    LoadFavCategories();
                }
                LoadFavRss();
            }
        }

        IEnumerable<ToolStripItem> IContextMenuManager.GetContextMenuItems()
        {
            yield return new ToolStripMenuItem("添加桌面快捷方式", null, (o, e) =>
            {
                ToolShortcutHelper.Create(_hosting, "RssReader");
            });
        }
    }
}
