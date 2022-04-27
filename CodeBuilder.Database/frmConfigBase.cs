// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Forms;
using Fireasy.Data;
using Fireasy.Data.Provider;
using System;
using System.Windows.Forms;

namespace CodeBuilder.Database
{
    public partial class frmConfigBase : FormBase, IConnectionConfig
    {
        public frmConfigBase()
        {
            InitializeComponent();
        }

        public string ConnectionString { get; set; }

        protected virtual IProvider Provider
        {
            get { return null; }
        }

        public DialogResult ShowDialog(IntPtr handle)
        {
            return ShowDialog();
        }

        private void frmConfigBase_Load(object sender, System.EventArgs e)
        {
            if (ConnectionString != null)
            {
                ParseConnectionStr(new ConnectionString(ConnectionString).Properties);
            }
        }

        protected virtual void ParseConnectionStr(ConnectionProperties properties)
        {
        }

        protected virtual string BuildConnectionStr()
        {
            return (string)ConnectionString;
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            ConnectionString = BuildConnectionStr();
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
    }
}
