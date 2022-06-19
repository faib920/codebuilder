// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using Fireasy.Common.Extensions;
using Fireasy.Common.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CodeBuilder.Core.Template
{
    /// <summary>
    /// 文件路径解析器。
    /// </summary>
    public class Parser
    {
        //缓存
        private Dictionary<string, List<AccessorMap>> _cache = new Dictionary<string, List<AccessorMap>>();
        private static readonly List<IPartitionOutputParser> _parserCache = new List<IPartitionOutputParser>();

        /// <summary>
        /// 清理缓存。
        /// </summary>
        public static void ClearCache()
        {
            _parserCache.Clear();
        }

        /// <summary>
        /// 注册解析器。
        /// </summary>
        /// <param name="parser">解析器。</param>
        public static void AddParser(IPartitionOutputParser parser)
        {
            if (parser != null)
            {
                _parserCache.Add(parser);
            }
        }

        public static string PreParse(string output, dynamic schema, dynamic profile)
        {
            var context = new OutputParseContext(output, schema, profile);
            foreach (var p in _parserCache)
            {
                p.Parse(context);
            }

            return context.Result;
        }

        /// <summary>
        /// 使用架构对象及变量对象解析输出的文件路径。
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="profile"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public string Parse(object schema, object profile, string output)
        {
            var mappers = _cache.TryGetValue(output, () =>
            {
                var list = new List<AccessorMap>();
                var schemaType = schema != null ? schema.GetType() : null;
                var profileType = profile != null ? profile.GetType() : null;
                var regex = new Regex(@"\{.(\w+)\}");
                var matches = regex.Matches(output);
                foreach (Match match in matches)
                {
                    var group = match.Groups[0];
                    var name = group.Value.Substring(1, group.Value.Length - 2);
                    var isDot = name[0] == '.';

                    if (schemaType != null)
                    {
                        var property = schemaType.GetProperty(name.Replace(".", string.Empty));
                        if (property != null)
                        {
                            list.Add(new AccessorMap
                            {
                                GroupName = group.Value,
                                Accessor = ReflectionCache.GetAccessor(property),
                                ObjectType = ObjectType.Schema,
                                IsDot = isDot
                            });
                        }
                    }

                    if (profileType != null)
                    {
                        var property = profileType.GetProperty(name.Replace(".", string.Empty));
                        if (property != null)
                        {
                            list.Add(new AccessorMap
                            {
                                GroupName = group.Value,
                                Accessor = ReflectionCache.GetAccessor(property),
                                ObjectType = ObjectType.Profile,
                                IsDot = isDot
                            });
                        }
                    }
                }

                return list;
            });

            foreach (var p in mappers)
            {
                var value = GetPropertyValue(schema, profile, p);

                if (p.IsDot && !string.IsNullOrEmpty(value))
                {
                    output = output.Replace(p.GroupName, (p.IsDot ? "." : string.Empty) + value.Replace(".", "\\"));
                }
                else
                {
                    output = output.Replace(p.GroupName, value.Replace(".", "\\"));
                }
            }

            return output;
        }

        /// <summary>
        /// 获取属性的值。
        /// </summary>
        /// <param name="schema">架构对象。</param>
        /// <param name="profile">变量对象。</param>
        /// <param name="map"></param>
        /// <returns></returns>
        private static string GetPropertyValue(object schema, object profile, AccessorMap map)
        {
            if (map.ObjectType == ObjectType.Schema)
            {
                return map.Accessor.GetValue(schema).ToStringSafely();
            }
            else if (map.ObjectType == ObjectType.Profile)
            {
                return map.Accessor.GetValue(profile).ToStringSafely();
            }

            return string.Empty;
        }

        /// <summary>
        /// <see cref="PropertyAccessor"/> 映射。
        /// </summary>
        private class AccessorMap
        {
            public string GroupName { get; set; }

            public PropertyAccessor Accessor { get; set; }

            public ObjectType ObjectType { get; set; }

            public bool IsDot { get; set; }
        }

        private enum ObjectType
        {
            Schema,
            Profile
        }
    }
}
