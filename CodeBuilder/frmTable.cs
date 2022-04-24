// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using CodeBuilder.Core.Initializers;
using CodeBuilder.Core.Source;
using CodeBuilder.Core.Template;
using Fireasy.Common.Extensions;
using Fireasy.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace CodeBuilder
{
    public partial class frmTable : DockForm
    {
        private readonly IDevHosting _hosting;

        public frmTable(IDevHosting hosting)
        {
            InitializeComponent();
            _hosting = hosting;
        }

        /// <summary>
        /// 选择列表控件中的表或字段项触发的事件。
        /// </summary>
        public Action<object> SelectItemAct { get; set; }

        /// <summary>
        /// 勾选列表控件中的表或字段项触发的事件。
        /// </summary>
        public Action<int> CheckItemsAct { get; set; }

        /// <summary>
        /// 将表结构填充表列表控件中。
        /// </summary>
        /// <param name="tables">数据表列表。</param>
        public void FillTables(IEnumerable<IObject> tables)
        {
            if (tables == null)
            {
                return;
            }

            lstObject.BeginUpdate();
            lstObject.Items.Clear();

            //循环所有数据表
            foreach (var t in tables)
            {
                //初始化架构对象，比如格式化类名
                InitializerUnity.Initialize(_hosting, t as SchemaBase);

                var titem = new TreeListItem();
                lstObject.Items.Add(titem);
                titem.Image = (t as Table).IsView ? Properties.Resources.view : Properties.Resources.table;
                titem.Checked = true;
                titem.Tag = t;
                titem.Cells[0].Value = t.Name;
                titem.Cells[1].Value = t.Description;

                //属性变更时，通知列表更新
                t.As<INotifyPropertyChanged>(s => s.PropertyChanged += (o, e) =>
                    {
                        UpdateObject(o, e.PropertyName);
                    });

                //循环数据表的字段
                foreach (var f in t.Fields)
                {
                    var citem = new TreeListItem();
                    titem.Items.Add(citem);

                    if (f is Column c)
                    {
                        //初始化架构对象，比如格式化属性名
                        InitializerUnity.Initialize(_hosting, c);

                        //设置图标
                        if (c.ForeignKey != null)
                        {
                            citem.Image = Properties.Resources.fk;
                        }
                        else if (c.IsPrimaryKey)
                        {
                            citem.Image = Properties.Resources.pk;
                        }
                        else
                        {
                            citem.Image = Properties.Resources.column;
                        }
                    }

                    citem.Checked = true;
                    citem.Tag = f;
                    citem.Cells[0].Value = f.Name;
                    citem.Cells[1].Value = f.Description;

                    //属性变更时，通知列表更新
                    f.As<INotifyPropertyChanged>(s => s.PropertyChanged += (o, e) =>
                        {
                            UpdateObject(o, e.PropertyName);
                        });
                }
            }

            lstObject.EndUpdate();
        }

        /// <summary>
        /// 获取勾选的数据表对象。
        /// </summary>
        /// <returns></returns>
        public List<Table> GetCheckedTables()
        {
            var list = new List<Table>();
            var rowIndex = 0;

            //循环一级节点（数据表）
            foreach (var titem in lstObject.Items)
            {
                if (titem.Checked)
                {
                    var clTable = (titem.Tag as Table).Clone();
                    clTable.Index = ++rowIndex;
                    clTable.Columns.Clear();

                    var colIndex = 0;

                    //循环二级节点（字段）
                    foreach (var citem in titem.Items)
                    {
                        if (citem.Checked)
                        {
                            var column = citem.Tag as Column;
                            column.Index = ++colIndex;
                            clTable.Columns.Add(column);
                        }
                    }

                    list.Add(clTable);
                }
            }

            return list;
        }

        /// <summary>
        /// 更新列表中的内容。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        public void UpdateObject(object obj, string propertyName)
        {
            if (lstObject.SelectedItems.Count == 0)
            {
                return;
            }

            var item = lstObject.SelectedItems[0];

            if (obj is Table)
            {
                var table = obj as Table;
                switch (propertyName)
                {
                    case nameof(Table.Name):
                        item.Cells[0].Value = table.Name;
                        break;
                    case nameof(Table.Description):
                        item.Cells[1].Value = table.Description;
                        break;
                }
            }
            else if (obj is Column)
            {
                var column = obj as Column;
                switch (propertyName)
                {
                    case nameof(Column.Name):
                        item.Cells[0].Value = column.Name;
                        break;
                    case nameof(Column.Description):
                        item.Cells[1].Value = column.Description;
                        break;
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
            SelectItemAct(lstObject.SelectedItems.Count > 0 ? lstObject.SelectedItems[0].Tag : null);
        }

        /// <summary>
        /// 重新构造数据架构。
        /// </summary>
        public void ReBuildSchema()
        {
            var tables = new List<Table>();
            foreach (var item in lstObject.Items)
            {
                var table = item.Tag as Table;
                tables.Add(table);
            }

            tables = SchemaUnity.Refactoring(tables);

            foreach (var item in lstObject.Items)
            {
                var table = item.Tag as Table;
                item.Tag = table = tables.FirstOrDefault(s => s.Name == table.Name); ;

                foreach (var child in item.Items)
                {
                    var column = child.Tag as Column;
                    child.Tag = table.Columns.FirstOrDefault(s => s.Name == column.Name);
                }
            }

            SelectItemAct(lstObject.SelectedItems.Count > 0 ? lstObject.SelectedItems[0].Tag : null);
        }

        /// <summary>
        /// 通用变量递归更新列表中的所有内容。
        /// </summary>
        /// <param name="items"></param>
        private void UpdateObjectByProfile(TreeListItemCollection items)
        {
            foreach (var item in items)
            {
                InitializerUnity.Initialize(_hosting, item.Tag as SchemaBase);

                UpdateObjectByProfile(item.Items);
            }
        }

        private void lstObject_ItemSelectionChanged(object sender, TreeListItemSelectionEventArgs e)
        {
            SelectItemAct?.Invoke(lstObject.SelectedItems.Count > 0 ? lstObject.SelectedItems[0].Tag : null);
        }

        /// <summary>
        /// 单击修改后更新对象的属性。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstObject_AfterCellUpdated(object sender, TreeListAfterCellUpdatedEventArgs e)
        {
            var obj = e.Cell.Item.Tag;

            if (obj is Table table)
            {
                table.Description = e.NewValue.ToString();
            }
            else if (obj is Column column)
            {
                column.Description = e.NewValue.ToString();
            }

            SelectItemAct?.Invoke(obj);
        }

        private void mnuSelAllTable_Click(object sender, EventArgs e)
        {
            foreach (var item in lstObject.Items)
            {
                item.Checked = true;
            }

            CheckItemsAct(lstObject.CheckedItems.Where(s => s.Tag is Table).Count());
        }

        private void mnuSelInvTable_Click(object sender, EventArgs e)
        {
            foreach (var item in lstObject.Items)
            {
                item.Checked = !item.Checked;
            }

            CheckItemsAct(lstObject.CheckedItems.Where(s => s.Tag is Table).Count());
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
            if (item.Tag is Column)
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
            if (item.Tag is Column)
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
            TemplateDefinition template;
            if (_hosting.TemplateProvider == null ||
                (template = _hosting.Template) == null)
            {
                _hosting.ShowWarn("你还没有选择生成模板，请从【模板】菜单中选择。");
                return;
            }

            if (lstObject.SelectedItems.Count == 0)
            {
                return;
            }

            var item = lstObject.SelectedItems[0];
            if (item.Tag is Column)
            {
                item = item.Parent;
            }

            var table = item.Tag as Table;

            var ckResult = ValidationUnity.Validate(_hosting.Profile, new[] { table });
            if (!string.IsNullOrEmpty(ckResult))
            {
                _hosting.ShowWarn(ckResult);
                return;
            }

            var option = new TemplateOption();
            option.Template = template;
            option.Partitions = _hosting.Template.GetAllPartitions();
            option.DynamicAssemblies.AddRange(StaticUnity.DynamicAssemblies);
            option.Profile = _hosting.Profile;

            var clTable = table.Clone();
            clTable.Columns.Clear();
            var colIndex = 0;

            foreach (var citem in item.Items)
            {
                if (citem.Checked)
                {
                    var column = citem.Tag as Column;
                    column.Index = ++colIndex;
                    clTable.Columns.Add(column);
                }
            }

            var tables = new List<Table> { clTable };

            Cursor = Cursors.WaitCursor;
            try
            {
                var result = _hosting.TemplateProvider.GenerateFiles(option, tables, null);
                if (result != null)
                {
                    foreach (var file in result)
                    {
                        var editor = new frmEditor (_hosting) { GenerateResult = file };
                        editor.Show(this.DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                    }
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void lstObject_ItemCheckChanged(object sender, TreeListItemEventArgs e)
        {
            CheckItemsAct(lstObject.CheckedItems.Where(s => s.Tag is Table).Count());
        }
    }
}
