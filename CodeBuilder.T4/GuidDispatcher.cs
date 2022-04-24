// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace CodeBuilder.T4
{
    [Serializable]
    public class GuidDispatcher : MarshalByRefObject
    {
        private readonly Dictionary<string, Guid> _dict = new Dictionary<string, Guid>();

        public Guid this[string key]
        {
            get
            {
                if (!_dict.TryGetValue(key, out Guid guid))
                {
                    guid = Guid.NewGuid();
                    _dict.Add(key, guid);
                }

                return guid;
            }
        }
    }
}
