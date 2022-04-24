// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Variable;
using Microsoft.VisualStudio.TextTemplating;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CodeBuilder.T4
{
    [Serializable]
    public class TemplateHost : ITextTemplatingEngineHost
    {
        private CompilerErrorCollection _errorCollection;
        private string _path;
        private readonly List<string> _partitions;
        private string _fileExtention;
        private Encoding _fileEncoding;
        private readonly List<string> _assemblyLocationList = new List<string>();
        private readonly List<string> _namespaceList = new List<string>();

        public TemplateHost(string path, dynamic tables, dynamic references, List<string> assemblyList, List<string> partitions, GuidDispatcher guids)
        {
            _path = path;
            _partitions = partitions;

            Tables = tables;
            References = references;
            Guids = guids;

            Initialize();

            if (assemblyList != null)
            {
                _assemblyLocationList.AddRange(assemblyList);
            }
        }

        public dynamic Tables { get; private set; }

        public dynamic References { get; private set; }

        public dynamic Current { get; set; }

        public dynamic Profile { get; set; }

        public bool HasPartition(string partName)
        {
            return _partitions.Contains(partName);
        }

        public GuidDispatcher Guids { get; set; }

        public object GetHostOption(string optionName)
        {
            object obj2 = null;
            string str;
            if (((str = optionName) != null) && (str == "CacheAssemblies"))
            {
                obj2 = true;
            }
            return obj2;
        }

        public bool LoadIncludeText(string requestFileName, out string content, out string location)
        {
            content = string.Empty;
            location = string.Empty;
            requestFileName = Path.Combine(_path, requestFileName);
            
            if (File.Exists(requestFileName))
            {
                content = File.ReadAllText(requestFileName);
                return true;
            }

            return false;
        }

        public void LogErrors(CompilerErrorCollection errors)
        {
            _errorCollection = errors;
        }

        public AppDomain ProvideTemplatingAppDomain(string content)
        {
            return SingleAppDomainScope.Current.AppDomain;
        }

        public string ResolveAssemblyReference(string assemblyReference)
        {
            if (File.Exists(assemblyReference))
            {
                return assemblyReference;
            }

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, assemblyReference);
            if (File.Exists(path))
            {
                return path;
            }

            return string.Empty;
        }

        public Type ResolveDirectiveProcessor(string processorName)
        {
            string.Compare(processorName, "XYZ", StringComparison.OrdinalIgnoreCase);
            throw new Exception("没有找到指令处理器");
        }

        public string ResolveParameterValue(string directiveId, string processorName, string parameterName)
        {
            if (directiveId == null)
            {
                throw new ArgumentNullException("directiveId");
            }

            if (processorName == null)
            {
                throw new ArgumentNullException("processorName");
            }

            if (parameterName == null)
            {
                throw new ArgumentNullException("parameterName");
            }

            return string.Empty;
        }

        public string ResolvePath(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            if (!File.Exists(path))
            {
                string str = Path.Combine(Path.GetDirectoryName(TemplateFile), path);
                if (File.Exists(str))
                {
                    return str;
                }
            }

            return path;
        }

        public void SetFileExtension(string extension)
        {
            _fileExtention = extension;
        }

        public void SetOutputEncoding(Encoding encoding, bool fromOutputDirective)
        {
            _fileEncoding = encoding;
        }

        public IList<string> StandardAssemblyReferences
        {
            get { return _assemblyLocationList; }
        }

        public IList<string> StandardImports
        {
            get { return _namespaceList; }
        }

        public string TemplateFile { get; set; }

        public CompilerErrorCollection Errors
        {
            get { return _errorCollection; }
        }

        private void Initialize()
        {
            _assemblyLocationList.Add(typeof(System.String).Assembly.Location);
            _assemblyLocationList.Add(typeof(System.Diagnostics.Trace).Assembly.Location);
            _assemblyLocationList.Add(typeof(System.Linq.Expressions.Expression).Assembly.Location);
            _assemblyLocationList.Add(typeof(Microsoft.CSharp.RuntimeBinder.RuntimeBinderException).Assembly.Location);
            _assemblyLocationList.Add(typeof(System.Data.DbType).Assembly.Location);

            _assemblyLocationList.Add("CodeBuilder.Core.dll");
            _assemblyLocationList.Add("Fireasy.Common.dll");
            _assemblyLocationList.Add("CodeBuilder.T4.dll");
            _namespaceList.Add("System");
            _namespaceList.Add("System.IO");
            _namespaceList.Add("System.Text");
            _namespaceList.Add("System.Text.RegularExpressions");
            _namespaceList.Add("System.Diagnostics");
            _namespaceList.Add("System.Collections");
            _namespaceList.Add("System.Collections.Generic");
            _namespaceList.Add("System.Linq");
            _namespaceList.Add("System.Data");
            _namespaceList.Add("System.Dynamic");
            _namespaceList.Add("CodeBuilder.Core");
            _namespaceList.Add("CodeBuilder.T4");
            _namespaceList.Add("Fireasy.Common");
            _namespaceList.Add("Fireasy.Common.Extensions");
        }
    }
}
