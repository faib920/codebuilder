// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using System;

namespace CodeBuilder
{
    public partial class frmProperty : DockForm
    {
        private readonly DevHosting _hosting;

        public frmProperty(DevHosting hosting)
        {
            InitializeComponent();
            Icon = Properties.Resources.property;
            _hosting = hosting;
            _hosting.ViewInPropGridAct = obj => propertyGrid1.SelectedObject = obj;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _hosting.ViewInPropGridAct = null;
        }
    }
}
