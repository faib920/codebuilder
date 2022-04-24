// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System.Collections.Generic;

namespace CodeBuilder.Core
{
    public interface IObject
    {
        string Name { get; set; }

        string Description { get; set; }

        List<IField> Fields { get; }
    }
}
