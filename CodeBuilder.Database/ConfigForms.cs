// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace CodeBuilder.Database
{
    public class ConfigForms
    {
        private static Dictionary<string, Func<IConnectionConfig>> _dic = new Dictionary<string, Func<IConnectionConfig>>();

        static ConfigForms()
        {
            _dic.Add("SqlServer", () => new frmMsSqlConfig());
            _dic.Add("Oracle", () => new frmOracleConfig());
            _dic.Add("MySql", () => new frmMySqlConfig());
            _dic.Add("SQLite", () => new frmSQLiteConfig());
            _dic.Add("PostgreSql", () => new frmPostgresqlConfig());
            _dic.Add("Firebird", () => new frmFirebirdConfig());
            _dic.Add("OleDb", () => new DataLinkerDialog());
        }

        public static IConnectionConfig GetConfigForm(string providerName)
        {
            return _dic.TryGetValue(providerName, out Func<IConnectionConfig> r) ? r() : null;
        }
    }
}
