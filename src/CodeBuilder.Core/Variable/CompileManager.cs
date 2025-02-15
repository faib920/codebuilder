// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Initializers;
using CodeBuilder.Core.Source;
using CodeBuilder.Core.Template;
using CodeBuilder.Core.Validations;
using Fireasy.Common.Compiler;
using Fireasy.Common.DependencyInjection;
using Fireasy.Common.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CodeBuilder.Core.Variable
{
    public class CompileManager : ICompileManager, ISingletonService
    {
        private const string FileExtension = ".compiler";
        private readonly IServiceProvider _serviceProvider;

        public CompileManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public CompileResult Common { get; set; }

        public CompileResult Profile { get; set; }

        public CompileResult Schema { get; set; }

        public CompileResult ProfileWrap { get; set; }

        public Dictionary<string, CompileResult> SchemaWrap { get; set; }

        public void Compile(TemplateDefinition definition, bool forceBuild = true)
        {
            var cmpFile = Path.Combine(Util.GetTempPath(), definition.TId + FileExtension);
            var files = GetGetExtensionFiles(definition);

            void internalCompile()
            {
                ComplileExtensionTypes(definition, files);

                var json = JsonConvert.SerializeObject(new { Common, Profile, Schema, ProfileWrap, SchemaWrap }, Formatting.Indented);
                File.WriteAllText(cmpFile, json);
            }

            if (forceBuild || !File.Exists(cmpFile) || CheckHasNewCode(cmpFile, files))
            {
                internalCompile();
            }
            else
            {
                try
                {
                    var content = File.ReadAllText(cmpFile);
                    var result = JsonConvert.DeserializeObject<CompileResultFile>(content);
                    Common = ParseCompileResult(result.Common);
                    Profile = ParseCompileResult(result.Profile);
                    Schema = ParseCompileResult(result.Schema);
                    ProfileWrap = ParseCompileResult(result.ProfileWrap);
                    SchemaWrap = result.SchemaWrap.Where(s => s.Value != null).Select(s => new KeyValuePair<string, CompileResult>(s.Key, ParseCompileResult(s.Value))).ToDictionary(s => s.Key, s => s.Value);
                }
                catch
                {
                    internalCompile();
                }
            }

            LocalDynamicCache.ClearAll();
            InitializeProfile(Profile.Types);
            InitializeSchema(Schema.Types);
            FindPartitionOutputParsers();
        }

        public void ClearExpiredFiles()
        {
            var workDir = Util.GetTempPath();
            if (!Directory.Exists(workDir))
            {
                return;
            }

            Directory.GetFiles(workDir, "*" + FileExtension).AsParallel().ForAll(cmpFile =>
            {
                try
                {
                    var content = File.ReadAllText(cmpFile);
                    var result = JsonConvert.DeserializeObject<CompileResultFile>(content);
                    var files = result.GetAllFiles().ToList();
                    var fileName = new FileInfo(cmpFile).FullName;
                    var path = Path.Combine(Util.GetTempPath(), fileName.Substring(0, fileName.Length - FileExtension.Length));
                    foreach (var file in Directory.GetFiles(path, "*.dll"))
                    {
                        if (!files.Contains(file))
                        {
                            File.Delete(file);
                        }
                    }
                }
                catch { }
            });
        }

        private (FileInfo[] Common, FileInfo[] Profile, FileInfo[] Schema) GetGetExtensionFiles(TemplateDefinition definition)
        {
            var files0 = GetExtensionFiles(definition, "common", s => s.Common);
            var files1 = GetExtensionFiles(definition, "profile", s => s.Profile);
            var files2 = GetExtensionFiles(definition, "schema", s => s.Schema);
            return (files0, files1, files2);
        }

        private bool CheckHasNewCode(string compileFile, (FileInfo[] Common, FileInfo[] Profile, FileInfo[] Schema) files)
        {
            return files.Common.Union(files.Profile).Union(files.Schema).Max(s => s.LastWriteTime) > new FileInfo(compileFile).LastWriteTime;
        }

        /// <summary>
        /// 获取目录下的可扩展的代码文件。
        /// </summary>
        /// <param name="definition">模板定义。</param>
        /// <param name="category"></param>
        /// <returns></returns>
        private FileInfo[] GetExtensionFiles(TemplateDefinition definition, string category, Func<TemplateExtension, List<string>> func, string _ext = null)
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

        private void ComplileExtensionTypes(TemplateDefinition definition, (FileInfo[] Common, FileInfo[] Profile, FileInfo[] Schema) files)
        {
            if (files.Common.Length > 0)
            {
                Common = CompileTypes(definition, files.Common, AssemblyReferenceManager.CommonAssemblies);
            }
            if (files.Profile.Length > 0)
            {
                var assemblies = AssemblyReferenceManager.ProfileAssemblies.Union(Common.Files).Distinct().ToArray();
                Profile = CompileTypes(definition, files.Profile, assemblies);
                assemblies = assemblies.Union(Profile.Files).Distinct().ToArray();
                ProfileWrap = BuildWrapType<Profile>(definition, Profile, s => true, assemblies);
            }
            if (files.Schema.Length > 0)
            {
                bool filter(Type s, Type t)
                {
                    var attr = s.GetCustomAttribute<SchemaExtensionAttribute>();
                    if (attr == null)
                    {
                        return false;
                    }

                    return attr.SchemaType == t;
                };

                var assemblies = AssemblyReferenceManager.SchemaAssemblies.Union(Common.Files).Distinct().ToArray();
                Schema = CompileTypes(definition, files.Schema, assemblies);
                assemblies = assemblies.Union(Schema.Files).Distinct().ToArray();
                SchemaWrap = new Dictionary<string, CompileResult>
                {
                    { nameof(Table), BuildWrapType<Table>(definition, Schema, s => filter(s, typeof(Table)), assemblies) },
                    { nameof(Column), BuildWrapType<Column>(definition, Schema, s => filter(s, typeof(Column)), assemblies) },
                    { nameof(Reference), BuildWrapType<Reference>(definition, Schema, s => filter(s, typeof(Reference)), assemblies) },
                };
            }
        }

        private CompileResult CompileTypes(TemplateDefinition definition, IEnumerable<FileInfo> files, string[] assemblies = null)
        {
            var result = new CompileResult();
            result.Namespaces.Add("System");

            foreach (var g in files.GroupBy(s => s.Extension))
            {
                var language = g.Key.Equals(".cs", StringComparison.OrdinalIgnoreCase) ? "csharp" : "vb";
                var compilerManager = _serviceProvider.TryGetService<ICodeCompilerManager>();
                var compiler = compilerManager.CreateCompiler(language);
                var fileName = Util.GenerateTempFileName(definition.TId, out var assemblyName);
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

                configOpt.OutputAssembly = fileName;
                assemblies?.ForEach(s => configOpt.Assemblies.Add(s));

                try
                {
                    compiler.CompileAssembly(g.Select(s => File.ReadAllText(s.FullName, Encoding.UTF8)), configOpt).GetExportedTypes().ForEach(s => result.Types.Add(s));
                }
                catch (Exception exp)
                {
                    throw new CompileException("编译失败", exp);
                }
            }

            return result;
        }

        private (Type, string) CompileType(TemplateDefinition definition, string source, string[] assemblies)
        {
            var compilerManager = _serviceProvider.TryGetService<ICodeCompilerManager>();
            var compiler = compilerManager.CreateCompiler("C#");
            var fileName = Util.GenerateTempFileName(definition.TId, out var assemblyName);
            StaticUnity.DynamicAssemblies.Add(fileName);

            var configOpt = new ConfigureOptions { AssemblyName = assemblyName };
            configOpt.OutputAssembly = fileName;
            foreach (var ass in assemblies)
            {
                configOpt.Assemblies.Add(ass);
            }

            return (compiler.CompileType(source, null, configOpt), fileName);
        }

        private CompileResult BuildWrapType<T>(TemplateDefinition definition, CompileResult baseResult, Func<Type, bool> filter, string[] assemblies)
        {
            var properties = baseResult.Types.Where(filter).SelectMany(s => s.GetProperties(BindingFlags.Public | BindingFlags.Instance)).ToArray();

            if (properties.Length > 0)
            {
                try
                {
                    var result = BuildWrapType<T>(definition, properties, assemblies);
                    return new CompileResult
                    {
                        Types = new HashSet<Type> { result.Item1 },
                        Files = new HashSet<string> { result.Item2 }
                    };
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }

            return null;
        }

        private (Type, string) BuildWrapType<T>(TemplateDefinition definition, PropertyInfo[] properties, string[] assemblies)
        {
            var source = $@"
{string.Join(Environment.NewLine, Profile.Namespaces.Select(s => "using " + s + ";"))}

public class {typeof(T).Name}_Wrap : {typeof(T).FullName}
{{
{string.Join(Environment.NewLine, typeof(T).GetConstructors().Select(s => $"public {typeof(T).Name}_Wrap(" + string.Join(",", s.GetParameters().Select(p => p.ParameterType.FullName + " " + p.Name)) + ") : base (" + string.Join(",", s.GetParameters().Select(p => p.Name)) + ") { }"))}
{string.Join(Environment.NewLine, properties.Select(s => string.Join(Environment.NewLine, GetPropertyCustomAttributes(s)) + Environment.NewLine + "public " + s.PropertyType.Name + " " + s.Name + " { get; set; }"))}
}}
";

            return CompileType(definition, source, assemblies);
        }

        private CompileResult ParseCompileResult(CompilePersisResult innerResult)
        {
            var types = new HashSet<Type>();
            foreach (var f in innerResult.Files)
            {
                var assembly = Assembly.LoadFrom(f);
                var assemblyName = assembly.GetName().Name;

                foreach (var t in innerResult.Types.Where(s => s.Split(',')[1].Trim() == assemblyName))
                {
                    var typeName = t.Split(',')[0];
                    types.Add(assembly.GetType(typeName, true));
                }
            }

            return new CompileResult { Files = innerResult.Files, Namespaces = innerResult.Namespaces, Types = types };
        }

        private void InitializeProfile(IEnumerable<Type> types)
        {
            types.Where(s => typeof(IProfileInitializer).IsAssignableFrom(s)).ForEach(s => InitializerUnity.Register(Activator.CreateInstance(s) as IProfileInitializer));
        }

        private void InitializeSchema(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                foreach (var initAttr in type.GetCustomAttributes<SchemaInitializerAttribute>(false))
                {
                    var initializer = Activator.CreateInstance(type) as ISchemaInitializer;
                    if (initializer != null)
                    {
                        InitializerUnity.Register(initAttr.SchemaType, initializer);
                    }
                }
                foreach (var valdAttr in type.GetCustomAttributes<SchemaValidatorAttribute>(false))
                {
                    var validator = Activator.CreateInstance(type) as ISchemaValidator;
                    if (validator != null)
                    {
                        Validations.ValidationUnity.Register(valdAttr.SchemaType, validator);
                    }
                }
            }
        }

        /// <summary>
        /// 获取属性的自定义特性。
        /// </summary>
        /// <param name="property"></param>
        /// <param name="dyPropertyBuilder"></param>
        private List<string> GetPropertyCustomAttributes(PropertyInfo property)
        {
            var attributes = new List<string>();
            var broAttr = property.GetCustomAttributes<BrowsableAttribute>().FirstOrDefault();
            var catAttr = property.GetCustomAttributes<CategoryAttribute>().FirstOrDefault();
            var desAttr = property.GetCustomAttributes<DescriptionAttribute>().FirstOrDefault();
            var defAttr = property.GetCustomAttributes<DefaultValueAttribute>().FirstOrDefault();
            var disAttr = property.GetCustomAttributes<DisplayNameAttribute>().FirstOrDefault();
            var reqAttr = property.GetCustomAttributes<RequiredCheckAttribute>().FirstOrDefault();
            var unpAttr = property.GetCustomAttributes<UnPersistentlyAttribute>().FirstOrDefault();

            if (broAttr != null)
            {
                attributes.Add($"[BrowsableAttribute({(broAttr.Browsable ? "true" : "false")})]");
            }

            if (catAttr != null)
            {
                attributes.Add($"[CategoryAttribute(\"{catAttr.Category}\")]");
            }

            if (desAttr != null)
            {
                attributes.Add($"[DescriptionAttribute(\"{desAttr.Description}\")]");
            }

            if (defAttr != null)
            {
                attributes.Add($"[DefaultValueAttribute(typeof({property.PropertyType.Name}), \"{defAttr.Value}\")]");
            }

            if (disAttr != null)
            {
                attributes.Add($"[DisplayNameAttribute(\"{disAttr.DisplayName}\")]");
            }

            if (reqAttr != null)
            {
                attributes.Add($"[RequiredCheckAttribute]");
            }

            if (unpAttr != null)
            {
                attributes.Add($"[UnPersistentlyAttribute]");
            }

            if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
            {
                attributes.Add($"[BrowsableAttribute(false)]");
            }

            return attributes;
        }

        private void FindPartitionOutputParsers()
        {
            var types = new List<Type>();
            if (Profile?.Types?.Any() == true)
            {
                types.AddRange(Profile.Types);
            }
            if (Schema?.Types?.Any() == true)
            {
                types.AddRange(Schema.Types);
            }

            foreach (var type in types)
            {
                if (typeof(IPartitionOutputParser).IsAssignableFrom(type))
                {
                    var parser = Activator.CreateInstance(type) as IPartitionOutputParser;
                    Parser.AddParser(parser);
                }
            }
        }

    }
}
