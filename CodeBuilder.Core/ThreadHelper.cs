// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Threading;

namespace CodeBuilder.Core
{
    public static class ThreadHelper
    {
        public static void Start(Action action)
        {
            var thread = new Thread(new ThreadStart(action));
            thread.IsBackground = true;
            thread.Start();
        }
    }
}
