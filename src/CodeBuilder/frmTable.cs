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
using CodeBuilder.Core.Initializers;
using CodeBuilder.Core.Source;
using CodeBuilder.Core.Template;
using CodeBuilder.Core.Validations;
using CodeBuilder.Core.Variable;
using Fireasy.Common.Extensions;
using Fireasy.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CodeBuilder
{
    public partial class frmTable : DockFormBase
    {
        private readonly IDevHosting _hosting;
        private string _fileName;
        private int _filterIndex;
        private List<TreeListItem> _filterItems = null;
        private bool _isLoading = false;
        private Popup _popup1;
        private Dictionary<TreeListItem, Dictionary<string, UpdateFlag>> _updatedBag;
        private Dictionary<TreeListItem, UpdateFlag> _updatedItems;
        private Dictionary<UpdateFlag, bool> _filterOpts = new Dictionary<UpdateFlag, bool>
        {
            { UpdateFlag.Added, false },
            { UpdateFlag.Modified, false },
            { UpdateFlag.Removed, false },
        };
        private Func<TreeListItem, bool> _predicate;

        public enum UpdateFlag
        {
            Added,
            Modified,
            Removed
        }

        public frmTable(IDevHosting hosting)
        {
            InitializeComponent();
            _hosting = hosting;
            InitializeColumns();

            _popup1 = new Popup(panel3) { DropShadowEnabled = false, Font = Font, Resizable = false };
            _popup1.Closed += (o, e) =>
            {
                label2.Text = "6";
            };
            _popup1.Opened += (o, e) =>
            {
                label2.Text = "5";
            };

            _predicate = s => s.Level == 0 ? (_filterOpts.All(t => !t.Value) ? true : _updatedItems.TryGetValue(s, out var sflag) && _filterOpts[sflag]) : true;

            chkFilterMode.Checked = Config.Instance.Source_FilterMode;
            btnNext.Visible = !chkFilterMode.Checked;
            LoadAssistants();
        }

        /// <summary>
        /// 选择列表控件中的表或字段项触发的事件。
        /// </summary>
        public Action<object> SelectItemAct { get; set; }

        /// <summary>
        /// 勾选列表控件中的表或字段项触发的事件。
        /// </summary>
        public Action<int> CheckItemsAct { get; set; }

        public Action<ValidateEntry> ShowValidationAct { get; set; }

        public Action<int, int, int> ShowSynchronizedAct { get; set; }

        private void LoadAssistants()
        {
            var assistants = _hosting.ServiceProvider.GetServices<ISourceAssistant>();

            foreach (var assist in assistants)
            {
                var menuItem = new ToolStripMenuItem(assist.Name);
                menuItem.Tag = assist;
                menuItem.Click += (o, e) =>
                {
                    var ass = (o as ToolStripMenuItem).Tag as ISourceAssistant;
                    if (ass == null)
                    {
                        return;
                    }

                    var tables = lstObject.Items.Select(s => s.DataItem as Table);
                    if (!ass.PreHandle(tables))
                    {
                        return;
                    }

                    var time = Processor.Run(this, calcelToken => ass.HandleAsync(tables, calcelToken));

                    _hosting.HideProgress();

                    ass.PostHandle(new SourceAssistantPostHandleContext(lstObject));
                };

                toolStripMenuItem3.DropDownItems.Add(menuItem);
            }
        }

        private void InitializeColumns()
        {
            if (Config.Instance.Columns?.Count <= 0)
            {
                return;
            }

            lstObject.Columns.Clear();

            var schemaMgr = _hosting.ServiceProvider.TryGetService<ISchemaExtensionManager>();
            var mappers = schemaMgr.GetPropertyMaps<Column>();

            foreach (var name in Config.Instance.Columns)
            {
                var map = mappers.FirstOrDefault(s => s.Name == name);
                if (map == null && !map.IsUICustomized)
                {
                    continue;
                }

                var column = new TreeListColumn
                {
                    DataKey = map.Name,
                    Text = map.DisplayName,
                    Width = map.Width,
                    Editable = name != nameof(Column.Name),
                    Sortable = true,
                    Validatable = false
                };

                var propertyType = map.Type.GetNonNullableType();
                if (propertyType.IsEnum)
                {
                    var editor = new TreeListComboBoxEditor();
                    editor.Inner.DropDownStyle = ComboBoxStyle.DropDownList;
                    foreach (var value in Enum.GetValues(propertyType))
                    {
                        editor.Inner.Items.Add(value);
                    }
                    column.SetEditor(editor);
                }
                else if (propertyType == typeof(bool))
                {
                    column.DataType = TreeListCellDataType.Boolean;
                    column.TextAlign = HorizontalAlignment.Center;
                }
                else if (propertyType.IsNumericType())
                {
                    if (map.Type != typeof(decimal) || map.Type == typeof(double) || map.Type == typeof(float))
                    {
                        column.DataType = TreeListCellDataType.Decimal;
                    }
                    else
                    {
                        column.DataType = TreeListCellDataType.Integer;
                    }
                    column.TextAlign = HorizontalAlignment.Right;
                }

                lstObject.Columns.Add(column);
            }
        }

        public void SetFilterFlag(UpdateFlag flag, bool opt)
        {
            _filterOpts[flag] = opt;

            FindAndFiltering(txtKeyword.Text);
        }

        /// <summary>
        /// 将表结构填充表列表控件中。
        /// </summary>
        /// <param name="tables">数据表列表。</param>
        /// <param name="loadMode">加载模式</param>
        public void FillTables(IEnumerable<Table> tables, LoadMode loadMode = LoadMode.Default)
        {
            if (tables == null)
            {
                return;
            }

            _filterItems = null;
            lblLocCount.Text = string.Empty;
            _updatedBag = new Dictionary<TreeListItem, Dictionary<string, UpdateFlag>>();
            _updatedItems = new Dictionary<TreeListItem, UpdateFlag>();

            _filterOpts[UpdateFlag.Added] = false;
            _filterOpts[UpdateFlag.Modified] = false;
            _filterOpts[UpdateFlag.Removed] = false;


            _isLoading = true;
            lstObject.BeginUpdate();

            try
            {
                InternalFillTables(tables, loadMode);
                CheckItemsAct(lstObject.CheckedItems.Where(s => s.Level == 0).Count());
                ShowSynchronizedAct(_updatedItems.Count(s => s.Value == UpdateFlag.Added), _updatedItems.Count(s => s.Value == UpdateFlag.Modified), _updatedItems.Count(s => s.Value == UpdateFlag.Removed));
            }
            finally
            {
                lstObject.EndUpdate();
                _isLoading = false;
            }
        }

        public void CloseFile()
        {
            if (!string.IsNullOrEmpty(_fileName))
            {
                ClearAll();
                _fileName = null;
            }
        }

        private void InternalFillTables(IEnumerable<Table> tables, LoadMode loadMode = LoadMode.Default)
        {
            var currTableDict = lstObject.Items.ToDictionary(s => (s.DataItem as Table)._Name, s => new { Item = s, Table = s.DataItem as Table });

            var host = tables.FirstOrDefault()?.Host ?? new Host();

            if (loadMode == LoadMode.Default)
            {
                lstObject.Items.Clear();
            }

            TreeListItem lastTableItem = null;

            //循环所有数据表
            foreach (var table in tables)
            {
                if (loadMode == LoadMode.Append)
                {
                    if (currTableDict.ContainsKey(table._Name))
                    {
                        continue;
                    }
                }
                else if (loadMode == LoadMode.Synchronize)
                {
                    if (currTableDict.TryGetValue(table._Name, out var existsItem))
                    {
                        UpdateTable(existsItem.Table, table);

                        lastTableItem = existsItem.Item;

                        var up = new Dictionary<string, UpdateFlag>();
                        var isChanged = false;
                        var existsColumnDict = existsItem.Table.Columns.ToDictionary(s => s._Name);
                        var existsColumnItemDict = (existsItem.Item.Items.Count > 0 || existsItem.Item.IsDemandLoad) ?
                            existsItem.Item.Items.Select(s => new { Item = s, Column = (s.DataItem as Column) }).ToDictionary(s => s.Column._Name) : null;

                        foreach (var column in table.Columns)
                        {
                            if (!existsColumnDict.TryGetValue(column._Name, out var currColumn))
                            {
                                isChanged = true;

                                existsItem.Table.Columns.Add(column);
                                if (existsItem.Item.Items.Count > 0 || existsItem.Item.IsDemandLoad)
                                {
                                    var citem = LoadColumnNode(existsItem.Item, column);
                                    citem.BackgroundColor = Consts.AddedColor;
                                }
                                else
                                {
                                    up.Add(column._Name, UpdateFlag.Added);
                                }
                            }
                            else if (UpdateColumn(currColumn, column))
                            {
                                isChanged = true;

                                if (existsColumnItemDict?.TryGetValue(currColumn._Name, out var existsColumnItem) == true)
                                {
                                    existsColumnItem.Item.BackgroundColor = Consts.ModifiedColor;
                                }
                                else
                                {
                                    up.Add(column._Name, UpdateFlag.Modified);
                                }

                                InitializerUnity.Initialize(_hosting, currColumn);
                            }
                            else if (existsColumnItemDict?.TryGetValue(currColumn._Name, out var existsColumnItem) == true)
                            {
                                if (existsColumnItem.Item.BackgroundColor != Color.Empty)
                                {
                                    existsColumnItem.Item.BackgroundColor = Color.Empty;
                                }
                            }
                        }

                        var currColumnDict = table.Columns.ToDictionary(s => s._Name);

                        foreach (var column in existsItem.Table.Columns)
                        {
                            if (!currColumnDict.TryGetValue(column._Name, out var _))
                            {
                                isChanged = true;

                                if (existsColumnItemDict?.TryGetValue(column._Name, out var existsColumnItem) == true)
                                {
                                    existsColumnItem.Item.BackgroundColor = Consts.RemovedColor;
                                    existsColumnItem.Item.Checked = false;
                                }
                                else
                                {
                                    up.Add(column._Name, UpdateFlag.Removed);
                                }
                            }
                        }

                        if (isChanged)
                        {
                            existsItem.Item.BackgroundColor = Consts.ModifiedColor;
                            _updatedItems.Add(existsItem.Item, UpdateFlag.Modified);

                            if (up.Count > 0)
                            {
                                _updatedBag.Add(existsItem.Item, up);
                            }
                        }
                        else if (existsItem.Item.BackgroundColor != Color.Empty)
                        {
                            existsItem.Item.BackgroundColor = Color.Empty;
                        }

                        continue;
                    }
                }

                host.Attach(table);

                //初始化架构对象，比如格式化类名
                InitializerUnity.Initialize(_hosting, table);

                var titem = new TreeListItem();
                lstObject.Items.Insert(lastTableItem == null ? 0 : lastTableItem.Index + 1, titem);
                titem.Image = table.IsView ? Properties.Resources.view : Properties.Resources.table;
                titem.Checked = true;
                lastTableItem = titem;

                if (table is INotifyPropertyChanged npc)
                {
                    npc.PropertyChanged += (o1, e1) => UpdateObject(titem, e1.PropertyName);
                }

                if (loadMode == LoadMode.Synchronize)
                {
                    _updatedItems.Add(titem, UpdateFlag.Added);
                    titem.BackgroundColor = Consts.AddedColor;
                }

                titem.Bind(table);

                titem.ShowExpanded = table.Columns.Count > 0;
            }

            var newTableDict = tables.ToDictionary(s => s._Name);

            foreach (var kvp in currTableDict)
            {
                if (!newTableDict.ContainsKey(kvp.Value.Table._Name))
                {
                    kvp.Value.Item.BackgroundColor = Consts.RemovedColor;
                    kvp.Value.Item.Checked = false;

                    kvp.Value.Item.Items.ForEach(s => s.Checked = false);

                    _updatedItems.Add(kvp.Value.Item, UpdateFlag.Removed);
                }
            }
        }

        /// <summary>
        /// 获取勾选的数据表对象。
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Table> GetTables(bool filterChecked)
        {
            var rowIndex = 0;

            //循环一级节点（数据表）
            foreach (var titem in lstObject.Items)
            {
                if (filterChecked && !titem.Checked)
                {
                    continue;
                }
                var clTable = (titem.DataItem as Table).Clone();
                if (clTable.Index == 0)
                {
                    clTable.Index = ++rowIndex;
                }
                else
                {
                    rowIndex = clTable.Index;
                }

                var colIndex = 0;

                //没有展开过节点
                if (!titem.IsDemandLoad && clTable.Columns.Count > 0)
                {
                    var savedColumns = new List<Column>(clTable.Columns);
                    clTable.Columns.Clear();

                    foreach (var column in savedColumns)
                    {
                        //初始化架构对象，比如格式化属性名
                        InitializerUnity.Initialize(_hosting, column);

                        if (column.Index == 0)
                        {
                            column.Index = ++colIndex;
                        }
                        else
                        {
                            colIndex = column.Index;
                        }

                        clTable.Columns.Add(column);
                    }

                    savedColumns.Clear();
                }
                else
                {
                    clTable.Columns.Clear();

                    foreach (var citem in titem.Items)
                    {
                        if (filterChecked && !citem.Checked)
                        {
                            continue;
                        }

                        var column = citem.DataItem as Column;
                        if (column.Index == 0)
                        {
                            column.Index = ++colIndex;
                        }
                        else
                        {
                            colIndex = column.Index;
                        }

                        clTable.Columns.Add(column);
                    }
                }

                yield return clTable;
            }
        }

        /// <summary>
        /// 获取表名称列表。
        /// </summary>
        /// <returns></returns>
        public List<string> GetTableNames()
        {
            var list = new List<string>();

            foreach (var titem in lstObject.Items)
            {
                list.Add(titem.Text);
            }

            return list;
        }

        public void InitializeBuildMenu()
        {
            mnuBuildPart.DropDownItems.Clear();

            if (_hosting.Template != null)
            {
                mnuBuildPart.Visible = true;
                FillMenuItems(mnuBuildPart.DropDownItems, _hosting.Template.Groups);
                FillMenuItems(mnuBuildPart.DropDownItems, _hosting.Template.Partitions);
            }
            else
            {
                mnuBuildPart.Visible = false;
            }
        }

        /// <summary>
        /// 更新列表中的内容。
        /// </summary>
        /// <param name="item"></param>
        /// <param name="propertyName"></param>
        public void UpdateObject(TreeListItem item, string propertyName)
        {
            if (item.DataItem is Table table && item.Level == 0)
            {
                if (lstObject.HasSelectedItems && lstObject.SelectedItems[0] == item)
                {
                    SelectItemAct?.Invoke(table);
                }
            }
            if (item.DataItem is Column column && item.Level == 1)
            {
                switch (propertyName)
                {
                    case nameof(Column.IsPrimaryKey):
                    case nameof(Column.ForeignKey):
                        SetColumnItemImage(item, column);
                        break;
                }

                if (lstObject.HasSelectedItems && lstObject.SelectedItems[0] == item)
                {
                    SelectItemAct?.Invoke(column);
                }
            }
        }

        /// <summary>
        /// 应用变量。
        /// </summary>
        public void ApplyProfile()
        {
            UpdateObjectByProfile(lstObject.Items);
            SelectItemAct?.Invoke(lstObject.HasSelectedItems ? lstObject.SelectedItems[0].DataItem : null);
        }

        /// <summary>
        /// 重新构造数据架构。
        /// </summary>
        public void ReBuildSchema()
        {
            if (lstObject.Items.Count == 0)
            {
                return;
            }

            var tables = new List<Table>();
            foreach (var item in lstObject.Items)
            {
                if (item.DataItem is Table table)
                {
                    tables.Add(table);
                }
            }

            var dtables = SchemaUnity.Refactoring(_hosting.ServiceProvider, tables);

            foreach (var item in lstObject.Items)
            {
                if (item.DataItem is Table table)
                {
                    if (dtables.TryGetValue(table._Name, out var ntable))
                    {
                        item.Bind(InitializerUnity.Initialize(_hosting, ntable));

                        foreach (var child in item.Items)
                        {
                            if (child.DataItem is Column column)
                            {
                                var ncolumn = ntable.FindColumn(column._Name);
                                if (ncolumn != null)
                                {
                                    child.Bind(InitializerUnity.Initialize(_hosting, ncolumn));
                                }
                            }
                        }
                    }
                }
            }

            SelectItemAct?.Invoke(lstObject.HasSelectedItems ? lstObject.SelectedItems[0].DataItem : null);
        }

        public string SaveFile(bool isSaveAs = false)
        {
            var tables = GetTables(false);
            if (!tables.Any())
            {
                _hosting.ShowWarn("列表中空空如也，没有什么东西可以保存。");
                return string.Empty;
            }

            var saveFileName = _fileName;

            if (string.IsNullOrEmpty(_fileName) || isSaveAs)
            {
                var filter = FileTypeHelper.GetFilter(".dss");
                if (isSaveAs)
                {
                    filter += "|" + FileTypeHelper.GetFilter(".dso");
                    filter += "|" + FileTypeHelper.GetFilter(".dsr");
                }
                using (var dialog = new SaveFileDialog { Filter = filter })
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        saveFileName = dialog.FileName;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }

            var schemaRepos = _hosting.ServiceProvider.TryGetService<ISchemaRepository>();

            if (saveFileName.EndsWith(".dss", StringComparison.OrdinalIgnoreCase) ||
                saveFileName.EndsWith(".dso", StringComparison.OrdinalIgnoreCase))
            {
                schemaRepos.SaveSchemaFile(saveFileName, tables);
                _fileName = saveFileName;
            }
            else if (saveFileName.EndsWith(".dsr", StringComparison.OrdinalIgnoreCase))
            {
                schemaRepos.SaveRelationFile(saveFileName, tables);
            }

            return _fileName;
        }

        public string OpenFile(string filename)
        {
            var schemaRepos = _hosting.ServiceProvider.TryGetService<ISchemaRepository>();

            if (filename.EndsWith(".dss", StringComparison.OrdinalIgnoreCase) ||
                filename.EndsWith(".dso", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    Cursor = Cursors.WaitCursor;

                    lstObject.Items.Clear();

                    var tables = schemaRepos.ReadFile(filename);

                    FillTables(tables);
                }
                catch (Exception exp)
                {
                    _hosting.ShowError(exp);
                }
                finally
                {
                    Cursor = Cursors.Default;
                }

                _fileName = filename;
                return _fileName;
            }
            else if (filename.EndsWith(".dsr", StringComparison.OrdinalIgnoreCase))
            {
                var tables = GetTables(false);
                if (tables.Any())
                {
                    schemaRepos.ReadRelationFile(filename, tables);
                    RefreshReferences();
                }
                else
                {
                    _hosting.ShowWarn("请先获取数据源或打开架构文件，才能应用关系文件。");
                }
            }

            return string.Empty;
        }

        public void SelectTable(Table table)
        {
            foreach (var item in lstObject.Items)
            {
                if (table.Name == item.Text)
                {
                    item.EnsureVisible();
                    item.Selected = true;
                    return;
                }
            }
        }

        public void SelectColumn(Column column)
        {
            foreach (var titem in lstObject.Items)
            {
                if (column.Owner.Name == titem.Text)
                {
                    if (titem.Items.Count == 0)
                    {
                        titem.ShowExpanded = false;
                        LoadColumnNodes(titem);
                        titem.Expended = true;
                    }

                    foreach (var citem in titem.Items)
                    {
                        if (column.Name == citem.Text)
                        {
                            citem.EnsureVisible();
                            citem.Selected = true;
                            return;
                        }
                    }
                }
            }
        }

        private void ClearAll()
        {
            lstObject.Items.Clear();
            SelectItemAct?.Invoke(null);
            CheckItemsAct(0);

            lblLocCount.Text = "";
            _filterItems?.Clear();
            _filterIndex = 0;

            _updatedBag?.Clear();
            _updatedItems?.Clear();

            _filterOpts[UpdateFlag.Added] = false;
            _filterOpts[UpdateFlag.Modified] = false;
            _filterOpts[UpdateFlag.Removed] = false;

            ShowSynchronizedAct(0, 0, 0);
        }

        /// <summary>
        /// 通用变量递归更新列表中的所有内容。
        /// </summary>
        /// <param name="items"></param>
        private void UpdateObjectByProfile(TreeListItemCollection items)
        {
            foreach (var item in items)
            {
                InitializerUnity.Initialize(_hosting, item.DataItem as SchemaBase);

                UpdateObjectByProfile(item.Items);
            }
        }

        private void RefreshReferences()
        {
            foreach (var titem in lstObject.Items)
            {
                var table = titem.DataItem as Table;

                //循环二级节点（字段）
                foreach (var citem in titem.Items)
                {
                    var column = citem.DataItem as Column;

                    SetColumnItemImage(citem, column);

                    if (citem.Selected)
                    {
                        _hosting.ViewInPropGrid(column);
                    }
                }
            }
        }

        private void FindAndLocation(string keyword)
        {
            _filterItems = new List<TreeListItem>();
            _filterIndex = 0;

            Cursor = Cursors.WaitCursor;
            lstObject.LoadingText = "正在搜索，请稍候...";
            lstObject.BeginUpdate();

            foreach (var item in lstObject.Items)
            {
                var table = item.DataItem as Table;
                var filter = string.IsNullOrEmpty(keyword) || !chkTable.Checked ? false : (Regex.IsMatch(table.Name, keyword, RegexOptions.IgnoreCase) ||
                    (chkRemark.Checked && Regex.IsMatch(table.Description, keyword, RegexOptions.IgnoreCase)));
                if (filter)
                {
                    _filterItems.Add(item);
                }

                var color = filter ? Color.LightSkyBlue : _updatedItems.TryGetValue(item, out UpdateFlag flag) ? GetColor(flag) : Color.Empty;
                if (item.BackgroundColor != color)
                {
                    item.BackgroundColor = color;
                }

                var findColumns = false;
                if (!item.IsDemandLoad)
                {
                    foreach (var column in table.Columns)
                    {
                        if (string.IsNullOrEmpty(keyword) || !chkColumn.Checked ? false : (Regex.IsMatch(column.Name, keyword, RegexOptions.IgnoreCase) ||
                            (chkRemark.Checked && Regex.IsMatch(column.Description, keyword, RegexOptions.IgnoreCase))))
                        {
                            findColumns = true;
                            break;
                        }
                    }

                    if (findColumns)
                    {
                        item.ShowExpanded = false;
                        LoadColumnNodes(item);
                        SearchColumns(keyword, item);
                    }
                }
                else
                {
                    SearchColumns(keyword, item);
                }
            }

            lstObject.EndUpdate();
            lstObject.LoadingText = "正在加载，请稍候...";
            Cursor = Cursors.Default;

            if (_filterItems.Count > 0)
            {
                var item = _filterItems[_filterIndex++];

                lblLocCount.Text = $"第 {_filterIndex} 个，共搜索到 {_filterItems.Count} 个";

                lstObject.Focus();
                if (item.Level > 0 && !item.Parent.Expended)
                {
                    item.Parent.Expended = true;
                }

                item.Selected = true;
                item.EnsureVisible();
            }
            else
            {
                lblLocCount.Text = string.Empty;
            }
        }

        private void FindAndFiltering(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                lstObject.Filtering(_predicate);
                return;
            }
            lstObject.BeginUpdate();
            foreach (var item in lstObject.Items)
            {
                if (item.IsDemandLoad)
                {
                    continue;
                }

                var table = item.DataItem as Table;
                var findColumns = false;
                foreach (var column in table.Columns)
                {
                    if (string.IsNullOrEmpty(keyword) || !chkColumn.Checked ? false : (Regex.IsMatch(column.Name, keyword, RegexOptions.IgnoreCase) ||
                        (chkRemark.Checked && Regex.IsMatch(column.Description, keyword, RegexOptions.IgnoreCase))))
                    {
                        findColumns = true;
                        break;
                    }
                }

                if (findColumns)
                {
                    item.ShowExpanded = false;
                    LoadColumnNodes(item);
                }
            }
            lstObject.EndUpdate();

            var option = new TreeFilterOption
            {
                Filtered = s =>
                {
                    if (s.BackgroundColor != Color.Empty)
                    {
                        if (s.Level == 0 && _updatedItems.TryGetValue(s, out UpdateFlag flag))
                        {
                            s.BackgroundColor = GetColor(flag);
                        }
                        else
                        {
                            s.BackgroundColor = Color.Empty;
                        }
                    }
                }
            };

            lstObject.Filtering(s =>
            {
                if (s.Level == 0)
                {
                    var table = s.DataItem as Table;
                    var isfilter = s.Items.HasVisiableItems || (string.IsNullOrEmpty(keyword) || !chkTable.Checked ? false : (Regex.IsMatch(table.Name, keyword, RegexOptions.IgnoreCase) ||
                        (chkRemark.Checked && Regex.IsMatch(table.Description, keyword, RegexOptions.IgnoreCase))));
                    return isfilter && _predicate(s);
                }
                else
                {
                    var column = s.DataItem as Column;

                    var isfilter = string.IsNullOrEmpty(keyword) || !chkColumn.Checked ? false : (Regex.IsMatch(column.Name, keyword, RegexOptions.IgnoreCase) ||
                        (chkRemark.Checked && Regex.IsMatch(column.Description, keyword, RegexOptions.IgnoreCase)));
                    return isfilter && _predicate(s);
                }
            }, option);
        }

        private void lstObject_ItemSelectionChanged(object sender, TreeListItemSelectionEventArgs e)
        {
            SelectItemAct?.Invoke(lstObject.HasSelectedItems ? lstObject.SelectedItems[0].DataItem : null);
        }

        private void lstObject_BeforeCellEditing(object sender, TreeListBeforeCellEditingEventArgs e)
        {
            if (e.Cell.Item.Level == 0 && e.Cell.Column.DataKey != nameof(Table.Description))
            {
                e.Cancel = true;
            }
        }

        private void lstObject_CellDataUpdated(object sender, TreeListCellDataUpdatedEventArgs e)
        {
            var obj = e.ItemData;

            SelectItemAct?.Invoke(obj);
        }

        private void lstObject_ItemDragOver(object sender, TreeListItemDragOverEventArgs e)
        {
            e.Dragable = ((e.Source.DataItem is Column && e.Item.DataItem is Column && e.Source.Parent == e.Item.Parent) || (e.Source.DataItem is Table && e.Item.DataItem is Table)) && e.Position != DragPosition.Children;
        }

        private void lstObject_AfterItemDragDown(object sender, TreeListAfterItemDragDownEventArgs e)
        {
            if (e.Item.DataItem is Table)
            {
                var index = 0;
                for (var i = 0; i < lstObject.Items.Count; i++)
                {
                    if (lstObject.Items[i].DataItem is Table table)
                    {
                        table.Index = ++index;
                    }
                }
            }
            else
            {
                var index = 0;
                var items = e.Source.Parent.Items;
                for (var i = 0; i < items.Count; i++)
                {
                    if (items[i].DataItem is Column column)
                    {
                        column.Index = ++index;
                    }
                }
            }

            SelectItemAct?.Invoke(e.Source.DataItem);
        }

        private void mnuSelAllTable_Click(object sender, EventArgs e)
        {
            _isLoading = true;

            foreach (var item in lstObject.Items.Where(s => s.Visible))
            {
                item.Checked = true;
            }

            _isLoading = false;

            CheckItemsAct(lstObject.CheckedItems.Where(s => s.Level == 0).Count());
        }

        private void mnuSelInvTable_Click(object sender, EventArgs e)
        {
            _isLoading = true;

            foreach (var item in lstObject.Items.Where(s => s.Visible))
            {
                item.Checked = !item.Checked;
            }

            _isLoading = false;

            CheckItemsAct(lstObject.CheckedItems.Where(s => s.Level == 0).Count());
        }

        /// <summary>
        /// 选择所有对象。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuSelAllColumn_Click(object sender, EventArgs e)
        {
            if (!lstObject.HasSelectedItems)
            {
                return;
            }

            var item = lstObject.SelectedItems[0];
            if (item.DataItem is Column)
            {
                item = item.Parent;
            }

            foreach (var child in item.Items)
            {
                child.Checked = true;
            }
        }

        /// <summary>
        /// 反向选择对象。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuSelInvColumn_Click(object sender, EventArgs e)
        {
            if (!lstObject.HasSelectedItems)
            {
                return;
            }

            var item = lstObject.SelectedItems[0];
            if (item.DataItem is Column)
            {
                item = item.Parent;
            }

            foreach (var child in item.Items)
            {
                child.Checked = !child.Checked;
            }
        }

        /// <summary>
        /// 折叠所有。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuCollaps_Click(object sender, EventArgs e)
        {
            foreach (var item in lstObject.Items.Where(s => s.Visible))
            {
                item.Collapse();
            }
        }

        /// <summary>
        /// 生成预览。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuBuild_Click(object sender, EventArgs e)
        {
            if (_hosting.TemplateProvider == null || _hosting.Template == null)
            {
                _hosting.ShowWarn("你还没有选择生成模板，请从【模板】菜单中选择。");
                return;
            }

            BuildCode(_hosting.Template.GetAllPartitions());
        }

        private void mnuFind_Click(object sender, EventArgs e)
        {
            if (panel1.Visible)
            {
                panel1.Visible = false;
                txtKeyword.Text = string.Empty;
                txtKeyword.Focus();
            }
            else
            {
                panel1.Visible = true;
                txtKeyword.Focus();
            }
        }

        private void lstObject_ItemCheckChanged(object sender, TreeListItemEventArgs e)
        {
            if (_isLoading)
            {
                return;
            }

            CheckItemsAct(lstObject.CheckedItems.Where(s => s.Level == 0).Count());
        }

        private void lstObject_DemandLoad(object sender, TreeListItemEventArgs e)
        {
            LoadColumnNodes(e.Item);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }

        private void FillMenuItems(ToolStripItemCollection items, List<PartitionDefinition> partitions)
        {
            foreach (var par in partitions)
            {
                var menuItem = items.Add(par.Name);
                menuItem.Tag = par;
                menuItem.Click += (o, e) => BuildCode(new List<PartitionDefinition> { (PartitionDefinition)((ToolStripMenuItem)o).Tag });
            }
        }

        private void FillMenuItems(ToolStripItemCollection items, List<GroupDefinition> groups)
        {
            foreach (var g in groups)
            {
                var gitem = new ToolStripMenuItem(g.Name);
                items.Add(gitem);

                FillMenuItems(gitem.DropDownItems, g.Groups);
                FillMenuItems(gitem.DropDownItems, g.Partitions);
            }
        }

        private void BuildCode(List<PartitionDefinition> partitions)
        {
            TemplateDefinition template;
            if (_hosting.TemplateProvider == null ||
                (template = _hosting.Template) == null)
            {
                _hosting.ShowWarn("你还没有选择生成模板，请从【模板】菜单中选择。");
                return;
            }

            if (!lstObject.HasSelectedItems)
            {
                _hosting.ShowWarn("请选择对象列表中的一个表。");
                return;
            }

            var item = lstObject.SelectedItems[0];
            if (item.DataItem is Column)
            {
                item = item.Parent;
            }

            var table = item.DataItem as Table;

            var clTable = table.Clone();
            var colIndex = 0;

            //没有展开过节点
            if (!item.IsDemandLoad && clTable.Columns.Count > 0)
            {
                var savedColumns = new List<Column>(clTable.Columns);
                clTable.Columns.Clear();

                foreach (var column in savedColumns)
                {
                    //初始化架构对象，比如格式化属性名
                    InitializerUnity.Initialize(_hosting, column);

                    column.Index = ++colIndex;
                    clTable.Columns.Add(column);
                }

                savedColumns.Clear();
            }
            else
            {
                clTable.Columns.Clear();

                foreach (var citem in item.Items)
                {
                    if (!citem.Checked)
                    {
                        continue;
                    }

                    var column = citem.DataItem as Column;
                    column.Index = ++colIndex;
                    clTable.Columns.Add(column);
                }
            }

            var ckResult = Util.Validate(_hosting, new[] { table });
            if (!ckResult.IsSuccess)
            {
                Util.ShowValidation(_hosting, ckResult, r => new frmShowValidation(r, ShowValidationAct).Show(_hosting.MainWindow));
                return;
            }

            var option = new TemplateOption();
            option.Template = template;
            option.Partitions = partitions;
            option.DynamicAssemblies.AddRange(StaticUnity.DynamicAssemblies);
            option.Profile = _hosting.Profile;

            var tables = new List<Table> { clTable };

            Cursor = Cursors.WaitCursor;
            try
            {
                var isCancellation = false;
                var time = Processor.Run(this, async calcelToken =>
                {
                    var result = await _hosting.TemplateProvider.GenerateFilesAsync(option, tables, null, calcelToken);
                    if (result != null)
                    {
                        foreach (var rstPart in result.Partitions)
                        {
                            Invoke(new Action(() =>
                            {
                                var editor = new frmEditor(_hosting, CodeCategory.None) { GenerateResult = rstPart };
                                editor.Show(this.DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                            }));
                        }
                    }
                }, () => isCancellation = true);

                if (!isCancellation)
                {
                    _hosting.ConsoleInfo($"代码已生成完毕，共耗时 {time.ToStringEx()}");
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void txtKeyword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F && panel1.Visible)
            {
                btnClose_Click(null, null);
                lstObject.Focus();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                timer1.Enabled = false;
                if (chkFilterMode.Checked)
                {
                    FindAndFiltering(txtKeyword.Text);
                }
                else
                {
                    FindAndLocation(txtKeyword.Text);
                }
            }
            else
            {
                timer1.Stop();
                timer1.Start();
            }
        }

        private void txtKeyword_TextChanged(object sender, EventArgs e)
        {
            label3.Visible = txtKeyword.Text.Length > 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            if (chkFilterMode.Checked)
            {
                FindAndFiltering(txtKeyword.Text);
            }
            else
            {
                FindAndLocation(txtKeyword.Text);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if ((_filterItems?.Count ?? 0) <= 0)
            {
                return;
            }

            if (_filterIndex >= _filterItems.Count)
            {
                _filterIndex = 0;
            }

            var item = _filterItems[_filterIndex++];
            if (item.Level > 0 && !item.Parent.Expended)
            {
                item.Parent.Expended = true;
            }

            lblLocCount.Text = $"第 {_filterIndex} 个，共搜索到 {_filterItems.Count} 个";

            item.Selected = true;
            item.EnsureVisible();
            lstObject.Focus();
        }

        private void mnuColumns_Click(object sender, EventArgs e)
        {
            if (new frmTableCustomize(_hosting).ShowDialog(this) == DialogResult.OK)
            {
                InitializeColumns();
            }
        }

        private void mnuRemove_Click(object sender, EventArgs e)
        {
            if (!lstObject.HasSelectedItems)
            {
                return;
            }

            var item = lstObject.SelectedItems[0];
            if (item.Level == 0)
            {
                var host = lstObject.Items.Count > 0 ? (lstObject.Items[0].DataItem as Table).Host : null;
                var table = item.DataItem as Table;
                host?.Tables.Remove(table);
                lstObject.Items.Remove(item);

                if (_updatedItems.TryGetValue(item, out var flag))
                {
                    _updatedItems.Remove(item);

                    if (!_updatedItems.Any(s => s.Value == flag))
                    {
                        _filterOpts[flag] = false;

                        lstObject.Filtering(_predicate);
                    }

                    CheckItemsAct(lstObject.CheckedItems.Where(s => s.Level == 0).Count());
                    ShowSynchronizedAct(_updatedItems.Count(s => s.Value == UpdateFlag.Added), _updatedItems.Count(s => s.Value == UpdateFlag.Modified), _updatedItems.Count(s => s.Value == UpdateFlag.Removed));
                }
            }
            else if (item.Level == 1)
            {
                var column = item.DataItem as Column;
                (item.Parent.DataItem as Table).Columns.Remove(column);
                item.Parent.Items.Remove(item);
                if (_updatedBag.TryGetValue(item.Parent, out var dic) && dic.Count > 0)
                {
                    dic.Remove(column._Name);

                    if (dic.Count == 0)
                    {
                        _updatedItems.Remove(item.Parent);
                        item.Parent.BackgroundColor = Color.Empty;
                        ShowSynchronizedAct(_updatedItems.Count(s => s.Value == UpdateFlag.Added), _updatedItems.Count(s => s.Value == UpdateFlag.Modified), _updatedItems.Count(s => s.Value == UpdateFlag.Removed));
                    }
                }
            }
        }

        private void mnuClear_Click(object sender, EventArgs e)
        {
            if (lstObject.Items.Count > 0 && _hosting.ShowConfirm("是否清空所有对象?") == ShowMsgButton.Yes)
            {
                ClearAll();
            }
        }

        private void lstObject_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Move : DragDropEffects.None;
        }

        private void lstObject_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (files[0].EndsWith(".dss", StringComparison.OrdinalIgnoreCase))
            {
                OpenFile(files[0]);
            }
        }

        private void SearchColumns(string keyword, TreeListItem item)
        {
            foreach (var citem in item.Items)
            {
                var column = citem.DataItem as Column;
                var filter = string.IsNullOrEmpty(keyword) || !chkColumn.Checked ? false : (Regex.IsMatch(column.Name, keyword, RegexOptions.IgnoreCase) ||
                    (chkRemark.Checked && Regex.IsMatch(column.Description, keyword, RegexOptions.IgnoreCase)));
                if (filter)
                {
                    _filterItems.Add(citem);
                }

                var color = filter ? Color.LightSkyBlue : _updatedBag.TryGetValue(item, out var dic) && dic.TryGetValue(column._Name, out var flag) ? GetColor(flag) : Color.Empty;
                if (citem.BackgroundColor != color)
                {
                    citem.BackgroundColor = color;
                }
            }
        }

        private void LoadColumnNodes(TreeListItem item)
        {
            var table = item.DataItem as Table;
            item.IsDemandLoad = true;

            //循环数据表的字段
            foreach (var column in table.Columns)
            {
                var citem = LoadColumnNode(item, column);
                if (item.BackgroundColor == Consts.AddedColor)
                {
                    citem.BackgroundColor = Consts.AddedColor;
                }
                else if (item.BackgroundColor == Consts.RemovedColor)
                {
                    citem.BackgroundColor = Consts.RemovedColor;
                    citem.Checked = false;
                }
                else if (_updatedBag.TryGetValue(item, out var up))
                {
                    if (up.TryGetValue(column._Name, out var flag))
                    {
                        switch (flag)
                        {
                            case UpdateFlag.Added:
                                citem.BackgroundColor = Consts.AddedColor;
                                break;
                            case UpdateFlag.Modified:
                                citem.BackgroundColor = Consts.ModifiedColor;
                                break;
                            case UpdateFlag.Removed:
                                citem.BackgroundColor = Consts.RemovedColor;
                                citem.Checked = false;
                                break;
                        }
                    }
                }
            }
        }

        private TreeListItem LoadColumnNode(TreeListItem item, Column column)
        {
            var citem = new TreeListItem();
            citem.Bind(column);

            item.Items.Add(citem);

            //初始化架构对象，比如格式化属性名
            InitializerUnity.Initialize(_hosting, column);

            SetColumnItemImage(citem, column);

            if (column is INotifyPropertyChanged npc)
            {
                npc.PropertyChanged += (o1, e1) => UpdateObject(citem, e1.PropertyName);
            }

            citem.Checked = true;

            return citem;
        }

        private void SetColumnItemImage(TreeListItem item, Column column)
        {
            //设置图标
            if (column.ForeignKey != null)
            {
                item.Image = Properties.Resources.fk;
            }
            else if (column.IsPrimaryKey)
            {
                item.Image = Properties.Resources.pk;
            }
            else
            {
                item.Image = Properties.Resources.column;
            }
        }

        private bool UpdateTable(Table currTable, Table newTable)
        {
            var isUpdated = false;
            if (string.IsNullOrWhiteSpace(currTable.Description) && !string.IsNullOrEmpty(newTable.Description))
            {
                currTable.Description = newTable.Description;
                isUpdated = true;
            }

            bool IsSynchronizable(string key)
            {
                return Config.Instance.SyncIgnore == null || !Config.Instance.SyncIgnore.Contains(key);
            }

            if (IsSynchronizable(nameof(Table.Indexes)))
            {
                if (currTable.Indexes.Count != newTable.Indexes.Count)
                {
                    isUpdated = true;
                }

                for (var i = 0; i < currTable.Indexes.Count; i++)
                {
                    if (currTable.Indexes[i].Name != newTable.Indexes[i].Name)
                    {
                        isUpdated = true;
                    }
                    else
                    {
                        for (var j = 0; j < currTable.Indexes[i].Columns.Count; j++)
                        {
                            if (currTable.Indexes[i].Columns[j].Name != newTable.Indexes[i].Columns[j].Name)
                            {
                                isUpdated = true;
                                break;
                            }
                        }
                    }
                }

                currTable.Indexes.Clear();
                currTable.Indexes.AddRange(newTable.Indexes);
            }

            return isUpdated;
        }

        private bool UpdateColumn(Column currColumn, Column newColumn)
        {
            var changed = new List<ChangedProperty>();

            bool IsSynchronizable(string key)
            {
                return Config.Instance.SyncIgnore == null || !Config.Instance.SyncIgnore.Contains(key);
            }

            if (IsSynchronizable(nameof(Column.ColumnType)) && currColumn.ColumnType != newColumn.ColumnType)
            {
                changed.Add(new ChangedProperty(nameof(Column.ColumnType), currColumn.ColumnType, newColumn.ColumnType));
                currColumn.ColumnType = newColumn.ColumnType;
            }

            if (IsSynchronizable(nameof(Column.DataType)) && currColumn.DataType != newColumn.DataType)
            {
                changed.Add(new ChangedProperty(nameof(Column.DataType), currColumn.DataType, newColumn.DataType));
                currColumn.DataType = newColumn.DataType;
            }

            if (IsSynchronizable(nameof(Column.DbType)) && currColumn.DbType != newColumn.DbType)
            {
                changed.Add(new ChangedProperty(nameof(Column.DbType), currColumn.DbType, newColumn.DbType));
                currColumn.DbType = newColumn.DbType;
            }

            if (IsSynchronizable(nameof(Column.DefaultValue)) && currColumn.DefaultValue != newColumn.DefaultValue)
            {
                changed.Add(new ChangedProperty(nameof(Column.DefaultValue), currColumn.DefaultValue, newColumn.DefaultValue));
                currColumn.DefaultValue = newColumn.DefaultValue;
            }

            if (IsSynchronizable(nameof(Column.IsPrimaryKey)) && currColumn.IsPrimaryKey != newColumn.IsPrimaryKey)
            {
                changed.Add(new ChangedProperty(nameof(Column.IsPrimaryKey), currColumn.IsPrimaryKey, newColumn.IsPrimaryKey));
                currColumn.IsPrimaryKey = newColumn.IsPrimaryKey;
            }

            if (IsSynchronizable(nameof(Column.ForeignKey)) && currColumn.ForeignKey != newColumn.ForeignKey)
            {
                changed.Add(new ChangedProperty(nameof(Column.ForeignKey), currColumn.ForeignKey, newColumn.ForeignKey));
                currColumn.UnbindForeignKey();
                currColumn.BindForeignKey(newColumn.ForeignKey);
            }

            if (IsSynchronizable(nameof(Column.IsUniqueKey)) && currColumn.IsUniqueKey != newColumn.IsUniqueKey)
            {
                changed.Add(new ChangedProperty(nameof(Column.IsUniqueKey), currColumn.IsUniqueKey, newColumn.IsUniqueKey));
                currColumn.IsUniqueKey = newColumn.IsUniqueKey;
            }

            if (IsSynchronizable(nameof(Column.IsNullable)) && currColumn.IsNullable != newColumn.IsNullable)
            {
                changed.Add(new ChangedProperty(nameof(Column.IsNullable), currColumn.IsNullable, newColumn.IsNullable));
                currColumn.IsNullable = newColumn.IsNullable;
            }

            if (IsSynchronizable(nameof(Column.AutoIncrement)) && currColumn.AutoIncrement != newColumn.AutoIncrement)
            {
                changed.Add(new ChangedProperty(nameof(Column.AutoIncrement), currColumn.AutoIncrement, newColumn.AutoIncrement));
                currColumn.AutoIncrement = newColumn.AutoIncrement;
            }

            if (IsSynchronizable(nameof(Column.Length)) && currColumn.Length != newColumn.Length)
            {
                changed.Add(new ChangedProperty(nameof(Column.Length), currColumn.Length, newColumn.Length));
                currColumn.Length = newColumn.Length;
            }

            if (IsSynchronizable(nameof(Column.Scale)) && currColumn.Scale != newColumn.Scale)
            {
                changed.Add(new ChangedProperty(nameof(Column.Scale), currColumn.Scale, newColumn.Scale));
                currColumn.Scale = newColumn.Scale;
            }

            if (IsSynchronizable(nameof(Column.Precision)) && currColumn.Precision != newColumn.Precision)
            {
                changed.Add(new ChangedProperty(nameof(Column.Precision), currColumn.Precision, newColumn.Precision));
                currColumn.Precision = newColumn.Precision;
            }

            if (IsSynchronizable(nameof(Column.Description)) && currColumn.Description != newColumn.Description)
            {
                if (string.IsNullOrEmpty(currColumn.Description))
                {
                    changed.Add(new ChangedProperty(nameof(Column.Description), currColumn.Description, newColumn.Description));
                    currColumn.Description = newColumn.Description;
                }
            }

            if (IsSynchronizable(nameof(Column.Charset)) && currColumn.Charset != newColumn.Charset)
            {
                changed.Add(new ChangedProperty(nameof(Column.Charset), currColumn.Charset, newColumn.Charset));
                currColumn.Charset = newColumn.Charset;
            }

            if (IsSynchronizable(nameof(Column.Collation)) && currColumn.Collation != newColumn.Collation)
            {
                changed.Add(new ChangedProperty(nameof(Column.Collation), currColumn.Collation, newColumn.Collation));
                currColumn.Collation = newColumn.Collation;
            }

            currColumn.SetChangeDetails(changed);

            return changed.Any();
        }

        private Color GetColor(UpdateFlag flag)
        {
            switch (flag)
            {
                case UpdateFlag.Added:
                    return Consts.AddedColor;
                case UpdateFlag.Modified:
                    return Consts.ModifiedColor;
                case UpdateFlag.Removed:
                    return Consts.RemovedColor;
                default:
                    return Color.Empty;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            _popup1.Show(label2);
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            var path = new GraphicsPath();
            path.AddLine(0, 5, 0, panel3.Height - 1);
            path.AddLine(0, panel3.Height - 1, panel3.Width - 1, panel3.Height - 1);
            path.AddLine(panel3.Width - 1, panel3.Height - 1, panel3.Width - 1, 5);
            path.AddLine(panel3.Width - 1, 5, 15, 5);
            path.AddLine(15, 5, 10, 0);
            path.AddLine(10, 0, 5, 5);
            path.CloseFigure();
            e.Graphics.FillPath(SystemBrushes.Info, path);
            e.Graphics.DrawPath(Pens.Black, path);
        }

        private void chkFillOpt_CheckedChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtKeyword.Text))
            {
                if (chkFilterMode.Checked)
                {
                    FindAndFiltering(txtKeyword.Text);
                }
                else
                {
                    FindAndLocation(txtKeyword.Text);
                }
            }
        }

        private void chkFilterMode_CheckedChanged(object sender, EventArgs e)
        {
            btnNext.Visible = lblLocCount.Visible = !chkFilterMode.Checked;
            Config.Instance.Source_FilterMode = chkFilterMode.Checked;
            Config.Instance.Save();

            if (string.IsNullOrEmpty(txtKeyword.Text))
            {
                return;
            }

            if (chkFilterMode.Checked)
            {
                FindAndFiltering(txtKeyword.Text);
            }
            else
            {
                lstObject.Filtering(_predicate);
                FindAndLocation(txtKeyword.Text);
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            txtKeyword.Text = string.Empty;

            if (chkFilterMode.Checked)
            {
                FindAndFiltering(txtKeyword.Text);
            }
            else
            {
                lstObject.Filtering(_predicate);
                FindAndLocation(txtKeyword.Text);
            }
        }
    }
}
