// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System.ComponentModel.Composition;
using System.IO;

namespace CodeBuilder.Core.DynamicFunc
{
    [Export(typeof(IDynamicFuncProvider))]
    public class DefaultFuncProvider : IDynamicFuncProvider
    {
        private IDevHosting _hosting;

        public string Name { get; set; } = "默认函数集";

        public void Initialize(IDevHosting hosting)
        {
            _hosting = hosting;
        }

        [DynamicFunc("读取文本文件", Description = "方法 dynamic ReadStrings(string fileName) \r\n示例 (List<string>)Hosting.Funcs.ReadStrings(\"demo.text\");")]
        public dynamic ReadStrings(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }

            return File.ReadAllLines(filePath);
        }
    }
}
