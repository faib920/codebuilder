using CodeBuilder.Core.Template;
using Fireasy.Common.Extensions;
using Microsoft.CSharp;
using Microsoft.VisualBasic;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CodeBuilder.Core.Variable
{
    public class BaseExtensionManager
    {
        /// <summary>
        /// 获取目录下的可扩展的代码文件。
        /// </summary>
        /// <param name="definition">模板定义。</param>
        /// <param name="ext"></param>
        /// <returns></returns>
        protected static FileInfo[] GetExtensionFiles(TemplateDefinition definition, string category, Func<TemplateExtension, List<string>> func)
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
                        .Where(s => s.Exists));
                }
            }

            return list.ToArray();
        }

        /// <summary>
        /// 根据文件扩展名获取相应的 <see cref="CodeDomProvider"/>。
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        protected static CodeDomProvider GetCodeProvider(string ext)
        {
            if (ext == ".vb")
            {
                return new VBCodeProvider();
            }

            return new CSharpCodeProvider();
        }

        protected static List<Type> CompileTypes(TemplateDefinition definition, IEnumerable<FileInfo> files, string[] assemblies, bool common = false, Action<string> assemblyAdd = null)
        {
            var pluginTypes = new List<Type>();

            foreach (var g in files.GroupBy(s => s.Extension))
            {
                var compiler = new Fireasy.Common.Compiler.CodeCompiler();
                var fileName = Util.GenerateTempFileName();
                StaticUnity.DynamicAssemblies.Add(fileName);

                if (assemblyAdd != null)
                {
                    assemblyAdd.Invoke(fileName);
                }

                compiler.OutputAssembly = fileName;
                compiler.CodeProvider = GetCodeProvider(g.Key);
                foreach (var ass in assemblies)
                {
                    compiler.Assemblies.Add(ass);
                }

                if (!common)
                {
                    compiler.Assemblies.AddRange(CommonExtensionManager.GetCommonDynamicAssemblies(definition));
                }

                try
                {
                    pluginTypes.AddRange(compiler.CompileAssembly(g.Select(s => s.FullName).ToArray()).GetExportedTypes());
                }
                catch (Exception exp)
                {
                    throw new CompileException(exp.Message);
                }
            }

            return pluginTypes;
        }


        /// <summary>
        /// 初始化对象的默认值。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected static T InitializeDefaultValue<T>(T obj, Type _wrapType)
        {
            if (obj == null)
            {
                return default;
            }

            var map = _wrapType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(s => s.CanWrite)
                .Select(s => new { Property = s, DefaultValue = s.GetCustomAttributes<DefaultValueAttribute>().FirstOrDefault() })
                .Where(s => s.DefaultValue != null)
                .ToArray();

            foreach (var item in map)
            {
                item.Property.FastSetValue(obj, item.DefaultValue.Value);
            }

            return obj;
        }
    }
}
