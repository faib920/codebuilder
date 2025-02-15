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
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace CodeBuilder.Core.Designer
{
    internal class ChangedPropertyEditor : UITypeEditor
    {
        private CollectionView _listView;

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider != null)
            {
                var edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (edSvc == null || (value is ICollection collection && collection.Count == 0))
                {
                    return value;
                }
                if (_listView == null)
                {
                    _listView = new CollectionView(this);
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

        internal class CollectionView : TreeList
        {
            private IWindowsFormsEditorService _edSvc;

            public CollectionView(UITypeEditor editor)
            {
                ShowGridLines = false;
                HotTracking = true;
                BorderStyle = System.Windows.Forms.BorderStyle.None;
                Height = 300;
                Width = 400;

                var column1 = Columns.Add("名称");
                column1.Width = 140;
                var column2 = Columns.Add("原值");
                column2.Width = 120;
                var column3 = Columns.Add("新值");
                column3.Width = 120;
            }

            public void Start(IWindowsFormsEditorService edSvc, string propertyName, object value)
            {
                _edSvc = edSvc;

                Items.Clear();

                if (value is IEnumerable enumerable)
                {
                    foreach (var col in enumerable)
                    {
                        if (col is ChangedProperty property)
                        {
                            var item = Items.Add(property.Name);
                            item.Image = Properties.Resources.item;
                            item.Cells[1].Value = property.OldValue;
                            item.Cells[2].Value = property.NewValue;
                        }
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
