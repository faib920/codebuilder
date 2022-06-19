using System.ComponentModel;
using CodeBuilder.Core;

public class AbpProfileExt
{
    [Description("模块名。")]
    [RequiredCheck]
    [UnPersistently]
    public string Module { get; set; }

    [Description("子命名空间。用于指定默认命名空间之后的子目录，可以使用.来无限扩展，如 Customer.OrderHistory。")]
    [UnPersistently]
    public string SubNameSpace { get; set; }

    [Description("如果实体类使用聚合根的话，聚合根已经定义了一些主要的属性，因此生成时需要排除，使用竖线分隔，如 Id|CreateUserId|ModifyUserId|RowVersion。")]
    [DefaultValue("Id|RowVersion")]
    public string EntityExcludeProperties { get; set; }

    [Description("如果DTO类使用聚合根的话，聚合根已经定义了一些主要的属性，因此生成时需要排除，使用竖线分隔，如 Id|CreateUserId|ModifyUserId|RowVersion。")]
    [DefaultValue("Id|RowVersion")]
    public string DTOExcludeProperties { get; set; }
}