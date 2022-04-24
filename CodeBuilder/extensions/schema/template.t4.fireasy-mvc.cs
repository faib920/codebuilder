using System;
using System.ComponentModel;
using CodeBuilder.Core.Source;

[SchemaExtension(typeof(Table))]
public class ColumnExtForTree
{
    [Description("是否树型结构。默认使用 Code 作为编码。")]
    public bool IsTree { get; set; }
}

[SchemaExtension(typeof(Column))]
public class ColumnExtForEasyUI
{
    [Category("EasyUI")]
    [Description("EasyUI控件类别。")]
    public EasyUIFieldType ControlType { get; set; }

    [Category("EasyUI")]
    [Description("是否生成域。")]
    [DefaultValue(true)]
    public bool GenerateField { get; set; }
}

public enum EasyUIFieldType
{
    TextBox,
    NumberBox,
    ComboBox,
    ComboTree,
    DateBox,
    DateTimeBox,
}