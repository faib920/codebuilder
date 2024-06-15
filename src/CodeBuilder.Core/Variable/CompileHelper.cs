// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Template;
using Fireasy.Common.Compiler;
using Fireasy.Common.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CodeBuilder.Core.Variable
{
    public class CompileHelper
    {
        /// <summary>
        /// 动态编译扩展动态类。
        /// </summary>
        /// <param name="definition">模板定义。</param>
        /// <returns></returns>
        internal static CompileResult ComplileCommonExtensionTypes(IServiceProvider serviceProvider, TemplateDefinition definition)
        {
            var pluginTypes = new List<Type>();
            var files = GetExtensionFiles(definition, "common", s => s.Common);

            if (files.Length == 0)
            {
                return new CompileResult();
            }

            return CompileTypes(serviceProvider, definition, files, AssemblyReferenceManager.CommonAssemblies, true, fileName => LocalDynamicCache.CommonAssemblies.Add(fileName));
        }

        /// <summary>
        /// 获取目录下的可扩展的代码文件。
        /// </summary>
        /// <param name="definition">模板定义。</param>
        /// <param name="category"></param>
        /// <returns></returns>
        internal static FileInfo[] GetExtensionFiles(TemplateDefinition definition, string category, Func<TemplateExtension, List<string>> func, string _ext = null)
        {
            var workPath = DevHostingHolder.Instance.WorkPath;
            var list = new List<FileInfo>();

            if (definition.Extension != null)
            {
                if (definition.Extension.UseBase)
                {
                    var path = Path.Combine(workPath, "extensions", category);
                    if (Directory.Exists(path))
                    {
                        foreach (var ext in new[] { "*.cs", "*.vb" })
                        {
                            list.AddRange(Directory.GetFiles(path, "base" + ext).Select(s => new FileInfo(s)));
                        }
                    }
                }

                List<string> vs;
                if ((vs = func(definition.Extension)) != null && vs.Count > 0)
                {
                    list.AddRange(vs
                        .Select(s => new FileInfo(Path.Combine(workPath, "extensions", category, s)))
                        .Where(s => s.Exists && !list.Any(t => t.FullName.Equals(s.FullName, StringComparison.InvariantCultureIgnoreCase))));
                }
            }

            return list.Distinct().ToArray();
        }

        internal static CompileResult CompileTypes(IServiceProvider serviceProvider, TemplateDefinition definition, IEnumerable<FileInfo> files, string[] assemblies, bool common = false, Action<string> assemblyAdd = null)
        {
            var result = new CompileResult();
            result.Namespaces.Add("System");

            foreach (var g in files.GroupBy(s => s.Extension))
            {
                var language = g.Key.Equals(".cs", StringComparison.OrdinalIgnoreCase) ? "csharp" : "vb";
                var compilerManager = serviceProvider.TryGetService<ICodeCompilerManager>();
                var compiler = compilerManager.CreateCompiler(language);
                var fileName = Util.GenerateTempFileName(out var assemblyName);
                StaticUnity.DynamicAssemblies.Add(fileName);

                result.Files.Add(fileName);

                var configOpt = new ConfigureOptions { AssemblyName = assemblyName };
                configOpt.PostCompileHandler = c =>
                {
                    foreach (var tree in c.SyntaxTrees)
                    {
                        foreach (var node in tree.GetRoot().DescendantNodes())
                        {
                            if (node.GetType().Name == "ImportsStatementSyntax")
                            {
                                var ns = node.GetType().GetProperty("ImportsClauses").GetValue(node).ToString();
                                result.Namespaces.Add(ns);
                            }
                            else if (node.GetType().Name == "UsingDirectiveSyntax")
                            {
                                var ns = node.GetType().GetProperty("Name").GetValue(node).ToString();
                                result.Namespaces.Add(ns);
                            }

                        }
                    }
                };

                if (assemblyAdd != null)
                {
                    assemblyAdd.Invoke(fileName);
                }

                configOpt.OutputAssembly = fileName;
                foreach (var ass in assemblies)
                {
                    configOpt.Assemblies.Add(ass);
                }

                if (!common)
                {
                    if (LocalDynamicCache.CommonAssemblies.Count == 0)
                    {
                        var result1 = ComplileCommonExtensionTypes(serviceProvider, definition);
                        LocalDynamicCache.CommonExtendTypes.AddRange(result1.Types);
                        result1.Namespaces.ForEach(s => result.Namespaces.Add(s));
                    }

                    configOpt.Assemblies.AddRange(LocalDynamicCache.CommonAssemblies);
                }

                try
                {
                    result.Types.AddRange(compiler.CompileAssembly(g.Select(s => File.ReadAllText(s.FullName, Encoding.UTF8)), configOpt).GetExportedTypes());
                }
                catch (Exception exp)
                {
                    throw new CompileException(exp.Message);
                }
            }

            return result;
        }

        internal static Type CompileType(IServiceProvider serviceProvider, string source, string[] assemblies, bool common = false, Action<string> assemblyAdd = null)
        {
            var compilerManager = serviceProvider.TryGetService<ICodeCompilerManager>();
            var compiler = compilerManager.CreateCompiler("C#");
            var fileName = Util.GenerateTempFileName(out var assemblyName);
            StaticUnity.DynamicAssemblies.Add(fileName);

            var configOpt = new ConfigureOptions { AssemblyName = assemblyName };
            configOpt.OutputAssembly = fileName;
            foreach (var ass in assemblies)
            {
                configOpt.Assemblies.Add(ass);
            }

            return compiler.CompileType(source, null, configOpt);
        }
    }

    public class CompileResult
    {
        public List<Type> Types { get; set; } = new List<Type>();

        public HashSet<string> Namespaces { get; set; } = new HashSet<string>();

        public List<string> Files { get; set; } = new List<string>();
    }
}
