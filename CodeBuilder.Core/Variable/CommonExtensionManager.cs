// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Initializers;
using CodeBuilder.Core.Template;
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
    public class CommonExtensionManager : BaseExtensionManager
    {
        private static List<Type> _extendTypes = new List<Type>();
        private static List<string> _assemblies = null;

        /// <summary>
        /// 获取公共的动态程序集列表。
        /// </summary>
        /// <param name="definition">模板定义。</param>
        /// <returns></returns>
        public static List<string> GetCommonDynamicAssemblies(TemplateDefinition definition)
        {
            if (_assemblies == null)
            {
                _assemblies = new List<string>();
                _extendTypes = ComplileExtensionTypes(definition);
            }

            return _assemblies;
        }

        /// <summary>
        /// 动态编译扩展动态类。
        /// </summary>
        /// <param name="definition">模板定义。</param>
        /// <returns></returns>
        private static List<Type> ComplileExtensionTypes(TemplateDefinition definition)
        {
            var pluginTypes = new List<Type>();
            var files = GetExtensionFiles(definition, "common", s => s.Common);

            if (files.Length == 0)
            {
                return new List<Type>();
            }

            return CompileTypes(definition, files, AssemblyReferenceManager.CommonAssemblies, true, fileName => _assemblies.Add(fileName));
        }

        /// <summary>
        /// 获取目录下的可扩展的代码文件。
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        private static string[] GetExtensionFiles(string ext)
        {
            var path = Path.Combine(DevHostingHolder.Instance.WorkPath, "extensions", "common");
            if (Directory.Exists(path))
            {
                return Directory.GetFiles(path, "*" + ext);
            }

            return new string[0];
        }
    }
}
