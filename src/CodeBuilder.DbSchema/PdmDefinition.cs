// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Source;
using System.Collections.Generic;

namespace CodeBuilder.DbSchema
{
    public class PdmAbstract
    {
        public string Name { get; set; }
    }

    public class PdmDefinition
    {
        public List<PdmSchema> Schemas { get; set; } = new List<PdmSchema>();
    }

    public class PdmSchema : PdmAbstract
    {
        public List<PdmTable> Tables { get; set; } = new List<PdmTable>();
    }

    public class PdmTable : Table
    {
        public string Uri { get; set; }
    }
}
