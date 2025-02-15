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
    public class SubKeyViewEditor : UITypeEditor
    {
        private ReferenceView _listView;

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider != null)
            {
                var edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (edSvc == null || (value is List<Reference> referehces && referehces.Count == 0))
                {
                    return value;
                }
                if (_listView == null)
                {
                    _listView = new ReferenceView(this);
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

        internal class ReferenceView : TreeList
        {
            private IWindowsFormsEditorService _edSvc;

            public ReferenceView(UITypeEditor editor)
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

                if (value is List<Reference> references)
                {
                    foreach (var fk in references)
                    {
                        var item = Items.Add(fk.Name);
                        item.Image = Properties.Resources.relation;

                        var name = $"{Determine(fk.PkColumn.Name)} ← {Determine(fk.FkTable._Name)}.{Determine(fk.FkColumn.Name)}";
                        var sub = item.Items.Add(name);
                        sub.Image = Properties.Resources.item;

                        item.Expended = true;
                    }
                }
            }

            private string Determine(string str)
            {
                if (!string.IsNullOrEmpty(str) && str.IndexOf(" ") != -1)
                {
                    return $"[{str}]";
                }

                return str;
            }

            protected override void OnItemClick(TreeListItemEventArgs e)
            {
                _edSvc.CloseDropDown();
            }
        }
    }
}
