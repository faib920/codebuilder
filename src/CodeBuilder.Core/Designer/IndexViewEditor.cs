// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Source;
using Fireasy.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace CodeBuilder.Core.Designer
{
    public class IndexViewEditor : UITypeEditor
    {
        private IndexView _listView;

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider != null)
            {
                var edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (edSvc == null || (value is List<Index> indexes && indexes.Count == 0))
                {
                    return value;
                }
                if (_listView == null)
                {
                    _listView = new IndexView(this);
                }

                _listView.Start(edSvc, context.PropertyDescriptor.Name, value);
                edSvc.DropDownControl(_listView);
            }

            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        /// <summary>
        /// 设置编辑器是否可调大小。
        /// </summary>
        public override bool IsDropDownResizable
        {
            get
            {
                return true;
            }
        }

        internal class IndexView : TreeList
        {
            private IWindowsFormsEditorService _edSvc;

            public IndexView(UITypeEditor editor)
            {
                ShowHeader = false;
                ShowGridLines = false;
                HotTracking = true;
                BorderStyle = System.Windows.Forms.BorderStyle.None;
                Height = 200;
                Width = 300;

                var column = Columns.Add(string.Empty);
                column.Spring = true;
            }

            public void Start(IWindowsFormsEditorService edSvc, string propertyName, object value)
            {
                _edSvc = edSvc;

                Items.Clear();

                if (value is List<Index> indexes)
                {
                    foreach (var index in indexes)
                    {
                        var item = Items.Add(index.Name);
                        item.Image = index.IsUniqueKey ? Properties.Resources.unique : Properties.Resources.index;

                        foreach (var col in index.Columns)
                        {
                            var sub = item.Items.Add(col.Name);
                            if (string.IsNullOrEmpty(col.SortOrder))
                            {
                                sub.Image = Properties.Resources.item;
                            }
                            else
                            {
                                sub.Image = col.SortOrder == "ASC" ? Properties.Resources.column1 : Properties.Resources.column2;
                            }
                        }

                        item.Expended = true;
                    }
                }
            }

            protected override void OnItemClick(TreeListItemEventArgs e)
            {
                _edSvc.CloseDropDown();
            }
        }
    }
}
