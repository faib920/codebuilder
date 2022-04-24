// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using Fireasy.Data;
using Fireasy.Data.Provider;
using System.Windows.Forms;

namespace CodeBuilder.Database
{
    public partial class frmSQLiteConfig : frmConfigBase
    {
        public frmSQLiteConfig()
        {
            InitializeComponent();
        }

        protected override IProvider Provider
        {
            get { return SQLiteProvider.Instance; }
        }

        protected override void ParseConnectionStr(ConnectionProperties properties)
        {
            if (!string.IsNullOrWhiteSpace(ConnectionString))
            {
                txtFile.Text = properties.TryGetValue("data source");
            }
        }

        protected override string BuildConnectionStr()
        {
            return string.Format("data source={0};pooling=True", txtFile.Text);
        }

        private void btnBrowse_Click(object sender, System.EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "SQLite DB|*.db3|所有文件|*.*";
                dialog.FileName = txtFile.Text;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtFile.Text = dialog.FileName;
                }
            }
        }
    }
}
