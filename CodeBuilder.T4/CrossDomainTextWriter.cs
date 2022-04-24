// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.IO;
using System.Reflection;

namespace CodeBuilder.T4
{
    [Serializable]
    public sealed class CrossDomainTextWriter : MarshalByRefObject
    {
        private CrossDomainTextWriter _remoteTracer;

        public CrossDomainTextWriter()
        {
        }

        public CrossDomainTextWriter(AppDomain domain, TextWriter writer)
        {
            _remoteTracer = domain.CreateInstanceFrom(Assembly.GetExecutingAssembly().Location, typeof(CrossDomainTextWriter).FullName).Unwrap() as CrossDomainTextWriter;
            if (_remoteTracer != null)
            {
                _remoteTracer.StartListening(this, writer);
            }
        }

        public void StartListening(CrossDomainTextWriter farTracer, TextWriter writer)
        {
            _remoteTracer = farTracer;
        }
    }
}
