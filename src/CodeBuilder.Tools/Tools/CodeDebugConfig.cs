// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace CodeBuilder.Tools.Tools
{
    public static class CodeDebugConfig
    {
        public static IEnumerable<string> GlobalAssemblies => new List<string> { "System.Core.dll", "System.dll", "Microsoft.CSharp.dll", "Fireasy.Common.dll", "Newtonsoft.Json.dll" };

        public static List<string> LoadConfig(IDevHosting devHosting)
        {
            var fileName = Path.Combine(devHosting.WorkPath, "config", "codedebugger.cfg");
            if (!File.Exists(fileName))
            {
                return new List<string>();
            }

            var json = File.ReadAllText(fileName);
            return JsonConvert.DeserializeObject<List<string>>(json);
        }

        public static void SaveConfig(IDevHosting devHosting, List<string> list)
        {
            var fileName = Path.Combine(devHosting.WorkPath, "config", "codedebugger.cfg");

            var json = JsonConvert.SerializeObject(list);
            File.WriteAllText(fileName, json);
        }
    }
}
