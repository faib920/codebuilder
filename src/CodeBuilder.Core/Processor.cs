// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using Fireasy.Common.Diagnostics;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeBuilder.Core
{
    public class Processor
    {
        private static frmProcessor _processForm = null;

        /// <summary>
        /// 是否处理中
        /// </summary>
        public static bool IsProcessing => _processForm != null;

        /// <summary>
        /// 尝试恢复进度提示框。
        /// </summary>
        /// <returns></returns>
        public static bool TryRestore()
        {
            if (_processForm != null)
            {
                _processForm.Opacity = 1;

                return true;
            }

            return false;
        }

        /// <summary>
        /// 在后台启动一个任务，并显示一个进度提示框。
        /// </summary>
        /// <param name="owner">进度提示框的所有者。</param>
        /// <param name="taskFunc">要启动的任务。</param>
        /// <param name="cancelAct">取消后进行的动作。</param>
        /// <returns></returns>
        public static TimeSpan Run(IWin32Window owner, Func<CancellationToken, Task> taskFunc, Action cancelAct = null, bool onBackground = false)
        {
            return TimeWatcher.Watch(() =>
                {
                    _processForm = new frmProcessor(taskFunc, cancelAct, onBackground);
                    _processForm.ShowDialog(owner);
                    _processForm = null;
                    
                    if (owner is INotifyWindow notifyWnd)
                    {
                        notifyWnd.RestoreWindow();
                    }
                });
        }
    }
}
