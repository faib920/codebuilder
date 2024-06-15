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
using Fireasy.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CodeBuilder.JsonTool
{
    public partial class frmJsonFormatter : DockFormBase, IContextMenuManager
    {
        private bool _isNew = false;
        private List<TreeListItem> _searchItems;
        private int _searchIndex = 0;
        private string _lastSearchText;
        private bool _isStarting = true;
        private List<string> _marks = new List<string>();
        private string _weburl;
        private readonly IDevHosting _hosting;
        private Popup _popup;
        private Color _markColor = Color.Red;

        public frmJsonFormatter(IDevHosting hosting)
        {
            InitializeComponent();
            Icon = Util.GetIcon();
            _hosting = hosting;

            txtResult.Language = FastColoredTextBoxNS.Language.JSON;

            txtResult.Font = txtSource.Font = new System.Drawing.Font(txtResult.Font.FontFamily, (int)_hosting.GetConfig("FontSize"));

            treeList1.ShowRowNumber = true;
            _popup = new Popup(panel3) { DropShadowEnabled = false, Font = Font, Resizable = false };
        }

        private bool Parse(TreeListItemCollection items, object obj)
        {
            if (obj is JObject jobj)
            {
                foreach (var kvp in jobj)
                {
                    var item = items.Add(kvp.Key);
                    item.ImageIndex = 2;

                    if (kvp.Value is JObject subdic)
                    {
                        item.ImageIndex = 1;
                        Parse(item.Items, kvp.Value);
                    }
                    else if (kvp.Value is JArray jarray)
                    {
                        item.ImageIndex = 0;
                        var i = 0;
                        foreach (var v in jarray)
                        {
                            var subitem = item.Items.Add("[" + i++ + "]");
                            subitem.ImageIndex = 1;
                            if (!Parse(subitem.Items, v))
                            {
                                subitem.Cells[1].Value = v?.ToString();
                            }

                            subitem.Expended = true;
                        }

                        item.Cells[1].Value = "(" + item.Items.Count + "项)";
                    }
                    else if (kvp.Value is JValue jvalue)
                    {
                        item.Cells[1].Value = jvalue.Value == null ? "<null>" : jvalue.Value;
                    }

                    item.Expended = true;
                    TryMark(item);
                }

                return true;
            }
            else if (obj is JArray jarray)
            {
                var i = 0;
                foreach (var v in jarray)
                {
                    var subitem = items.Add(i++.ToString());
                    subitem.ImageIndex = 1;
                    if (!Parse(subitem.Items, v))
                    {
                        subitem.Cells[1].Value = v?.ToString();
                    }
                    subitem.Expended = true;
                    TryMark(subitem);
                }

                return true;
            }

            return false;
        }

        private void txtSource_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                _isNew = true;
                HandleTree();
            }
        }

        private void txtSource_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V && txtSource.Text.Length > 0)
            {
                _searchItems = null;
                _searchIndex = 0;
                _isStarting = true;
            }
        }

        private void HandleTree()
        {
            if (string.IsNullOrWhiteSpace(txtSource.Text))
            {
                return;
            }

            Cursor = Cursors.WaitCursor;

            try
            {
                var obj = JsonConvert.DeserializeObject(txtSource.Text);

                treeList1.Items.Clear();
                treeList1.BeginUpdate();
                Parse(treeList1.Items, obj);
                treeList1.EndUpdate();

                var str = (obj as JToken).ToString(Formatting.Indented);
                if (str.Length < 100000)
                {
                    txtResult.Text = str;
                    txtResult.Visible = true;
                    txtResult1.Visible = false;
                }
                else
                {
                    txtResult1.Text = str;
                    txtResult.Visible = false;
                    txtResult1.Visible = true;
                }
            }
            catch (Exception exp)
            {
                _hosting.ShowError(exp);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void mnuPaste_Click(object sender, EventArgs e)
        {
            txtSource.Text = Clipboard.GetText();
            HandleTree();
        }

        private async void mnuFromWeb_Click(object sender, EventArgs e)
        {
            var frm = new frmWeburl() { Weburl = _weburl };
            if (frm.ShowDialog() == DialogResult.OK)
            {
                _weburl = frm.Weburl;

                try
                {
                    var context = await new HttpClient().GetStringAsync(_weburl);

                    if (txtSource.Text.Length > 0)
                    {
                        _searchItems = null;
                        _searchIndex = 0;
                        _isStarting = true;
                    }

                    txtSource.Text = context;

                    _isNew = true;
                    HandleTree();
                }
                catch (Exception exp)
                {
                    _hosting.ShowError(exp);
                }
            }
        }

        private void mnuCopyKey_Click(object sender, EventArgs e)
        {
            if (treeList1.SelectedItems.Count != 0)
            {
                var item = treeList1.SelectedItems[0];
                Clipboard.SetText(item.Text);
            }
        }

        private void mnuCopy_Click(object sender, EventArgs e)
        {
            if (treeList1.SelectedItems.Count != 0)
            {
                var item = treeList1.SelectedItems[0];
                if (item.Cells[1].Value != null)
                {
                    Clipboard.SetText(item.Cells[1].Text);
                }
            }
        }

        private void mnuView_Click(object sender, EventArgs e)
        {
            HandleTree();
        }

        private void mnuFind_Click(object sender, EventArgs e)
        {
            plnFind.Visible = true;
            txtTreeKeyword.Focus();
        }

        private void mnuMark_Click(object sender, EventArgs e)
        {
            var markText = string.Join("|", _marks);
            var frm = new frmMark { MarkText = markText };
            if (frm.ShowDialog() == DialogResult.OK)
            {
                _markColor = frm.MarkColor ?? Color.Red;

                _marks = frm.MarkText.Split('|').ToList();
                Mark(treeList1.Items, frm.ClearMarks);
            }
        }

        private void btnTreeClose_Click(object sender, EventArgs e)
        {
            _searchItems = null;
            _searchIndex = 0;
            plnFind.Visible = false;
        }

        private void btnTreeFind_Click(object sender, EventArgs e)
        {
            if (_searchItems == null || _lastSearchText != txtTreeKeyword.Text)
            {
                _searchItems = new List<TreeListItem>();
                Find(treeList1.Items, txtTreeKeyword.Text);
                _isStarting = true;
                _lastSearchText = txtTreeKeyword.Text;
                _searchIndex = 0;
            }

            if (_searchItems.Count == 0)
            {
                _hosting.ShowInfo("没有找到你想要的内容。");
                return;
            }

            if (_searchIndex == 0 && !_isStarting)
            {
                if (_hosting.ShowConfirm("已定位最后一个，是否从头开始?") == ShowMsgButton.No)
                {
                    return;
                }
            }

            _searchItems[_searchIndex].EnsureVisible();
            _searchItems[_searchIndex].Selected = true;
            treeList1.Focus();

            if (_searchIndex == _searchItems.Count - 1)
            {
                _searchIndex = 0;
                _isStarting = false;
            }
            else
            {
                _searchIndex++;
            }
        }

        private void Find(TreeListItemCollection items, string keyword)
        {
            foreach (var item in items)
            {
                if (keyword.Contains("=="))
                {
                    var d = keyword.Split(new[] { "==" }, StringSplitOptions.RemoveEmptyEntries);
                    if (item.Text.Equals(d[0].Trim(), StringComparison.InvariantCultureIgnoreCase)
                        && item.Cells[1].Value != null && item.Cells[1].Text.Equals(d[1].Trim(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        _searchItems.Add(item);
                    }
                }

                if (Regex.IsMatch(item.Text, keyword, RegexOptions.IgnoreCase) || (item.Cells[1].Value != null && Regex.IsMatch(item.Cells[1].Text, keyword, RegexOptions.IgnoreCase)))
                {
                    _searchItems.Add(item);
                }
                else
                {
                    Find(item.Items, keyword);
                }
            }
        }

        private void Mark(TreeListItemCollection items, bool clear = true)
        {
            foreach (var item in items)
            {
                TryMark(item, clear);
                Mark(item.Items, clear);
            }
        }

        private bool TryMark(TreeListItem item, bool clear = true)
        {
            if (_marks.Count > 0 && (_marks.Any(s => Regex.IsMatch(item.Text, s, RegexOptions.IgnoreCase)) || (item.Cells[1].Value != null && _marks.Any(s => Regex.IsMatch(item.Cells[1].Text, s, RegexOptions.IgnoreCase)))))
            {
                item.BackgroundColor = _markColor;
                return true;
            }

            if (clear)
            {
                item.BackgroundColor = Color.Empty;
            }

            return false;
        }

        private void treeList1_ItemSelectionChanged(object sender, TreeListItemSelectionEventArgs e)
        {
            if (treeList1.SelectedItems.Count > 0)
            {
                var item = treeList1.SelectedItems[0];
                if (item.Cells[1].Value == null)
                {
                    _hosting.ViewInPropGrid(new { Key = item.Text });
                }
                else
                {
                    _hosting.ViewInPropGrid(new { Key = item.Text, Value = item.Cells[1].Text });
                }
            }
            else
            {
                _hosting.ViewInPropGrid(null);
            }
        }

        private void treeList1_ItemClick(object sender, TreeListItemEventArgs e)
        {
            var str = "";
            var node = e.Item;
            while (node != null)
            {
                str = str.Length == 0 ? node.Text : node.Text + "\\" + str;
                node = node.Parent;
            }

            lblStatus.Text = str;
        }

        private void mnuColor_Click(object sender, EventArgs e)
        {
            if (treeList1.SelectedItems.Count > 0)
            {
                var item = treeList1.SelectedItems[0];
                item.BackgroundColor = ((ToolStripMenuItem)sender).BackColor;
            }
        }

        private void mnuClearMark_Click(object sender, EventArgs e)
        {
            if (treeList1.SelectedItems.Count > 0)
            {
                var item = treeList1.SelectedItems[0];
                item.BackgroundColor = Color.Empty;
            }
        }

        private void mnuExpand_Click(object sender, EventArgs e)
        {
            treeList1.BeginUpdate();
            ExpandNodes(treeList1.Items, true);
            treeList1.EndUpdate();
        }

        private void mnuCollapse_Click(object sender, EventArgs e)
        {
            treeList1.BeginUpdate();
            ExpandNodes(treeList1.Items, false);
            treeList1.EndUpdate();
        }

        private void ExpandNodes(TreeListItemCollection items, bool isExpand)
        {
            foreach (var item in items)
            {
                item.Expended = isExpand;
                ExpandNodes(item.Items, isExpand);
            }
        }

        private void plnFind_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(Pens.LightGray, 0, 0, plnFind.Width - 1, plnFind.Height - 1);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            _popup.Show(this, toolStrip1.Right - panel3.Width, toolStrip1.Location.Y + 26);
        }

        IEnumerable<ToolStripItem> IContextMenuManager.GetContextMenuItems()
        {
            yield return new ToolStripMenuItem("添加桌面快捷方式", null, (o, e) =>
            {
                ToolShortcutHelper.Create(_hosting, "JsonFormatter");
            });
        }
    }
}
