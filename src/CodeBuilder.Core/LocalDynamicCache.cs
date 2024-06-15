// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Initializers;
using CodeBuilder.Core.Template;
using CodeBuilder.Core.Validations;
using System;
using System.Collections.Generic;

namespace CodeBuilder.Core
{
    /// <summary>
    /// 本地动态缓存。
    /// </summary>
    public static class LocalDynamicCache
    {
        internal static Dictionary<Type, List<ISchemaInitializer>> SchemaInitializerCache { get; private set; } = new Dictionary<Type, List<ISchemaInitializer>>();
        internal static List<IProfileInitializer> ProfileCache { get; private set; } = new List<IProfileInitializer>();
        internal static List<string> CommonAssemblies { get; private set; } = new List<string>();
        internal static List<Type> CommonExtendTypes { get; private set; } = new List<Type>();
        internal static List<IPartitionOutputParser> PartitionOutputParsers { get; private set; } = new List<IPartitionOutputParser>();
        internal static Dictionary<Type, List<ISchemaValidator>> SchemaValidatorCache { get; private set; } = new Dictionary<Type, List<ISchemaValidator>>();

        /// <summary>
        /// 清理所有缓存。
        /// </summary>
        public static void ClearAll()
        {
            SchemaInitializerCache.Clear();
            ProfileCache.Clear();
            CommonAssemblies.Clear();
            CommonExtendTypes.Clear();
            PartitionOutputParsers.Clear();
            SchemaValidatorCache.Clear();
            StaticUnity.DynamicAssemblies.Clear();
        }

        /// <summary>
        /// 清理 <see cref="IPartitionOutputParser"/> 缓存。
        /// </summary>
        public static void ClearPartitionOutputParsers()
        {
            PartitionOutputParsers.Clear();
        }
    }
}
