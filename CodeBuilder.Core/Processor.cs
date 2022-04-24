// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using Fireasy.Common;
using System;
using System.Windows.Forms;

namespace CodeBuilder.Core
{
    public class Processor
    {
        /// <summary>
        /// 在后台启动一个任务，并显示一个进度提示框。
        /// </summary>
        /// <param name="owner">进度提示框的所有者。</param>
        /// <param name="taskAct">要启动的任务。</param>
        /// <param name="cancelAct">取消后进行的动作。</param>
        /// <returns></returns>
        public static TimeSpan Run(IWin32Window owner, Action taskAct, Action cancelAct = null)
        {
            return TimeWatcher.Watch(() =>
                {
                    new frmProcessor(taskAct, cancelAct).ShowDialog(owner);
                });
        }

        /// <summary>
        /// 获取当前线程内的请求是否已取消。
        /// </summary>
        /// <returns></returns>
        public static bool IsCancellationRequested()
        {
            return CancellationTokenScope.Current != null &&
                CancellationTokenScope.Current.IsCancellationRequested;
        }
    }
}
