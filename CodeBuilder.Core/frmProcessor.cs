// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeBuilder.Core
{
    public partial class frmProcessor : Form
    {
        private Task task;
        private CancellationTokenSource cancelToken;
        private Action taskAct;
        private Action calcelAct;

        public frmProcessor(Action taskAct, Action calcelAct)
        {
            InitializeComponent();
            this.taskAct = taskAct;
            this.calcelAct = calcelAct;
            cancelToken = new CancellationTokenSource();
        }

        private void frmProcessor_Load(object sender, EventArgs e)
        {
            task = Task.Factory.StartNew(() =>
                {
                    using (var scope = new CancellationTokenScope(cancelToken.Token))
                    {
                        taskAct();
                    }
                }, cancelToken.Token)
                .ContinueWith(t =>
                    {
                        Invoke(new Action(() => Close()));
                    });
        }

        private void frmProcessor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                cancelToken.Cancel();
                if (calcelAct != null)
                {
                    calcelAct();
                }

                Close();
            }
        }
    }
}
