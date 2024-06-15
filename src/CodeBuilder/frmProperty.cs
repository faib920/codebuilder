// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Forms;
using System;
using System.Windows.Forms;

namespace CodeBuilder
{
    public partial class frmProperty : DockFormBase
    {
        private readonly DevHosting _hosting;

        public frmProperty(DevHosting hosting)
        {
            InitializeComponent();
            Icon = Properties.Resources.property;
            _hosting = hosting;
            _hosting.ViewInPropGridAct = obj =>
            {
                this.Invoke(new Action(() =>
                {
                    propertyGrid1.SelectedObject = obj;
                }));
            };
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _hosting.ViewInPropGridAct = null;
        }
        public void SelectProperty(string propertyName)
        {
            if (propertyGrid1.SelectedObject == null || propertyGrid1.SelectedGridItem == null)
            {
                return;
            }

            propertyGrid1.Focus();

            foreach (GridItem gridItem in propertyGrid1.SelectedGridItem.Parent.GridItems)
            {
                if (gridItem.Label == propertyName)
                {
                    propertyGrid1.SelectedGridItem = gridItem;
                    break;
                }
            }
        }
    }
}
