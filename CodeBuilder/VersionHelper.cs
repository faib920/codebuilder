// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Diagnostics;

namespace CodeBuilder
{
    public static class VersionHelper
    {
        /// <summary>
        /// 比较版本。
        /// </summary>
        /// <param name="sourceFileName">本地文件。</param>
        /// <param name="target">目标版本。</param>
        /// <returns></returns>
        public static int CompareVersion(string sourceFileName, string target)
        {
            var version = FileVersionInfo.GetVersionInfo(sourceFileName);
            return CompareVersion(version, target);
        }

        /// <summary>
        /// 比较版本。
        /// </summary>
        /// <param name="source">本地文件的版本。</param>
        /// <param name="target">目标版本。</param>
        /// <returns></returns>
        public static int CompareVersion(FileVersionInfo source, string target)
        {
            return new Version(target).CompareTo(new Version(source.FileMajorPart, source.FileMinorPart, source.FileBuildPart, source.ProductPrivatePart));
        }
    }
}
