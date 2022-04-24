// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using Fireasy.Common;
using System;

namespace CodeBuilder.T4
{
    public class SingleAppDomainScope : Scope<SingleAppDomainScope>
    {
        private AppDomain _domain;

        public AppDomain AppDomain
        {
            get
            {
                return _domain ?? (_domain = AppDomain.CreateDomain("Generation App Domain"));
            }
        }

        protected override bool Dispose(bool disposing)
        {
            if (_domain != null)
            {
                AppDomain.Unload(_domain);
            }

            return base.Dispose(disposing);
        }
    }
}
