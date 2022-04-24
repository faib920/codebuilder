// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Template;
using System.Collections.Generic;
using System.Linq;

namespace CodeBuilder.Core
{
    /// <summary>
    /// 模板辅助类。
    /// </summary>
    public class TemplateUnity
    {
        /// <summary>
        /// 获取每一类模板下的所有模板定义。
        /// </summary>
        public static Dictionary<string, List<TemplateDefinition>> Templates { get; } = new Dictionary<string, List<TemplateDefinition>>();

        /// <summary>
        /// 清除所有模板定义。
        /// </summary>
        public static void Clear()
        {
            Templates.Clear();
        }

        /// <summary>
        /// 添加一个模板定义。
        /// </summary>
        /// <param name="category">分类，使用 <see cref="ITemplateProvider"/> 实例的名称。</param>
        /// <param name="definition">模板定义。</param>
        public static void Add(string category, TemplateDefinition definition)
        {
            if (!Templates.TryGetValue(category, out List<TemplateDefinition> list))
            {
                list = new List<TemplateDefinition>();
                Templates.Add(category, list);
            }

            list.Add(definition);
        }

        /// <summary>
        /// 尝试返回一个标识对应的模板定义。
        /// </summary>
        /// <param name="category">分类，使用 <see cref="ITemplateProvider"/> 实例的名称。</param>
        /// <param name="id">模板标识。</param>
        /// <param name="definition">模板定义。</param>
        /// <returns></returns>
        public static bool TryGet(string category, string id, out TemplateDefinition definition)
        {
            if (Templates.TryGetValue(category, out List<TemplateDefinition> list))
            {
                definition = list.FirstOrDefault(s => s.Id == id);
                return definition != null;
            }

            definition = null;
            return false;
        }
    }
}
