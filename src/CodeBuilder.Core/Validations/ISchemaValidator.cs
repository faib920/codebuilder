// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace CodeBuilder.Core.Validations
{
    /// <summary>
    /// 架构验证器。
    /// </summary>
    public interface ISchemaValidator
    {
        ValidateResult Validate(dynamic schema);
    }
}
