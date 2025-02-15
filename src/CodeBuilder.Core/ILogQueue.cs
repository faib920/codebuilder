// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace CodeBuilder.Core
{
    public interface ILogQueue
    {
        void Push(int type, string msg);
    }

    public interface ILogQueueSupported
    {
        ILogQueue GetQueue();
    }
}
