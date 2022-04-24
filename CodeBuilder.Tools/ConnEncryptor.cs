// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using System.ComponentModel.Composition;

namespace CodeBuilder.Tools
{
    [Export(typeof(IToolProvider))]
    public class ConnEncryptor : IToolProvider
    {
        private IDevHosting _hosting;

        public string Name
        {
            get { return "连接串加解密"; }
        }

        public void Initialize(IDevHosting hosting)
        {
            _hosting = hosting;
        }

        public void Execute()
        {
            new frmConnEncrypt().ShowDialog(_hosting.DockContainer);
        }
    }
}
