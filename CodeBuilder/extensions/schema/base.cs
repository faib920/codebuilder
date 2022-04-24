using System.ComponentModel;
using CodeBuilder.Core;
using CodeBuilder.Core.Source;
using CodeBuilder.Core.Initializers;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

// 用于格式化类名
[SchemaInitializer(typeof(Table))]
public class ClassNameInitializer : ISchemaInitializer
{
    public IDevHosting Hosting { get; set; }

    public void Initialize(dynamic profile, dynamic schema)
    {
        var table = schema as Table;
        var tableName = table.Name;

        // 正则替换前缀
        if (!string.IsNullOrEmpty(profile.TableRegex))
        {
            var regx = new Regex(profile.TableRegex);
            if (regx.IsMatch(tableName))
            {
                tableName = regx.Replace(tableName, string.Empty);
            }
        }

        // Pascal 命名
        if (profile.TableNamingMode == NamingMode.Pascal)
        {
            table.ClassName = SchemaHelper.PascalFormat(tableName);
        }
        else
        {
            table.ClassName = tableName;
        }

        Hosting.ConsoleInfo(tableName);
        Hosting.ConsoleError(table.ClassName);
    }
}

// 用于格式化属性名
[SchemaInitializer(typeof(Column))]
public class PropertyNameInitializer : ISchemaInitializer
{
    public void Initialize(dynamic profile, dynamic schema)
    {
        var column = schema as Column;
        var columnName = column.Name;

        // 正则替换前缀
        if (!string.IsNullOrEmpty(profile.ColumeRegex))
        {
            var regx = new Regex(profile.ColumeRegex);
            if (regx.IsMatch(columnName))
            {
                columnName = regx.Replace(columnName, string.Empty);
            }
        }

        // Pascal 命名
        if (profile.ColumnNamingMode == NamingMode.Pascal)
        {
            column.PropertyName = SchemaHelper.PascalFormat(columnName);
        }
        else
        {
            column.PropertyName = columnName;
        }
    }
}

// 用于转换属性类型
[SchemaInitializer(typeof(Column))]
public class PropertyTypeInitializer : ISchemaInitializer
{
    public void Initialize(dynamic profile, dynamic schema)
    {
        var column = schema as Column;
        if (profile.Language == Language.CSharp)
        {
            column.PropertyType = SchemaHelper.GetCSharpType(column);
        }
        else if (profile.Language == Language.Java)
        {
            column.PropertyType = SchemaHelper.GetJavaType(column);
        }
        else if (profile.Language == Language.VB)
        {
            column.PropertyType = SchemaHelper.GetVBType(column);
        }
        else
        {
            column.PropertyType = "??";
        }
    }
}

internal class SchemaHelper
{
    // Pascal 命名转换
    public static string PascalFormat(string name)
    {
        var sb = new StringBuilder();
        foreach (var arr in name.Split('_', ' '))
        {
            if (arr.Length > 0)
            {
                sb.Append(arr.Substring(0, 1).ToUpper());
                sb.Append(arr.Substring(1).ToLower());
            }
        }

        return sb.ToString();
    }

    // 获取 CSharp 的类型名称
    public static string GetCSharpType(Column column)
    {
        var propertyType = column.PropertyType;
        if (column.DbType == null)
        {
            return propertyType;
        }

        switch ((DbType)column.DbType)
        {
            case DbType.String:
            case DbType.StringFixedLength:
            case DbType.AnsiString:
            case DbType.AnsiStringFixedLength:
                propertyType = "string";
                break;
            case DbType.Int16:
                propertyType = "short";
                break;
            case DbType.UInt16:
                propertyType = "ushort";
                break;
            case DbType.Int32:
                propertyType = "int";
                break;
            case DbType.UInt32:
                propertyType = "uint";
                break;
            case DbType.Int64:
                propertyType = "long";
                break;
            case DbType.UInt64:
                propertyType = "ulong";
                break;
            case DbType.Decimal:
                propertyType = "decimal";
                break;
            case DbType.Single:
                propertyType = "float";
                break;
            case DbType.Double:
                propertyType = "double";
                break;
            case DbType.Boolean:
                propertyType = "bool";
                break;
            case DbType.Byte:
            case DbType.SByte:
                propertyType = column.Name.StartsWith("Is") ? "bool" : "int";
                break;
            case DbType.Date:
            case DbType.DateTime:
            case DbType.DateTime2:
            case DbType.DateTimeOffset:
                propertyType = "DateTime";
                break;
            case DbType.Guid:
                propertyType = "Guid";
                break;
            case DbType.Binary:
                propertyType = "byte[]";
                break;
        }

        if (column.IsNullable && propertyType != "string" && column.DbType != DbType.Binary)
        {
            propertyType += "?";
        }

        return propertyType;
    }

    // 获取 Java 的类型名称
    public static string GetJavaType(Column column)
    {
        var propertyType = column.PropertyType;
        if (column.DbType == null)
        {
            return propertyType;
        }

        switch ((DbType)column.DbType)
        {
            case DbType.String:
            case DbType.StringFixedLength:
            case DbType.AnsiString:
            case DbType.AnsiStringFixedLength:
                propertyType = "String";
                break;
            case DbType.Int16:
            case DbType.UInt16:
                propertyType = "short";
                break;
            case DbType.Int32:
            case DbType.UInt32:
                propertyType = "int";
                break;
            case DbType.Int64:
            case DbType.UInt64:
                propertyType = "long";
                break;
            case DbType.Single:
            case DbType.Decimal:
                propertyType = "float";
                break;
            case DbType.Double:
                propertyType = "double";
                break;
            case DbType.Boolean:
                propertyType = "boolean";
                break;
            case DbType.Byte:
            case DbType.SByte:
                propertyType = "byte";
                break;
            case DbType.Date:
            case DbType.DateTime:
            case DbType.DateTime2:
            case DbType.DateTimeOffset:
                propertyType = "Date";
                break;
            case DbType.Binary:
                propertyType = "byte[]";
                break;
        }

        return propertyType;
    }

    // 获取 CSharp 的类型名称
    public static string GetVBType(Column column)
    {
        var propertyType = column.PropertyType;
        if (column.DbType == null)
        {
            return propertyType;
        }

        switch ((DbType)column.DbType)
        {
            case DbType.String:
            case DbType.StringFixedLength:
            case DbType.AnsiString:
            case DbType.AnsiStringFixedLength:
                propertyType = "String";
                break;
            case DbType.Int16:
                propertyType = "Short";
                break;
            case DbType.UInt16:
                propertyType = "UInt16";
                break;
            case DbType.Int32:
                propertyType = "Integer";
                break;
            case DbType.UInt32:
                propertyType = "UInt32";
                break;
            case DbType.Int64:
                propertyType = "Long";
                break;
            case DbType.UInt64:
                propertyType = "UInt64";
                break;
            case DbType.Decimal:
                propertyType = "Decimal";
                break;
            case DbType.Single:
                propertyType = "Single";
                break;
            case DbType.Double:
                propertyType = "Double";
                break;
            case DbType.Boolean:
                propertyType = "Boolean";
                break;
            case DbType.Byte:
            case DbType.SByte:
                propertyType = column.Name.StartsWith("Is") ? "Boolean" : "Integer";
                break;
            case DbType.Date:
            case DbType.DateTime:
            case DbType.DateTime2:
            case DbType.DateTimeOffset:
                propertyType = "DateTime";
                break;
            case DbType.Guid:
                propertyType = "Guid";
                break;
            case DbType.Binary:
                propertyType = "Byte()";
                break;
        }

        if (column.IsNullable && propertyType != "string" && column.DbType != DbType.Binary)
        {
            propertyType = "Nullable<" + propertyType + ">";
        }

        return propertyType;
    }
}