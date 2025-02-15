// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using CodeBuilder.Core.DynamicFunc;
using Fireasy.Common.Extensions;
using Fireasy.Data;
using Fireasy.Data.Provider;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;

namespace CodeBuilder.Database
{
    [Export(typeof(IDynamicFuncProvider))]
    public class DbFuncProvider : IDynamicFuncProvider
    {
        private IDevHosting _hosting;

        public string Name { get; set; } = "数据函数集";

        public void Initialize(IDevHosting hosting)
        {
            _hosting = hosting;
        }

        [DynamicFunc("查询Excel", Description = "方法 dynamic QueryExcel(string fileName, string sheet = null) \r\n示例 (List<dynamic>)Hosting.Funcs.QueryExcel(\"demo.xlsx\");")]
        public dynamic QueryExcel(string fileName, string sheet = null)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }

            var factory = _hosting.ServiceProvider.TryGetService<IDatabaseFactory>();
            using (var db = factory.CreateDatabase<OleDbProvider>($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|appdir|{fileName};Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'"))
            {
                var enumerable = db.ExecuteEnumerableAsync((SqlCommand)$"select * from [{(sheet ?? "sheet1$")}]").ConfigureAwait(false).GetAwaiter().GetResult();
                return new List<dynamic>(enumerable);
            }
        }

        [DynamicFunc("查询数据库", Description = "方法 dynamic QueryDb(string providerName, string sql) \r\n示例 (List<dynamic>)Hosting.Funcs.QueryDb(\"sqlite\", \"select * from customers\");\r\n其他 providerName 的配置请参考 appsettings.json 文件中的 dataInstances 配置节")]
        public dynamic QueryDb(string providerName, string sql)
        {
            var factory = _hosting.ServiceProvider.TryGetService<IDatabaseFactory>();
            using (var db = factory.CreateDatabase(providerName))
            {
                var enumerable = db.ExecuteEnumerableAsync((SqlCommand)sql).ConfigureAwait(false).GetAwaiter().GetResult();
                return new List<dynamic>(enumerable);
            }
        }
    }
}
