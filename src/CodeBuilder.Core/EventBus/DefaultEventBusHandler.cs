// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using Fireasy.Common.DependencyInjection;
using System;
using System.Collections.Generic;

namespace CodeBuilder.Core.EventBus
{
    public class DefaultEventBusHandler : IEventBusHandler, ISingletonService
    {
        private Dictionary<string, Dictionary<string, Action<object>>> _subscribers = new Dictionary<string, Dictionary<string, Action<object>>>();

        public void Publish(string name, object data = null)
        {
            if (_subscribers.TryGetValue(name, out var actions))
            {
                foreach (var kvp in actions)
                {
                    kvp.Value(data);
                }
            }
        }

        public string Subscribe(string name, Action<object> action)
        {
            if (!_subscribers.TryGetValue(name, out var actions))
            {
                actions = new Dictionary<string, Action<object>>();
                _subscribers.Add(name, actions);
            }

            var id = Guid.NewGuid().ToString();
            actions.Add(id, action);

            return id;
        }

        public void UnSubscribe(string name, string id)
        {
            if (_subscribers.TryGetValue(name, out var actions))
            {
                if (actions.ContainsKey(id))
                {
                    actions.Remove(id);
                }
            }
        }
    }
}
