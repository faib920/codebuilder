using System.ComponentModel;
using CodeBuilder.Core;
using CodeBuilder.Core.Template;
using CodeBuilder.Core.Initializers;

public class ProfileBaseExt
{
    [Description("命名空间。")]
    public string Namespace { get; set; }
	
    [Description("项目编码。")]
    [RequiredCheck]
    public string ProjectCode { get; set; }
    
    [Description("项目名称。")]
    public string ProjectName { get; set; }
	
    [Description("公司名称。")]
    public string CompanyName { get; set; }

    [Description("作者。")]
    public string Author { get; set; }
    
    [Description("表名转类名的替换正则。")]
    public string TableRegex { get; set; }
        
    [Description("字段名转类名的替换正则。")]
    public string ColumeRegex { get; set; }

    [Description("表转换的命名方式。Pascal 首字母大写，适用于用_分隔的命名，如 customer_base 转换为 CustomerBase；Original 延续原始名称。")]
    public NamingMode TableNamingMode { get; set; }

    [Description("字段转换的命名方式。Pascal 首字母大写，适用于用_分隔的命名，如 product_id 转换为 ProductId；Original 延续原始名称。")]
    public NamingMode ColumnNamingMode { get; set; }

    [Description("语言，主要对 PropertyType 产生影响。")]
    public Language Language { get; set; }

    [Description("是否使用异步编程 async/await。")]
    [DefaultValue(true)]
    public bool Async { get; set; }
}

public class LanguageInitializer : IProfileInitializer
{
    public void Initialize(dynamic profile, TemplateDefinition template)
    {
        if (template == null)
        {
            return;
        }
        if (template.Language == "CSharp")
        {
            profile.Language = Language.CSharp;
        }
        else if (template.Language == "Java")
        {
            profile.Language = Language.Java;
        }
        else if (template.Language == "VB")
        {
            profile.Language = Language.VB;
        }
        else
        {
            profile.Language = Language.None;
        }
    }
}