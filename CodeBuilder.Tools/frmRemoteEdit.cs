// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CodeBuilder.Tools
{
    public partial class frmRemoteEdit : Form
    {
        public frmRemoteEdit()
        {
            InitializeComponent();
            
        }

        public List<string> Groups { get; set; }

        public Remoter.Connection Connection { get; set; }

        private void frmRemoteEdit_Load(object sender, EventArgs e)
        {
            cboGroup.DataSource = Groups;
            if (Connection != null)
            {
                txtName.Text = Connection.name;
                txtHost.Text = Connection.host;
                cboGroup.Text = Connection.group;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (Connection == null)
            {
                Connection = new Remoter.Connection();
            }

            Connection.name = txtName.Text;
            Connection.host = txtHost.Text;
            Connection.group = cboGroup.Text;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
