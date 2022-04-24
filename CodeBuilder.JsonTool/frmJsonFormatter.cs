// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using Fireasy.Common.Extensions;
using Fireasy.Common.Serialization;
using Fireasy.Windows.Forms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace CodeBuilder.JsonTool
{
    public partial class frmJsonFormatter : DockContent, IClosableDockManaged
    {
        private bool _isNew = false;
        private List<TreeListItem> _searchItems;
        private int _searchIndex = 0;
        private string _lastSearchText;
        private bool _isStarting = true;
        private List<string> _marks = new List<string>();
        private string _weburl;
        private readonly IDevHosting _hosting;

        public frmJsonFormatter(IDevHosting hosting)
        {
            InitializeComponent();
            _hosting = hosting;
        }

        private bool Parse(TreeListItemCollection items, object obj)
        {
            if (obj is IDictionary<string, object> dic)
            {
                foreach (var kvp in dic)
                {
                    var item = items.Add(kvp.Key);
                    item.ImageIndex = 2;

                    if (kvp.Value is IDictionary<string, object> subdic)
                    {
                        item.ImageIndex = 1;
                        Parse(item.Items, kvp.Value);
                    }
                    else if (kvp.Value is IEnumerable enum2 && !(kvp.Value is string))
                    {
                        item.ImageIndex = 0;
                        var i = 0;
                        foreach (var v in enum2)
                        {
                            var subitem = item.Items.Add("[" + i++ + "]");
                            subitem.ImageIndex = 1;
                            if (!Parse(subitem.Items, v))
                            {
                                subitem.Cells[1].Value = v.ToStringSafely();
                            }

                            subitem.Expended = true;
                        }

                        item.Cells[1].Value = "(" + item.Items.Count + "项)";
                    }
                    else
                    {
                        item.Cells[1].Value = kvp.Value == null ? "<null>" : kvp.Value.ToStringSafely();
                    }

                    item.Expended = true;
                    TryMark(item);
                }

                return true;
            }
            else if (obj is IEnumerable enum1 && !(obj is string))
            {
                var i = 0;
                foreach (var v in enum1)
                {
                    var subitem = items.Add(i++.ToString());
                    subitem.ImageIndex = 1;
                    if (!Parse(subitem.Items, v))
                    {
                        subitem.Cells[1].Value = "[" + v.ToStringSafely() + "]";
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

                AddHistory();
            }
        }

        private void AddHistory()
        {
            if (mnuHistory.DropDownItems.Count == 0)
            {
                var clear = mnuHistory.DropDownItems.Add("清空历史");
                clear.Click += (o1, e1) => mnuHistory.DropDownItems.Clear();
            }

            var menu = mnuHistory.DropDownItems.Add(txtSource.Text.Left(80).Replace("\n", string.Empty) + (txtSource.Text.Length > 80 ? "..." : string.Empty));
            menu.Tag = txtSource.Text;
            menu.Click += (o1, e1) =>
            {
                if (_isNew)
                {
                    AddHistory();
                }

                _isNew = false;
                txtSource.Text = menu.Tag.ToString();
                HandleTree();
            };
        }

        private void HandleTree()
        {
            try
            {
                var serializer = new JsonSerializer(new JsonSerializeOption { Indent = true });
                var obj = serializer.Deserialize<dynamic>(txtSource.Text);

                treeList1.Items.Clear();
                treeList1.BeginUpdate();
                Parse(treeList1.Items, obj);
                treeList1.EndUpdate();

                txtResult.Text = serializer.Serialize(obj);
            }
            catch (Exception exp)
            {
                _hosting.ShowError(exp.Message);
            }
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

                        AddHistory();
                    }

                    txtSource.Text = context;

                    _isNew = true;
                    HandleTree();
                }
                catch (Exception exp)
                {
                    _hosting.ShowError(exp.Message);
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
                if (frm.MarkText != markText)
                {
                    _marks = frm.MarkText.Split('|').ToList();
                    Mark(treeList1.Items);
                }
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

                if (item.Text.Equals(keyword, StringComparison.InvariantCultureIgnoreCase) || (item.Cells[1].Value != null && item.Cells[1].Text.Equals(keyword, StringComparison.CurrentCultureIgnoreCase)))
                {
                    _searchItems.Add(item);
                }
                else
                {
                    Find(item.Items, keyword);
                }
            }
        }

        private void Mark(TreeListItemCollection items)
        {
            foreach (var item in items)
            {
                TryMark(item);
                Mark(item.Items);
            }
        }

        private bool TryMark(TreeListItem item)
        {
            if (_marks.Count > 0 && (_marks.Contains(item.Text, new StringEqualityComparer()) || (item.Cells[1].Value != null && _marks.Contains(item.Cells[1].Text, new StringEqualityComparer()))))
            {
                item.Highlight = true;
                return true;
            }

            item.Highlight = false;
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

        private class StringEqualityComparer : EqualityComparer<string>
        {
            public override bool Equals(string x, string y)
            {
                return string.Equals(x, y, StringComparison.CurrentCultureIgnoreCase);
            }

            public override int GetHashCode(string obj)
            {
                return obj.GetHashCode();
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
    }
}
