// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Forms;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeBuilder.Core
{
    public partial class frmProcessor : FormBase
    {
        private CancellationTokenSource _cancelToken;
        private Func<CancellationToken, Task> _taskFunc;
        private Action _calcelAct;
        private Task _task;
        private Size _size;

        public frmProcessor(Func<CancellationToken, Task> taskFunc, Action calcelAct, bool onBackground)
        {
            InitializeComponent();
            _taskFunc = taskFunc;
            _calcelAct = calcelAct;
            _cancelToken = new CancellationTokenSource();

            panel1.Visible = onBackground;
        }

        private void frmProcessor_Load(object sender, EventArgs e)
        {
            _task = Task.Run(() => _taskFunc(_cancelToken.Token)).ContinueWith(t =>
            {
                if (!_cancelToken.IsCancellationRequested)
                {
                    Invoke(new Action(() => Close()));
                }
            });
        }

        private void frmProcessor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Cancel();
            }
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            Opacity = 0;

            if (Owner is INotifyWindow notifyWnd)
            {
                notifyWnd.ShowOnNotifyIcon();
            }
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            Cancel();
        }

        private void Cancel()
        {
            _cancelToken.Cancel();
            if (_calcelAct != null)
            {
                _calcelAct();
            }

            Close();
        }
    }
}
