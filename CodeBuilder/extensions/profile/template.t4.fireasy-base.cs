using System.ComponentModel;
using CodeBuilder.Core;

public class ProfileExt1
{
    [Description("使用 FluentApi 进行实体映射。")]
    public bool Fluent { get; set; }

    [Description("Mvc中区域的名称。")]
    [UnPersistently]
    [RequiredCheck]
    public string MvcArea { get; set; }
}