// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Initializers;
using Fireasy.Common.Extensions;
using Microsoft.CSharp;
using Microsoft.VisualBasic;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CodeBuilder.Core.Variable
{
    /// <summary>
    /// 公共扩展管理类。
    /// </summary>
    public class CommonExtensionManager
    {
        private static List<Type> _extendTypes = new List<Type>();
        private static List<string> _assemblies = null;

        /// <summary>
        /// 获取公共的动态程序集列表。
        /// </summary>
        /// <returns></returns>
        public static List<string> GetCommonDynamicAssemblies()
        {
            if (_assemblies == null)
            {
                _assemblies = new List<string>();
                _extendTypes = ComplileExtensionTypes();
            }

            return _assemblies;
        }

        /// <summary>
        /// 动态编译扩展动态类。
        /// </summary>
        /// <returns></returns>
        private static List<Type> ComplileExtensionTypes()
        {
            var pluginTypes = new List<Type>();
            foreach (var ext in new[] { ".cs", ".vb" })
            {
                var files = GetExtensionFiles(ext);

                if (files.Length == 0)
                {
                    continue;
                }

                var compiler = new Fireasy.Common.Compiler.CodeCompiler();
                var fileName = Util.GenerateTempFileName();
                _assemblies.Add(fileName);

                compiler.OutputAssembly = fileName;
                compiler.CodeProvider = GetCodeProvider(ext);
                foreach (var ass in AssemblyReferenceManager.CommonAssemblies)
                {
                    compiler.Assemblies.Add(ass);
                }
                pluginTypes.AddRange(compiler.CompileAssembly(files).GetExportedTypes());
            }

            pluginTypes.Where(s => typeof(IProfileInitializer).IsAssignableFrom(s)).ForEach(s => InitializerUnity.Register(s.New<IProfileInitializer>()));

            return pluginTypes;
        }

        /// <summary>
        /// 获取目录下的可扩展的代码文件。
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        private static string[] GetExtensionFiles(string ext)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "extensions\\common");
            if (Directory.Exists(path))
            {
                return Directory.GetFiles(path, "*" + ext);
            }

            return new string[0];
        }

        /// <summary>
        /// 根据文件扩展名获取相应的 <see cref="CodeDomProvider"/>。
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        private static CodeDomProvider GetCodeProvider(string ext)
        {
            if (ext == ".vb")
            {
                return new VBCodeProvider();
            }

            return new CSharpCodeProvider();
        }
    }
}
