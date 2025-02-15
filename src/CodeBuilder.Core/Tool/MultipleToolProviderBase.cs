// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CodeBuilder.Core.Tool
{
    public abstract class MultipleToolProviderBase : IMultipleToolProvider
    {
        protected IDevHosting _hosting;

        public virtual IEnumerable<IToolMenu> SubItems { get; } = new List<IToolMenu>();

        public abstract string Name { get; }

        public abstract Form Execute(string name, params object[] arguments);

        public Form Execute(params object[] arguments)
        {
            throw new NotImplementedException();
        }

        public void Initialize(IDevHosting hosting)
        {
            _hosting = hosting;

            OnInitialize(hosting);
        }

        protected virtual void OnInitialize(IDevHosting hosting)
        {
        }
    }
}
