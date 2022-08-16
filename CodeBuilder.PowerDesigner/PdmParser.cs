// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Source;
using CodeBuilder.Core.Variable;
using Fireasy.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace CodeBuilder.PowerDesigner
{
    public class PdmParser
    {
        private readonly XmlDocument _doc;
        private readonly XmlNamespaceManager _nms;
        private readonly string _pdmFileName;
        private readonly string _databaseType;
        private readonly List<string> _existsCodes = new List<string>();

        public PdmParser(string pdmFileName)
        {
            _pdmFileName = pdmFileName;
            _doc = new XmlDocument();
            _doc.Load(pdmFileName);
            _nms = new XmlNamespaceManager(_doc.NameTable);
            _nms.AddNamespace("o", "object");
            _nms.AddNamespace("c", "collection");
            _nms.AddNamespace("a", "attribute");

            var dbNode = _doc.SelectSingleNode("//Model/o:RootObject/c:Children/o:Model/c:DBMS/o:Shortcut/a:Code", _nms);
            if (dbNode == null)
            {
                dbNode = _doc.SelectSingleNode("//Model/o:RootObject/c:Children/o:Model/c:DBMS/o:DBMS/a:Code", _nms);
            }

            _databaseType = dbNode.InnerText;
        }

        public static PdmDefinition Parse(string pdmFileName)
        {
            return new PdmParser(pdmFileName).ParseDefinition();
        }

        public PdmDefinition ParseDefinition()
        {
            var definition = new PdmDefinition();

            var packageRootNode = _doc.SelectSingleNode("//Model/o:RootObject/c:Children/o:Model/c:Packages", _nms);
            var diagramRootNode = _doc.SelectSingleNode("//Model/o:RootObject/c:Children/o:Model/c:PhysicalDiagrams", _nms);

            LoadPackages(definition.Packages, packageRootNode, null);
            LoadDiagrams(definition.Diagrams, diagramRootNode, null);

            return definition;
        }

        public Table ParseTable(PdmTable table)
        {
            var node = _doc.SelectSingleNode(table.Uri, _nms);
            var ndColumns = node.SelectNodes("c:Columns/o:Column", _nms);
            var newtable = SchemaExtensionManager.Build<Table>();

            var pkRootNode = node.SelectSingleNode("c:PrimaryKey/o:Key", _nms);
            var pkRefId = pkRootNode == null ? string.Empty : pkRootNode.Attributes["Ref"].InnerText;

            newtable.Name = table.Name;
            newtable.Description = table.Description;

            foreach (XmlNode child in ndColumns)
            {
                var id = child.Attributes["Id"].InnerText;
                var nameNode = child.SelectSingleNode("a:Name", _nms);
                var codeNode = child.SelectSingleNode("a:Code", _nms);
                var descNode = child.SelectSingleNode("a:Comment", _nms);
                var dtNode = child.SelectSingleNode("a:DataType", _nms);
                var lngNode = child.SelectSingleNode("a:Length", _nms);
                var mandNode = child.SelectSingleNode("a:Column.Mandatory", _nms);
                var idenNode = child.SelectSingleNode("a:Identity", _nms);
                var defaultNode = child.SelectSingleNode("a:DefaultValue", _nms);
                var column = SchemaExtensionManager.Build<Column>(newtable);
                column.Name = codeNode.InnerText;

                if (descNode != null && !string.IsNullOrEmpty(descNode.InnerText) &&
                    nameNode.InnerText != descNode.InnerText)
                {
                    column.Description = descNode.InnerText;
                }
                else
                {
                    column.Description = nameNode.InnerText;
                }

                if (dtNode != null)
                {
                    ParseDbTypeAndPrecision(dtNode.InnerText, column);
                    column.DbType = DbTypeManager.GetDbType(_databaseType, column.DataType);
                    column.ColumnType = dtNode.InnerText;
                }

                if (lngNode != null)
                {
                    column.Length = lngNode.InnerText.To<long>();
                }

                if (mandNode == null)
                {
                    column.IsNullable = true;
                }

                if (idenNode != null)
                {
                    column.AutoIncrement = idenNode.InnerText == "1";
                }

                if (defaultNode != null)
                {
                    column.DefaultValue = defaultNode.InnerText;
                }

                if (!string.IsNullOrEmpty(pkRefId))
                {
                    var pkNode = node.SelectSingleNode("c:Keys/o:Key[@Id='" + pkRefId + "']/c:Key.Columns/o:Column[@Ref='" + id + "']", _nms);
                    if (pkNode != null)
                    {
                        column.IsPrimaryKey = true;
                    }
                }

                newtable.Columns.Add(column);
            }

            return newtable;
        }

        public void ParseReferences(List<Table> source, List<Table> result)
        {
            foreach (var uri in source.GroupBy(s => ((PdmTable)s).Uri).ToDictionary(s => s.Key))
            {
                var node = _doc.SelectSingleNode(uri.Key, _nms);

                foreach (PdmTable t in uri.Value)
                {
                    foreach (XmlNode refChild in node.SelectNodes("../../c:References/o:Reference/c:ChildTable/o:Table[@Ref='" + t.Id + "']", _nms))
                    {
                        var ndRefer = refChild.ParentNode.ParentNode;
                        var pNode = ndRefer.SelectSingleNode("c:ParentTable/o:Table", _nms);
                        var cNode = ndRefer.SelectSingleNode("c:ChildTable/o:Table", _nms);

                        var isShortcut = false;
                        if (pNode == null)
                        {
                            pNode = ndRefer.SelectSingleNode("c:ParentTable/o:Shortcut", _nms);
                            var sNode = node.ParentNode.SelectSingleNode("o:Shortcut[@Id='" + pNode.Attributes["Ref"].InnerText + "']", _nms);
                            pNode = FindShortcutNode(sNode);
                            isShortcut = true;
                        }

                        if (pNode == null || cNode == null)
                        {
                            continue;
                        }

                        var parentTableId = pNode.Attributes[isShortcut ? "Id" : "Ref"].InnerText;
                        var childTableId = cNode.Attributes["Ref"].InnerText;

                        var parentTable = source.FirstOrDefault(s => ((PdmTable)s).Id == parentTableId) as PdmTable;
                        var childTable = source.FirstOrDefault(s => ((PdmTable)s).Id == childTableId) as PdmTable;
                        if (parentTable == null || childTable == null)
                        {
                            continue;
                        }

                        var ndObject1 = ndRefer.SelectSingleNode("c:Joins/o:ReferenceJoin/c:Object1/o:Column", _nms);
                        var ndObject2 = ndRefer.SelectSingleNode("c:Joins/o:ReferenceJoin/c:Object2/o:Column", _nms);
                        if (ndObject1 == null || ndObject2 == null)
                        {
                            continue;
                        }

                        var parentColumnId = ndObject1.Attributes["Ref"].InnerText;
                        var childColumnId = ndObject2.Attributes["Ref"].InnerText;

                        Table pkT, fkT;
                        Column pkC, fkC;
                        FindColumn(parentTable, result, parentColumnId, out pkT, out pkC);
                        FindColumn(childTable, result, childColumnId, out fkT, out fkC);
                        if (pkT != null && fkT != null && pkC != null && fkC != null)
                        {
                            var codeNode = ndRefer.SelectSingleNode("a:Code", _nms);
                            var refer = new Reference(pkT, pkC, fkT, fkC);
                            refer.Name = codeNode.InnerText;
                            fkC.BindForeignKey(refer);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 通过快捷方式查找目标节点。
        /// </summary>
        /// <param name="sNode"></param>
        /// <returns></returns>
        private XmlNode FindShortcutNode(XmlNode sNode)
        {
            var targetId = sNode.SelectSingleNode("a:TargetID", _nms).InnerText;
            var targetPath = sNode.SelectSingleNode("a:TargetPackagePath", _nms).InnerText;
            var sPath = targetPath.Split(new string[] { "::" }, StringSplitOptions.RemoveEmptyEntries);

            var nodes = _doc.SelectNodes("//Model/o:RootObject/c:Children/o:Model/c:Packages/o:Package", _nms);
            XmlNode fNode = null;
            for (var i = 1; i < sPath.Length; i++)
            {
                foreach (XmlNode child in nodes)
                {
                    if (child.SelectSingleNode("a:Name", _nms).AssertNotNull(s => s.InnerText) == sPath[i])
                    {
                        if (i == sPath.Length - 1)
                        {
                            fNode = child;
                            break;
                        }
                        else
                        {
                            nodes = child.SelectNodes("c:Packages/o:Package", _nms);
                        }
                    }

                    if (fNode != null || nodes == null)
                    {
                        break;
                    }
                }
            }

            if (fNode == null)
            {
                return null;
            }

            foreach (XmlNode child in fNode.SelectNodes("c:Tables/o:Table", _nms))
            {
                var tNode = child.SelectSingleNode("a:ObjectID", _nms).InnerText;
                if (tNode == targetId)
                {
                    return child;
                }
            }

            return null;
        }

        private void FindColumn(PdmTable table, List<Table> result, string columnId, out Table iTable, out Column iColumn)
        {
            iTable = result.FirstOrDefault(s => s.Name == table.Name);
            if (iTable == null)
            {
                iColumn = null;
                return;
            }

            var node = _doc.SelectSingleNode(table.Uri, _nms);
            var ndColumn = node.SelectSingleNode("c:Columns/o:Column[@Id='" + columnId + "']", _nms);
            var codeNode = ndColumn.SelectSingleNode("a:Code", _nms);
            iColumn = iTable.Columns.FirstOrDefault(s => s.Name == codeNode.InnerText);
        }

        private void LoadPackages(List<PdmPackage> packages, XmlNode packageRootNode, PdmPackage parent)
        {
            if (packageRootNode == null)
            {
                return;
            }

            foreach (XmlNode child in packageRootNode.SelectNodes("o:Package", _nms))
            {
                var nameNode = child.SelectSingleNode("a:Name", _nms);
                var packageNode = child.SelectSingleNode("c:Packages", _nms);
                var diagramNode = child.SelectSingleNode("c:PhysicalDiagrams", _nms);

                var package = new PdmPackage();
                package.Id = child.Attributes["Id"].InnerText;
                package.Name = nameNode.InnerText;
                package.Parent = parent;
                packages.Add(package);

                LoadPackages(package.Packages, packageNode, package);
                LoadDiagrams(package.Diagrams, diagramNode, package);
            }
        }

        private void LoadDiagrams(List<PdmDiagram> diagrams, XmlNode diagramRootNode, PdmPackage parent)
        {
            if (diagramRootNode == null)
            {
                return;
            }

            foreach (XmlNode child in diagramRootNode.SelectNodes("o:PhysicalDiagram", _nms))
            {
                var nameNode = child.SelectSingleNode("a:Name", _nms);
                var id = child.Attributes["Id"].InnerText;
                var symbolRootNode = child.SelectSingleNode("c:Symbols", _nms);

                var diagram = new PdmDiagram();
                diagram.Id = child.Attributes["Id"].InnerText;
                diagram.Name = nameNode.InnerText;
                diagram.Parent = parent;
                LoadTables(diagram, symbolRootNode);
                diagrams.Add(diagram);
            }
        }

        private void LoadTables(PdmDiagram diagram, XmlNode symbolRootNode)
        {
            if (symbolRootNode == null)
            {
                return;
            }

            var url = GetTableReferencePath(diagram);
            var tableRootNode = _doc.SelectSingleNode(url, _nms);

            foreach (XmlNode child in symbolRootNode.SelectNodes("o:TableSymbol", _nms))
            {
                if (child.SelectSingleNode("c:ClonePredecessor", _nms) != null)
                {
                    continue;
                }

                var refNode = child.SelectSingleNode("c:Object/o:Table", _nms);
                if (refNode != null)
                {
                    var id = refNode.Attributes["Ref"].InnerText;
                    if (_existsCodes.Contains(id))
                    {
                        continue;
                    }

                    _existsCodes.Add(id);

                    var tNode = tableRootNode.SelectSingleNode("o:Table[@Id='" + id + "']", _nms);

                    if (tNode != null)
                    {
                        var nameNode = tNode.SelectSingleNode("a:Name", _nms);
                        var codeNode = tNode.SelectSingleNode("a:Code", _nms);
                        var descNode = tNode.SelectSingleNode("a:Comment", _nms);

                        var table = new PdmTable();
                        table.Id = id;
                        table.Name = codeNode.InnerText;

                        if (descNode != null && !string.IsNullOrEmpty(descNode.InnerText) &&
                            nameNode.InnerText != descNode.InnerText)
                        {
                            table.Description = descNode.InnerText;
                        }
                        else
                        {
                            table.Description = nameNode.InnerText;
                        }

                        table.Uri = string.Format("{0}/o:Table[@Id='{1}']", url, id);
                        diagram.Tables.Add(table);
                    }
                }
            }
        }

        private string GetTableReferencePath(PdmDiagram diagram)
        {
            var sb = new StringBuilder();
            var parent = diagram.Parent;

            while (parent != null)
            {
                sb.Insert(0, "/c:Packages/o:Package[@Id='" + parent.Id + "']");
                parent = parent.Parent;
            }

            return string.Format("//Model/o:RootObject/c:Children/o:Model{0}/c:Tables", sb);
        }

        private void ParseDbTypeAndPrecision(string dbType, Column column)
        {
            var matches = Regex.Matches(dbType, @"(\w+)\((\d+)\)");
            if (matches.Count > 0)
            {
                column.DataType = matches[0].Groups[1].Value;
                column.Length = matches[0].Groups[1].Value.To<int>();
            }
            else
            {
                matches = Regex.Matches(dbType, @"(\w+)\((\d+),(\d+)\)");
                if (matches.Count > 0)
                {
                    column.DataType = matches[0].Groups[1].Value;
                    column.Precision = matches[0].Groups[1].Value.To<int>();
                    column.Scale = matches[0].Groups[2].Value.To<int>();
                }
                else
                {
                    column.DataType = dbType;
                }
            }
        }
    }
}
