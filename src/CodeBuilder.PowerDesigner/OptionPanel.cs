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
using CodeBuilder.Core.Source;
using Fireasy.Common.Extensions;
using Fireasy.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CodeBuilder.PowerDesigner
{
    public partial class OptionPanel : UserControl, IConfigurableControl
    {
        private readonly IDevHosting _hosting;
        private bool _isChanged;
        private string _eventId;

        public OptionPanel(IDevHosting hosting)
        {
            InitializeComponent();
            _hosting = hosting;
        }

        private void OptionPanel_Load(object sender, System.EventArgs e)
        {
            var editor = new TreeListComboBoxEditor();
            editor.Inner.DropDownStyle = ComboBoxStyle.DropDownList;
            foreach (var value in DataTypeManager.GetDatabaseKeys())
            {
                editor.Inner.Items.Add(value);
            }
            treeListColumn2.SetEditor(editor);

            foreach (var map in DBMSManager.GetMappers())
            {
                var item = lstData.Items.Add(map.Key);
                item.Cells[1].Value = map.Value;
            }

            lstData.Items.Add(string.Empty);

            var eventBus = _hosting.ServiceProvider.TryGetService<IEventBusHandler>();
            if (eventBus != null)
            {
                _eventId = eventBus.Subscribe("DataTypeChanged", _ =>
                {
                    editor = (TreeListComboBoxEditor)treeListColumn2.Editor;
                    editor.Inner.Items.Clear();
                    foreach (var value in DataTypeManager.GetDatabaseKeys())
                    {
                        editor.Inner.Items.Add(value);
                    }
                });
            }
        }

        private void lstData_KeyUp(object sender, KeyEventArgs e)
        {
            if (!lstData.HasSelectedItems)
            {
                return;
            }

            var item = lstData.SelectedItems[0];

            if (string.IsNullOrWhiteSpace(item.Text))
            {
                return;
            }

            if (e.KeyCode == Keys.Delete)
            {
                item.Cells[0].Value = string.Empty;
                item.Cells[1].Value = null;
                _isChanged = true;
            }
        }

        private void lstData_AfterCellUpdated(object sender, TreeListAfterCellUpdatedEventArgs e)
        {
            if (e.Cell.Column.Index == 0 && e.NewValue?.ToString().Length > 0)
            {
                e.Cell.Item.Checked = true;
                if (!string.IsNullOrEmpty(lstData.Items[lstData.Items.Count - 1].Cells[0].Text))
                {
                    lstData.Items.Add(string.Empty);
                }

                _isChanged = true;
            }
        }

        bool IConfigurableControl.SaveChanges()
        {
            if (!_isChanged)
            {
                return false;
            }

            var dict = new Dictionary<string, string>();
            foreach (var item in lstData.Items)
            {
                if (!string.IsNullOrEmpty(item.Text) && item.Cells[1].Value != null)
                {
                    if (dict.ContainsKey(item.Text))
                    {
                        item.EnsureVisible();
                        item.Selected = true;

                        _hosting.ShowWarn($"数据库标识 {item.Text} 已经存在。");

                        return false;
                    }

                    var dbType = item.Text;

                    dict.Add(dbType, item.Cells[1].Text);
                }
            }

            DBMSManager.SaveMappers(dict);
            _isChanged = false;

            return true;
        }

        bool IConfigurableControl.IsChanged => _isChanged;

        void IConfigurableControl.Close()
        {
            var eventBus = _hosting.ServiceProvider.TryGetService<IEventBusHandler>();
            if (eventBus != null && !string.IsNullOrWhiteSpace(_eventId))
            {
                eventBus.UnSubscribe("DataTypeChanged", _eventId);
            }

            base.DestroyHandle();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _hosting.Start("DataTypeManageTool", "Dialog");
        }
    }
}
