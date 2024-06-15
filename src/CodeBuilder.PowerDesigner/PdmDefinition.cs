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

namespace CodeBuilder.PowerDesigner
{
    public class PdmAbstract
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }

    public class PdmDefinition
    {
        public List<PdmDiagram> Diagrams { get; set; } = new List<PdmDiagram>();

        public List<PdmPackage> Packages { get; set; } = new List<PdmPackage>();
    }

    public class PdmPackage : PdmAbstract
    {
        public PdmPackage Parent { get; set; }

        public List<PdmDiagram> Diagrams { get; set; } = new List<PdmDiagram>();

        public List<PdmPackage> Packages { get; set; } = new List<PdmPackage>();
    }

    public class PdmDiagram : PdmAbstract
    {
        public PdmPackage Parent { get; set; }

        public List<PdmTable> Tables { get; set; } = new List<PdmTable>();
    }

    public class PdmTable : Table
    {
        public string Id { get; set; }

        public string Uri { get; set; }

        public PdmDiagram Parent { get; set; }
    }
}
