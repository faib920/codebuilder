## 代码生成器 CodeBuilder

  CodeBuilder 是一款功能强大的代码生成工具。它能将你所设计的数据库结构转换成你所想要的任何文本形式的文件，如 Java、C#、VB、Python、Javascript 等代码文件，以及 SQL 脚本、数据库设计文档等。通过开发插件，你甚至可将其转换成 Word、PDF 等二进制文件。

  CodeBuilder 基于插件式、开放式的思想，你通过实现其定义的接口，就可轻松地将你所开发的插件集成到 CodeBuilder 中来。目前 CodeBuilder 提供了数据源、模板以及工具三类接口。CodeBuilder 基于.NET Framework 4.0 开发，它使用了动态编译技术，你可以嵌入 C#或VB.NET 代码对对象属性进行个性化的扩展，结合灵活的模板生成你所想要的任何代码。

  `数据源 (ISourceProvider)`。提供数据结构的来源，目前提供 Database、PowerDesign、DbSchema、PDManer、Swagger 等几种数据源。数据库结构基于 Fireasy 的 SchemaProvider，目前支持 SQLite、MsSql(SqlServer)、MySql、PostgreSql、Oracle、Firebird、达梦、人大金仓、神州通用 等数据库，以及 OleDb、Odbc 驱动。

  `模板 (ITemplateProvider)`。提供代码生成的模板，目前提供 T4、Razor 和 NVelocity 三种模板。你可以自行编写符合自己的模板来生成你所想要的代码。

  `工具 (IToolProvider)`。工具是一些常用的小程序，你可以自己开发其后集成到 CodeBuilder 里进行使用。



## 发行版本

  [http://www.fireasy.cn/codebuilder](http://www.fireasy.cn/codebuilder)


## 在线文档

  [http://www.fireasy.cn/codebuilder/docs](http://www.fireasy.cn/codebuilder/docs)


## 欢迎加入

  QQ号： 55570729   QQ群： 6406277、225698098
