using System;
using System.ComponentModel;
using CodeBuilder.Core.Source;

[SchemaExtension(typeof(Table))]
public class ColumnExtForTree
{
    [Description("是否树型结构。默认使用 Code 作为编码。")]
    public bool IsTree { get; set; }
}