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
                var regex = new Regex(@"\{(\w+)\}");
                var matches = regex.Matches(output);
                foreach (Match match in matches)
                {
                    var group = match.Groups[0];
                    var name = group.Value.Substring(1, group.Value.Length - 2);

                    if (schemaType != null)
                    {
                        var property = schemaType.GetProperty(name);
                        if (property != null)
                        {
                            list.Add(new AccessorMap
                            {
                                GroupName = group.Value,
                                Accessor = ReflectionCache.GetAccessor(property),
                                ObjectType = ObjectType.Schema
                            });
                        }
                    }

                    if (profileType != null)
                    {
                        var property = profileType.GetProperty(name);
                        if (property != null)
                        {
                            list.Add(new AccessorMap
                            {
                                GroupName = group.Value,
                                Accessor = ReflectionCache.GetAccessor(property),
                                ObjectType = ObjectType.Profile
                            });
                        }
                    }
                }

                return list;
            });

            foreach (var p in mappers)
            {
                var value = GetPropertyValue(schema, profile, p);
                output = output.Replace(p.GroupName, value.Replace(".", "\\"));
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
        }

        private enum ObjectType
        {
            Schema,
            Profile
        }
    }
}
