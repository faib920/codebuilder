// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using CodeBuilder.Core.Source;
using Fireasy.Common;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace CodeBuilder.Razor
{
    public static class AssemblyConfig
    {
        public static IEnumerable<string> GlobalAssemblies => new List<string> {
            typeof(System.String).Assembly.Location,
            typeof(System.Diagnostics.Trace).Assembly.Location,
            typeof(System.Linq.Expressions.Expression).Assembly.Location,
            typeof(Microsoft.CSharp.RuntimeBinder.RuntimeBinderException).Assembly.Location,
            typeof(System.Data.DbType).Assembly.Location,
            typeof(Column).Assembly.Location,
            typeof(RazorEngine.Razor).Assembly.Location,
            typeof(DisposableBase).Assembly.Location,
            typeof(AssemblyConfig).Assembly.Location
        };

        public static List<string> LoadConfig(IDevHosting devHosting)
        {
            var fileName = Path.Combine(devHosting.WorkPath, "config", "razorassembly.cfg");
            if (!File.Exists(fileName))
            {
                return new List<string>();
            }

            var json = File.ReadAllText(fileName);
            return JsonConvert.DeserializeObject<List<string>>(json);
        }

        public static void SaveConfig(IDevHosting devHosting, List<string> list)
        {
            var fileName = Path.Combine(devHosting.WorkPath, "config", "razorassembly.cfg");

            var json = JsonConvert.SerializeObject(list);
            File.WriteAllText(fileName, json);
        }
    }
}
