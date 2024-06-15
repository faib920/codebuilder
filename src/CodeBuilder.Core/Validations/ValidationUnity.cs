// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Source;
using System;
using System.Collections.Generic;

namespace CodeBuilder.Core.Validations
{
    public static class ValidationUnity
    {
        /// <summary>
        /// 为指定的架构类型注册验证器。
        /// </summary>
        /// <param name="schemaType">架构类型。</param>
        /// <param name="validator">验证器。</param>
        public static void Register(Type schemaType, ISchemaValidator validator)
        {
            if (!LocalDynamicCache.SchemaValidatorCache.TryGetValue(schemaType, out List<ISchemaValidator> list))
            {
                list = new List<ISchemaValidator>();
                LocalDynamicCache.SchemaValidatorCache.Add(schemaType, list);
            }

            list.Add(validator);
        }

        /// <summary>
        /// 验证架构。
        /// </summary>
        /// <param name="hosting"></param>
        /// <param name="schema">架构对象。</param>
        public static ValidateResult Validate(IDevHosting hosting, object schema)
        {
            if (schema != null)
            {
                foreach (var kvp in LocalDynamicCache.SchemaValidatorCache)
                {
                    if (kvp.Key.IsAssignableFrom(schema.GetType()))
                    {
                        foreach (var validator in kvp.Value)
                        {
                            var result = Util.AttachDevHosting(validator, hosting).Validate(schema);
                            if (!result.IsSuccess)
                            {
                                return result;
                            }
                        }
                    }
                }
            }

            return ValidateResult.Success;
        }
    }
}
