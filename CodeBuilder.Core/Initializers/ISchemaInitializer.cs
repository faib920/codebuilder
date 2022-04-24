// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace CodeBuilder.Core.Initializers
{
    /// <summary>
    /// 提供对架构信息进行初始化的接口。
    /// </summary>
    public interface ISchemaInitializer
    {
        void Initialize(dynamic profile, dynamic schema);
    }
}
