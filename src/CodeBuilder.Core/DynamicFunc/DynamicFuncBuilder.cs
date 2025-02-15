// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using Fireasy.Common;
using Fireasy.Common.Compiler;
using Fireasy.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CodeBuilder.Core.DynamicFunc
{
    /// <summary>
    /// 动态函数构造器。
    /// </summary>
    public static class DynamicFuncBuilder
    {
        static IEnumerable<string> GlobalAssemblies => new List<string> {
            typeof(System.String).Assembly.Location,
            typeof(System.Diagnostics.Trace).Assembly.Location,
            typeof(System.Linq.Expressions.Expression).Assembly.Location,
            typeof(Microsoft.CSharp.RuntimeBinder.RuntimeBinderException).Assembly.Location,
            typeof(System.Data.DbType).Assembly.Location,
            typeof(DisposableBase).Assembly.Location,
            typeof(IDynamicFuncProvider).Assembly.Location
        };

        /// <summary>
        /// 构造动态函数、
        /// </summary>
        /// <param name="hosting"></param>
        /// <param name="providers"></param>
        /// <returns></returns>
        public static dynamic Build(IDevHosting hosting, IDynamicFuncProvider[] providers)
        {
            providers.ForEach(s => s.Initialize(hosting));

            var source = @"
using System;
using CodeBuilder.Core;
using CodeBuilder.Core.DynamicFunc;

public class DynamicFuncProxy
{
    private IDevHosting _hosting;
    private IDynamicFuncProvider[] _providers;

    public DynamicFuncProxy(IDevHosting hosting, IDynamicFuncProvider[] providers)
    {
        _hosting = hosting;
        _providers = providers;
    }
";
            for (var i = 0; i < providers.Length; i++)
            {
                var methods = providers[i].GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(s => s.IsDefined<DynamicFuncAttribute>());

                foreach (var item in methods)
                {
                    var hasReturn = item.ReturnType != typeof(void);
                    var parameters = item.GetParameters();
                    source += $@"
    public {(hasReturn? "dynamic" : "void")} {item.Name}({string.Join(",", parameters.Select(s => s.ParameterType.FullName + " " + s.Name))})
    {{
        try
        {{
            {(hasReturn ? "return " : "")}(({providers[i].GetType().FullName})_providers[{i}]).{item.Name}({string.Join(",", parameters.Select(s => s.Name))});
        }}
        catch (Exception exp)
        {{
            throw new DynamicFuncInvokeException(""调用函数 {item.Name} 时报错。"", exp);
        }}
    }}

                    ";
                }
            }

            source += @"
}";

            var compilerManager = hosting.ServiceProvider.TryGetService<ICodeCompilerManager>();
            var compiler = compilerManager.CreateCompiler("C#");
            var fileName = Util.GenerateTempFileName("dynamic_funcs", out var assemblyName);
            StaticUnity.DynamicAssemblies.Add(fileName);

            var configOpt = new ConfigureOptions { AssemblyName = assemblyName };
            configOpt.Assemblies.AddRange(GlobalAssemblies);
            providers.Select(s => s.GetType().Assembly).Distinct().Select(s => s.GetName()).ForEach(s =>
            {
                configOpt.Assemblies.Add(s.Name + ".dll");
            });

            configOpt.OutputAssembly = fileName;
            var funcType = compiler.CompileType(source, null, configOpt);

            return Activator.CreateInstance(funcType, new object[] { hosting, providers });
        }
    }
}
