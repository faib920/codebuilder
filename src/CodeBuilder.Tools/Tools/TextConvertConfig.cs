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
using System.Linq;

namespace CodeBuilder.Tools.Tools
{
    public static class TextConvertConfig
    {
        public class KeyValue
        {
            public string Key { get; set; }

            public string Value { get; set; }
        }

        public static List<KeyValue> LoadConfig(IDevHosting devHosting)
        {
            var fileName = Path.Combine(devHosting.WorkPath, "config", "textconverter.cfg");
            if (!File.Exists(fileName))
            {
                var list = new List<KeyValue>();
                list.Add(new KeyValue
                {
                    Key = "属性生成",
                    Value = "/// <summary>\r\n" +
"/// {1}\r\n" +
"/// </summary>\r\n" +
"[JsonProperty(\"{0}\")]\r\n" +
"public string {0} { get; set; }\r\n"
                });
                list.Add(new KeyValue
                {
                    Key = "对象赋值",
                    Value = "target.{0} = source.{0};"
                });
                list.Add(new KeyValue
                {
                    Key = "SQL插入",
                    Value = "insert table values('{0}', '{1}');"
                });

                var json1 = JsonConvert.SerializeObject(list);
                File.WriteAllText(fileName, json1);

                return list;
            }

            var json = File.ReadAllText(fileName);
            return JsonConvert.DeserializeObject<List<KeyValue>>(json);
        }

        public static void SaveConfig(IDevHosting devHosting, List<KeyValue> list)
        {
            var fileName = Path.Combine(devHosting.WorkPath, "config", "textconverter.cfg");

            var json = JsonConvert.SerializeObject(list);
            File.WriteAllText(fileName, json);
        }
    }
}
