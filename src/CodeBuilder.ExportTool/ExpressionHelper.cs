// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Source;
using CodeBuilder.Core.Variable;
using Fireasy.Common.Compiler;
using System;
using System.Collections.Generic;

namespace CodeBuilder.ExportTool
{
    public class ExpressionHelper
    {
        private static Dictionary<string, Func<dynamic, string>> _cache = new Dictionary<string, Func<dynamic, string>>();

        public static Func<dynamic, string> GetEvaluator(string expression)
        {
            if (!_cache.TryGetValue(expression, out Func<dynamic, string> func))
            {
                var compiler = new CSharpCodeCompiler();
                var options = new ConfigureOptions();

                var schema = expression.Substring(0, expression.IndexOf("."));

                var source = $@"
using CodeBuilder.Core.Source;

public class Main
{{
    public static string Evaluate(dynamic {schema})
    {{
        var obj = {expression};
        return obj == null ? string.Empty : obj.ToString();
    }}
}}
";
                options.Assemblies.AddRange(AssemblyReferenceManager.SchemaAssemblies);

                func = compiler.CompileDelegate<Func<dynamic, string>>(source, options: options);
                _cache.Add(expression, func);
            }

            return func;
        }

        public static string EvaluateTable(string expression, Table table)
        {
            var vars = ParserVariables(expression, "Table");

            foreach (var item in vars)
            {
                var func = ExpressionHelper.GetEvaluator(item);
                if (func != null)
                {
                    expression = expression.Replace("{" + item + "}", func(table));
                }
            }

            return expression;
        }

        public static string EvaluateColumn(string expression, Column column)
        {
            var vars = ParserVariables(expression, "Column");

            foreach (var item in vars)
            {
                var func = ExpressionHelper.GetEvaluator(item);
                if (func != null)
                {
                    expression = expression.Replace("{" + item + "}", func(column));
                }
            }

            return expression;
        }

        public static bool IsColumnTemplate(string expression)
        {
            return expression.Contains("{Column.");
        }

        private static List<string> ParserVariables(string expression, string prefix)
        {
            var vars = new List<string>();
            var str = expression;

            while (true)
            {
                var s = str.IndexOf("{" + prefix + ".");
                if (s == -1)
                {
                    break;
                }

                var e = str.IndexOf("}", s);

                if (e == -1)
                {
                    break;
                }

                vars.Add(str.Substring(s + 1, e - s - 1));

                str = str.Substring(e + 1);
            }

            return vars;
        }
    }
}
