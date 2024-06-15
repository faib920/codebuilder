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
using System.Threading.Tasks;
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
        private Popup _popup;

        public frmTable(IDevHosting hosting)
        {
            InitializeComponent();
            _hosting = hosting;
            InitializeColumns();

            _popup = new Popup(panel3) { DropShadowEnabled = false, Font = Font, Resizable = false };
            _popup.Closed += (o, e) =>
            {
                label2.Text = "6";
            };
            _popup.Opened += (o, e) =>
            {
                label2.Text = "5";
            };
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
                    Sortable = name == nameof(Column.Name) || name == nameof(Column.Description),
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

        /// <summary>
        /// 将表结构填充表列表控件中。
        /// </summary>
        /// <param name="tables">数据表列表。</param>
        /// <param name="append">是否追加模式</param>
        /// <param name="isNew">初始化。</param>
        public void FillTables(IEnumerable<IObject> tables, bool append = false, bool isNew = true)
        {
            if (tables == null)
            {
                return;
            }

            if (isNew)
            {
                _fileName = null;
            }

            _filterItems = null;
            lblLocCount.Text = string.Empty;

            _isLoading = true;
            lstObject.BeginUpdate();

            var host = append && lstObject.Items.Count > 0 ? (lstObject.Items[0].DataItem as Table).Host : null;

            if (!append)
            {
                lstObject.Items.Clear();
            }

            //循环所有数据表
            foreach (var table in tables)
            {
                if (append)
                {
                    if (lstObject.Items.Any(s => s.Text == table.Name))
                    {
                        continue;
                    }

                    host.Attach((Table)table);
                }

                //初始化架构对象，比如格式化类名
                InitializerUnity.Initialize(_hosting, table as SchemaBase);

                var titem = new TreeListItem();
                lstObject.Items.Add(titem);
                titem.Image = (table as Table).IsView ? Properties.Resources.view : Properties.Resources.table;
                titem.Checked = true;

                titem.Bind(table);

                titem.ShowExpanded = table.Fields.Count > 0;
            }

            lstObject.EndUpdate();
            _isLoading = false;
            CheckItemsAct(lstObject.CheckedItems.Where(s => s.DataItem is Table).Count());
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
                clTable.Index = ++rowIndex;

                var colIndex = 0;

                //没有展开过节点
                if (titem.Items.Count == 0 && clTable.Columns.Count > 0)
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

                    foreach (var citem in titem.Items)
                    {
                        if (filterChecked && !citem.Checked)
                        {
                            continue;
                        }

                        var column = citem.DataItem as Column;
                        column.Index = ++colIndex;
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
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        public void UpdateObject(string propertyName)
        {
            if (lstObject.SelectedItems.Count == 0)
            {
                return;
            }

            var item = lstObject.SelectedItems[0];

            if (item.DataItem is Column column && item.Level == 1)
            {
                switch (propertyName)
                {
                    case nameof(Column.IsPrimaryKey):
                    case nameof(Column.ForeignKey):
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

                        break;
                }
            }
        }

        /// <summary>
        /// 应用变量。
        /// </summary>
        public void ApplyProfile()
        {
            UpdateObjectByProfile(lstObject.Items);
            SelectItemAct(lstObject.SelectedItems.Count > 0 ? lstObject.SelectedItems[0].DataItem : null);
        }

        /// <summary>
        /// 重新构造数据架构。
        /// </summary>
        public void ReBuildSchema()
        {
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

            SelectItemAct(lstObject.SelectedItems.Count > 0 ? lstObject.SelectedItems[0].DataItem : null);
        }

        public string SaveFile(bool isSaveAs = false)
        {
            var tables = GetTables(false);
            if (!tables.Any())
            {
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
                    var tables = schemaRepos.ReadFile(filename);

                    FillTables(tables, isNew: false);
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

                    //设置图标
                    if (column.ForeignKey != null)
                    {
                        citem.Image = Properties.Resources.fk;
                    }
                    else if (column.IsPrimaryKey)
                    {
                        citem.Image = Properties.Resources.pk;
                    }
                    else
                    {
                        citem.Image = Properties.Resources.column;
                    }

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
                var filter = string.IsNullOrEmpty(keyword) || !chkTable.Checked ? false : Regex.IsMatch(item.Text, keyword, RegexOptions.IgnoreCase) ||
                    (chkRemark.Checked && Regex.IsMatch(item.Cells[1].Text, keyword, RegexOptions.IgnoreCase));
                if (filter)
                {
                    _filterItems.Add(item);
                }

                var color = filter ? Color.LightSkyBlue : Color.Empty;
                if (item.BackgroundColor != color)
                {
                    item.BackgroundColor = color;
                }

                var table = item.DataItem as Table;
                var findColumns = false;
                if (item.Items.Count == 0)
                {
                    foreach (var column in table.Columns)
                    {
                        if (string.IsNullOrEmpty(keyword) || !chkColumn.Checked ? false : Regex.IsMatch(column.Name, keyword, RegexOptions.IgnoreCase) ||
                            (chkRemark.Checked && Regex.IsMatch(column.Description, keyword, RegexOptions.IgnoreCase)))
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

        private void lstObject_ItemSelectionChanged(object sender, TreeListItemSelectionEventArgs e)
        {
            SelectItemAct?.Invoke(lstObject.SelectedItems.Count > 0 ? lstObject.SelectedItems[0].DataItem : null);
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

            foreach (var item in lstObject.Items)
            {
                item.Checked = true;
            }

            _isLoading = false;

            CheckItemsAct(lstObject.CheckedItems.Where(s => s.DataItem is Table).Count());
        }

        private void mnuSelInvTable_Click(object sender, EventArgs e)
        {
            _isLoading = true;

            foreach (var item in lstObject.Items)
            {
                item.Checked = !item.Checked;
            }

            _isLoading = false;

            CheckItemsAct(lstObject.CheckedItems.Where(s => s.DataItem is Table).Count());
        }

        /// <summary>
        /// 选择所有对象。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuSelAllColumn_Click(object sender, EventArgs e)
        {
            if (lstObject.SelectedItems.Count == 0)
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
            if (lstObject.SelectedItems.Count == 0)
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
                lstObject.Focus();
                panel1.Visible = false;
            }
            else
            {
                panel1.Visible = true;
                txtKeyword.Focus();
            }
        }

        private void mnuPrimaryKey_Click(object sender, EventArgs e)
        {
            var count = 0;
            var isCancellation = false;
            var assistant = _hosting.ServiceProvider.GetRequiredService<ISourceAssistant>();

            var time = Processor.Run(this, calcelToken =>
            {
                var tables = lstObject.Items.Select(s => s.DataItem as Table);
                var columns = new List<Column>();

                foreach (var table in tables)
                {
                    if (calcelToken.IsCancellationRequested)
                    {
                        return Task.CompletedTask;
                    }

                    foreach (var column in table.Columns.Where(s => !s.IsPrimaryKey))
                    {
                        if (calcelToken.IsCancellationRequested)
                        {
                            return Task.CompletedTask;
                        }

                        if (assistant.IsPrimaryKey(column))
                        {
                            columns.Add(column);
                        }
                    }
                }

                foreach (var column in columns)
                {
                    column.IsPrimaryKey = true;
                    count++;
                }

                return Task.CompletedTask;
            }, () => isCancellation = true);

            if (isCancellation)
            {
                return;
            }

            if (count > 0)
            {
                RefreshReferences();

                _hosting.ShowInfo($"一共发现并自动创建了 {count} 个主键。");
            }
        }

        private void mnuRelation_Click(object sender, EventArgs e)
        {
            var count = 0;
            var isCancellation = false;

            var assistant = _hosting.ServiceProvider.GetRequiredService<ISourceAssistant>();

            var time = Processor.Run(this, calcelToken =>
            {
                var tables = lstObject.Items.Select(s => s.DataItem as Table).ToList();
                var references = new Dictionary<Column, Reference>();
                var index = 0;
                foreach (var table in tables)
                {
                    if (calcelToken.IsCancellationRequested)
                    {
                        return Task.CompletedTask;
                    }

                    var p = (int)((++index / (tables.Count * 1.0)) * 100);
                    _hosting.ShowProgress($"{p}% 正在检索表 {table.Name}...", p);

                    foreach (var column in table.Columns)
                    {
                        if (calcelToken.IsCancellationRequested)
                        {
                            return Task.CompletedTask;
                        }

                        var reference = assistant.FindForeignKey(column, tables);
                        if (reference != null)
                        {
                            references.Add(column, reference);
                        }
                    }
                }

                foreach (var kvp in references)
                {
                    if (kvp.Key.BindForeignKey(kvp.Value))
                    {
                        count++;
                    }
                }

                return Task.CompletedTask;
            }, () => isCancellation = true);

            if (isCancellation)
            {
                return;
            }

            if (count > 0)
            {
                RefreshReferences();

                _hosting.ShowInfo($"一共发现并自动创建了 {count} 个外键。");
            }
        }

        private void mnuClearRelation_Click(object sender, EventArgs e)
        {
            if (_hosting.ShowConfirm("确认清理所有外键关系吗?") == ShowMsgButton.No)
            {
                return;
            }

            var tables = lstObject.Items.Select(s => s.DataItem as Table);
            var count = 0;

            foreach (var table in tables)
            {
                foreach (var column in table.Columns)
                {
                    if (column.ForeignKey != null)
                    {
                        column.UnbindForeignKey();
                        count++;
                    }
                }
            }

            if (count > 0)
            {
                RefreshReferences();
            }

            _hosting.ShowInfo("所有外键关系已被清理，你可以使用【创建外键关系】来重新生成。");
        }

        private void lstObject_ItemCheckChanged(object sender, TreeListItemEventArgs e)
        {
            if (_isLoading)
            {
                return;
            }

            CheckItemsAct(lstObject.CheckedItems.Where(s => s.DataItem is Table).Count());
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

            if (lstObject.SelectedItems.Count == 0)
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
            if (item.Items.Count == 0 && clTable.Columns.Count > 0)
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
                FindAndLocation(txtKeyword.Text);
            }
            else
            {
                timer1.Stop();
                timer1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            FindAndLocation(txtKeyword.Text);
        }

        private void btnLocation_Click(object sender, EventArgs e)
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

        private void mnuClear_Click(object sender, EventArgs e)
        {
            if (lstObject.Items.Count > 0 && _hosting.ShowConfirm("是否清空所有对象?") == ShowMsgButton.Yes)
            {
                lstObject.Items.Clear();
                SelectItemAct(null);
                CheckItemsAct(0);

                lblLocCount.Text = "";
                _filterItems?.Clear();
                _filterIndex = 0;
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
                var filter = string.IsNullOrEmpty(keyword) ? false : Regex.IsMatch(citem.Text, keyword, RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(citem.Cells[1].Text, keyword, RegexOptions.IgnoreCase);
                if (filter)
                {
                    _filterItems.Add(citem);
                }

                var color = filter ? Color.LightSkyBlue : Color.Empty;
                if (citem.BackgroundColor != color)
                {
                    citem.BackgroundColor = color;
                }
            }
        }

        private void LoadColumnNodes(TreeListItem item)
        {
            var table = item.DataItem as Table;

            //循环数据表的字段
            foreach (var column in table.Columns)
            {
                var citem = new TreeListItem();
                item.Items.Add(citem);

                //初始化架构对象，比如格式化属性名
                InitializerUnity.Initialize(_hosting, column);

                //设置图标
                if (column.ForeignKey != null)
                {
                    citem.Image = Properties.Resources.fk;
                }
                else if (column.IsPrimaryKey)
                {
                    citem.Image = Properties.Resources.pk;
                }
                else
                {
                    citem.Image = Properties.Resources.column;
                }

                if (column is INotifyPropertyChanged npc)
                {
                    npc.PropertyChanged += (o1, e1) => UpdateObject(e1.PropertyName);
                }

                citem.Checked = true;

                citem.Bind(column);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            if (_popup.IsOpened)
            {
                return;
            }

            _popup.Show(label2);
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
    }
}
