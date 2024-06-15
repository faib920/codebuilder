// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Source;
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace CodeBuilder.Core.Designer
{
    public class ForeignKeyEditor : UITypeEditor
    {
        private ForeignKeyEditorForm _form;

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider != null)
            {
                var edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (edSvc == null)
                {
                    return value;
                }

                var column = (Column)context.Instance;
                _form = new ForeignKeyEditorForm(column);
                if (_form.ShowDialog() == DialogResult.OK)
                {
                    value = _form.Value;
                }
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
                return false;
            }
        }
    }
}
