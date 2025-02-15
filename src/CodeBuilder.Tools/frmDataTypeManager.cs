// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using CodeBuilder.Core.EventBus;
using CodeBuilder.Core.Forms;
using CodeBuilder.Core.Source;
using Fireasy.Common.Extensions;
using Fireasy.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CodeBuilder.Tools
{
    public partial class frmDataTypeManager : DockFormBase, IContextMenuManager, ICloseManager
    {
        private string _selectDatabase;
        private readonly IDevHosting _hosting;
        private Color _regColor = Color.FromArgb(200, 200, 225);
        private bool _isChanged = false;
        private Popup _popup;

        public frmDataTypeManager(IDevHosting hosting)
        {
            InitializeComponent();
            _hosting = hosting;
            Icon = Util.GetIcon();
            _popup = new Popup(panel3) { DropShadowEnabled = false, Font = Font, Resizable = false };
        }

        IEnumerable<ToolStripItem> IContextMenuManager.GetContextMenuItems()
        {
            yield break;
        }

        private void frmDataTypeManager_Load(object sender, System.EventArgs e)
        {
            var editor = new TreeListComboBoxEditor();
            editor.Inner.DropDownStyle = ComboBoxStyle.DropDownList;
            foreach (var value in typeof(DbType).GetEnumValues())
            {
                editor.Inner.Items.Add(value);
            }
            treeListColumn2.SetEditor(editor);

            LoadDatabases();
        }

        private void LoadDatabases()
        {
            mnuDatabase.DropDownItems.Clear();

            foreach (var key in DataTypeManager.GetDatabaseKeys())
            {
                var item = new ToolStripMenuItem(key);
                mnuDatabase.DropDownItems.Add(item);

                if (item.Text == _selectDatabase)
                {
                    mnuDatabase_Click(item, new EventArgs());
                }

                item.Click += mnuDatabase_Click;
            }

            if (string.IsNullOrEmpty(_selectDatabase))
            {
                mnuDatabase_Click(mnuDatabase.DropDownItems[0], new EventArgs());
            }
        }

        private void LoadDataTypes(string database)
        {
            lstDataType.EndEdit();
            lstDataType.Items.Clear();

            foreach (var kvp in DataTypeManager.GetDataTypes(database))
            {
                var item = new TreeListItem(kvp.Key.Replace("@", string.Empty));
                lstDataType.Items.Add(item);
                item.Cells[1].Value = kvp.Value;
                if (kvp.Key.StartsWith("@"))
                {
                    item.BackgroundColor = _regColor;
                }
            }

            lstDataType.Items.Add(string.Empty);
        }

        private void SaveDataTypes()
        {
            var dict = new Dictionary<string, DbType>();
            foreach (var item in lstDataType.Items)
            {
                if (!string.IsNullOrEmpty(item.Text) && item.Cells[1].Value != null)
                {
                    if (dict.ContainsKey(item.Text))
                    {
                        item.EnsureVisible();
                        item.Selected = true;

                        _hosting.ShowWarn($"数据类型 {item.Text} 已经存在。");

                        return;
                    }

                    var dbType = item.Text;
                    if (item.BackgroundColor == _regColor)
                    {
                        dbType = "@" + dbType;
                    }

                    dict.Add(dbType, (DbType)item.Cells[1].Value);
                }
            }

            DataTypeManager.SaveDataTypes(_selectDatabase, dict);
            _isChanged = false;
        }

        private void mnuDatabase_Click(object sender, EventArgs e)
        {
            if (_isChanged && _hosting.ShowConfirm($"数据库配置 {_selectDatabase} 已修改，是否保存?") == ShowMsgButton.Yes)
            {
                SaveDataTypes();
            }

            var item = sender as ToolStripMenuItem;
            mnuDatabase.Text = item.Text;
            _selectDatabase = item.Text;
            item.ForeColor = Color.Blue;

            foreach (ToolStripMenuItem sitem in mnuDatabase.DropDownItems)
            {
                if (item != sitem)
                {
                    sitem.ForeColor = Color.Empty;
                }
            }

            LoadDataTypes(_selectDatabase);
            _isChanged = false;
        }

        private void mnuAddDb_Click(object sender, EventArgs e)
        {
            using (var dialog = new frmNewDatabase(_hosting))
            {
                dialog.CheckFunc = s => DataTypeManager.GetDatabaseKeys().Contains(s);

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    var item = new ToolStripMenuItem(dialog.Database);
                    mnuDatabase.DropDownItems.Add(item);
                    mnuDatabase_Click(item, new EventArgs());

                    item.Click += mnuDatabase_Click;
                }
            }
        }

        private void mnuDelDb_Click(object sender, EventArgs e)
        {
            if (_hosting.ShowConfirm($"确定的要删除数据库配置 {_selectDatabase} 吗?") == ShowMsgButton.Yes)
            {
                DataTypeManager.SaveDataTypes(_selectDatabase, null);
                _selectDatabase = null;
                LoadDatabases();

                var eventBus = _hosting.ServiceProvider.TryGetService<IEventBusHandler>();
                if (eventBus != null)
                {
                    eventBus.Publish("DataTypeChanged");
                }
            }
        }

        private void mnuSave_Click(object sender, EventArgs e)
        {
            if (_hosting.ShowConfirm($"确定的要保存数据库配置 {_selectDatabase} 吗?") == ShowMsgButton.No)
            {
                return;
            }

            SaveDataTypes();

            var eventBus = _hosting.ServiceProvider.TryGetService<IEventBusHandler>();
            if (eventBus != null)
            {
                eventBus.Publish("DataTypeChanged");
            }

            _isChanged = false;
        }

        private void lstDataType_KeyUp(object sender, KeyEventArgs e)
        {
            if (!lstDataType.HasSelectedItems)
            {
                return;
            }

            var item = lstDataType.SelectedItems[0];

            if (string.IsNullOrWhiteSpace(item.Text))
            {
                return;
            }

            if (e.KeyCode == Keys.Delete)
            {
                item.Cells[0].Value = string.Empty;
                item.Cells[1].Value = null;
                item.BackgroundColor = Color.Empty;
                _isChanged = true;
            }
            else if (e.KeyCode == Keys.F9)
            {
                if (item.BackgroundColor == Color.Empty)
                {
                    item.BackgroundColor = _regColor;
                }
                else
                {
                    item.BackgroundColor = Color.Empty;
                }
                _isChanged = true;
            }
        }

        private void mnuJson_Click(object sender, EventArgs e)
        {
            var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "datatypes.cfg");
            _hosting.OpenFile(fileName, "JSON");
        }

        private void lstDataType_AfterCellUpdated(object sender, TreeListAfterCellUpdatedEventArgs e)
        {
            if (e.Cell.Column.Index == 0 && e.NewValue?.ToString().Length > 0)
            {
                e.Cell.Item.Checked = true;
                if (!string.IsNullOrEmpty(lstDataType.Items[lstDataType.Items.Count - 1].Cells[0].Text))
                {
                    lstDataType.Items.Add(string.Empty);
                }
            }

            _isChanged = true;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            _popup.Show(this, toolStrip1.Right - panel3.Width, toolStrip1.Location.Y + 26);
        }
    }
}
