// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;

namespace CodeBuilder.Core.EventBus
{
    /// <summary>
    /// 事件总线处理器。
    /// </summary>
    public interface IEventBusHandler
    {
        /// <summary>
        /// 发布事件消息。
        /// </summary>
        /// <param name="name">订阅名称。</param>
        /// <param name="data"></param>
        void Publish(string name, object data = null);

        /// <summary>
        /// 订阅事件消息。
        /// </summary>
        /// <param name="name">订阅名称。</param>
        /// <param name="action">订阅者。</param>
        /// <returns></returns>
        string Subscribe(string name, Action<object> action);

        /// <summary>
        /// 注销订阅者。
        /// </summary>
        /// <param name="name">订阅名称。</param>
        /// <param name="id">事件id。</param>
        void UnSubscribe(string name, string id);
    }
}