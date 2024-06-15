// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using Fireasy.Common.Extensions;
using Fireasy.Data.Provider;
using System;
using System.Collections.Generic;

namespace CodeBuilder.Database
{
    public class ConfigForms
    {
        private static Dictionary<string, Func<IDevHosting, IProviderManager, IConnectionConfig>> _dic = new Dictionary<string, Func<IDevHosting, IProviderManager, IConnectionConfig>>();

        static ConfigForms()
        {
            _dic.Add("SqlServer", (h, m) => new frmMsSqlConfig(h).SetProvider(m.GetDefinedProvider("SqlServer")));
            _dic.Add("Oracle", (h, m) => new frmOracleConfig(h).SetProvider(m.GetDefinedProvider("Oracle")));
            _dic.Add("MySql", (h, m) => new frmMySqlConfig(h).SetProvider(m.GetDefinedProvider("MySql")));
            _dic.Add("SQLite", (h, m) => new frmSQLiteConfig(h).SetProvider(m.GetDefinedProvider("SQLite")));
            _dic.Add("PostgreSql", (h, m) => new frmPostgresqlConfig(h).SetProvider(m.GetDefinedProvider("PostgreSql")));
            _dic.Add("Firebird", (h, m) => new frmFirebirdConfig(h).SetProvider(m.GetDefinedProvider("Firebird")));
            _dic.Add("Dameng", (h, m) => new frmDamentConfig(h).SetProvider(m.GetDefinedProvider("Dameng")));
            _dic.Add("Kingbase", (h, m) => new frmKingbaseConfig(h).SetProvider(m.GetDefinedProvider("Kingbase")));
            _dic.Add("ShenTong", (h, m) => new frmShenTongConfig(h).SetProvider(m.GetDefinedProvider("ShenTong")));
            _dic.Add("OleDb", (h, m) => new DataLinkerDialog());
            _dic.Add("Odbc", (h, m) => new frmOdbcConfig(h).SetProvider(m.GetDefinedProvider("Odbc")));
        }

        public static IConnectionConfig GetConfigForm(IServiceProvider serviceProvider, string providerName)
        {
            var hosting = serviceProvider.TryGetService<IDevHosting>();
            var providerManager = serviceProvider.TryGetService<IProviderManager>();
            return _dic.TryGetValue(providerName, out Func<IDevHosting, IProviderManager, IConnectionConfig> r) ? r(hosting, providerManager) : null;
        }
    }
}
