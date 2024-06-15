## 代码生成器 CodeBuilder

  CodeBuilder 是一款功能强大的代码生成工具。它能将你所设计的数据库结构转换成你所想要的任何文本形式的文件，如 Java、C#、VB、Python、Javascript 等代码文件，以及 SQL 脚本、数据库设计文档等。通过开发插件，你甚至可将其转换成 Word、PDF 等二进制文件。

  CodeBuilder 基于插件式、开放式的思想，你通过实现其定义的接口，就可轻松地将你所开发的插件集成到 CodeBuilder 中来。目前 CodeBuilder 提供了数据源、模板以及工具三类接口。CodeBuilder 基于.NET Framework 4.0 开发，它使用了动态编译技术，你可以嵌入 C#或VB.NET 代码对对象属性进行个性化的扩展，结合灵活的模板生成你所想要的任何代码。

  `数据源 (ISourceProvider)`。提供数据结构的来源，目前提供 Database、PowerDesign、DbSchema、PDManer、Swagger 等几种数据源。数据库结构基于 Fireasy 的 SchemaProvider，目前支持 SQLite、MsSql(SqlServer)、MySql、PostgreSql、Oracle、Firebird、达梦、人大金仓、神州通用 等数据库，以及 OleDb、Odbc 驱动。

  `模板 (ITemplateProvider)`。提供代码生成的模板，目前提供 T4、Razor 和 NVelocity 三种模板。你可以自行编写符合自己的模板来生成你所想要的代码。

  `工具 (IToolProvider)`。工具是一些常用的小程序，你可以自己开发其后集成到 CodeBuilder 里进行使用。


## 接口定义

### 数据源 ISourceProvider

```csharp
/// <summary>
/// 数据源提供者插件。
/// </summary>
public interface ISourceProvider : IPlugin
{
    /// <summary>
    /// 获取图标，用于指定在使用向导上显示的图标。
    /// </summary>
    Image Icon { get; }

    /// <summary>
    /// 获取该插件的说明。
    /// </summary>
    string Description { get; }

    /// <summary>
    /// 连接数据源，获取预览表。
    /// </summary>
    /// <param name="option">选项。</param>
    /// <returns></returns>
    Task<List<Table>> PreviewAsync(SourceOption option, CancellationToken cancellationToken = default);

    /// <summary>
    /// 从历史中加载数据源。
    /// </summary>
    /// <param name="history"></param>
    /// <param name="option"></param>
    /// <returns></returns>
    Task<List<Table>> FromHistoryAsync(object history, SourceOption option, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取指定表的架构。使用 PreviewAsync 获取得预览表后，选择要生成的表，获取详细的架构信息。
    /// </summary>
    /// <param name="tables">选定的数据表。</param>
    /// <param name="processHandler">数据表的读取进度通知。</param>
    /// <returns></returns>
    Task<List<Table>> GetSchemaAsync(List<Table> tables, TableSchemaProcessHandler processHandler, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取历史记录。
    /// </summary>
    /// <returns></returns>
    IEnumerable<object> GetHistory();

    /// <summary>
    /// 清空历史记录
    /// </summary>
    void ClearHistory();

    /// <summary>
    /// 删除历史记录
    /// </summary>
    void DeleteHistory(object record);
}
```

### 模板 ITemplateProvider

```csharp
/// <summary>
/// 模板提供者插件。
/// </summary>
public interface ITemplateProvider : IPlugin
{
    /// <summary>
    /// 获取插件工作目录。
    /// </summary>
    string WorkDir { get; }

    /// <summary>
    /// 生成代码文件。
    /// </summary>
    /// <param name="option">模板选项。</param>
    /// <param name="tables">所选定的数据表。</param>
    /// <param name="handler">代码生成进度通知。</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<GenerateResult> GenerateFilesAsync(TemplateOption option, List<Table> tables, CodeGenerateHandler handler, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取所有模板定义。
    /// </summary>
    /// <returns></returns>
    List<TemplateDefinition> GetTemplates();

    /// <summary>
    /// 获取模板定义存储信息。
    /// </summary>
    /// <param name="definition"></param>
    /// <returns></returns>
    TemplateStorage GetStorage(TemplateDefinition definition);
}
```

### 工具 IToolProvider

```csharp
/// <summary>
/// 工具提供者插件。
/// </summary>
public interface IToolProvider : IPlugin
{
    /// <summary>
    /// 执行调用工具，返回工具的窗体。
    /// </summary>
    Form Execute();
}

/// <summary>
/// 提供子项的工具插件。
/// </summary>
public interface IMultipleToolProvider : IToolProvider
{
    /// <summary>
    /// 执行调用工具，返回工具的窗体。
    /// </summary>
    /// <param name="name">子工具名称。</param>
    /// <param name="parameter">参数。</param>
    /// <returns></returns>
    Form Execute(string name, object parameter);

    /// <summary>
    /// 获取子工具的菜单。
    /// </summary>
    IEnumerable<IToolMenu> SubItems { get; }
}
```

## 发行版本

  [http://www.fireasy.cn/codebuilder](http://www.fireasy.cn/codebuilder)


## 在线文档

  [http://www.fireasy.cn/codebuilder/docs](http://www.fireasy.cn/codebuilder/docs)


## 欢迎加入

  QQ号： 55570729   QQ群： 6406277、225698098
