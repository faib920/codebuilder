// -----------------------------------------------------------------------
// <copyright company="Fireasy"
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
        public PdmDefinition()
        {
            Diagrams = new List<PdmDiagram>();
            Packages = new List<PdmPackage>();
        }

        public List<PdmDiagram> Diagrams { get; set; }

        public List<PdmPackage> Packages { get; set; }
    }

    public class PdmPackage : PdmAbstract
    {
        public PdmPackage()
        {
            Diagrams = new List<PdmDiagram>();
            Packages = new List<PdmPackage>();
        }

        public PdmPackage Parent { get; set; }

        public List<PdmDiagram> Diagrams { get; set; }

        public List<PdmPackage> Packages { get; set; }
    }

    public class PdmDiagram : PdmAbstract
    {
        public PdmDiagram()
        {
            Tables = new List<PdmTable>();
        }

        public PdmPackage Parent { get; set; }

        public List<PdmTable> Tables { get; set; }
    }

    public class PdmTable : Table
    {
        public string Id { get; set; }

        public string Uri { get; set; }

        public PdmDiagram Parent { get; set; }
    }
}
