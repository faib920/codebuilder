using System;
using System.Data;
using System.ComponentModel;
using CodeBuilder.Core.Source;
using CodeBuilder.Core.Template;
using CodeBuilder.Core.Initializers;

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

// 用于指定easyui的控件类型
[SchemaInitializer(typeof(Column))]
public class EasyUITypeInitializer : ISchemaInitializer
{
    public void Initialize(dynamic profile, dynamic schema)
    {
        var column = schema as Column;
        switch (column.DbType)
        {
            case DbType.Int16:
            case DbType.Int32:
            case DbType.Int64:
            case DbType.Decimal:
            case DbType.Double:
            case DbType.Single:
                schema.ControlType = EasyUIFieldType.NumberBox;
                break;
            case DbType.Date:
            case DbType.DateTime:
            case DbType.DateTime2:
            case DbType.DateTimeOffset:
                schema.ControlType = EasyUIFieldType.DateBox;
                break;
        }
    }
}

//当没有指定 Module 时，将文件路径中的Areas移走
public class AreaOutputProcessor : IPartitionOutputParser
{
    public void Parse(OutputParseContext context)
    {
        if (string.IsNullOrEmpty(context.Profile.Module) && context.Result.IndexOf("Areas\\") != -1)
        {
            context.Result = context.Result.Replace("Areas\\", string.Empty);
        }
    }
}