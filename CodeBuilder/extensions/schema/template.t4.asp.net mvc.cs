using System;
using System.ComponentModel;
using CodeBuilder.Core.Source;

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

    [Description("是否树型结构。默认使用 Code 作为编码。")]
    public bool IsTree { get; set; }
}

[SchemaExtension(typeof(Table))]
public class TableExtForEasyUI
{
    [Category("EasyUI")]
    [Description("分列显示。")]
    [DefaultValue(1)]
    public int Clos { get; set; }
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