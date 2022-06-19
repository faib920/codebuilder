using System.ComponentModel;
using CodeBuilder.Core;

public class ProfileExt1
{
    [Description("使用 FluentApi 进行实体映射。")]
    public bool Fluent { get; set; }

    [Description("模块名。")]
    [UnPersistently]
    public string Module { get; set; }

    [Description("子命名空间。用于指定默认命名空间之后的子目录，可以使用.来无限扩展，如 Customer.OrderHistory。")]
    [UnPersistently]
    public string SubNameSpace { get; set; }
}