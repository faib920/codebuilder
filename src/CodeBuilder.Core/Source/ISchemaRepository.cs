// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System.Collections.Generic;

namespace CodeBuilder.Core.Source
{
    public interface ISchemaRepository
    {
        /// <summary>
        /// 保存架构文件。
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="tables"></param>
        void SaveSchemaFile(string fileName, IEnumerable<Table> tables);

        /// <summary>
        /// 保存关系文件。
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="tables"></param>
        void SaveRelationFile(string fileName, IEnumerable<Table> tables);

        /// <summary>
        /// 读取架构文件。
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        List<Table> ReadFile(string fileName);

        /// <summary>
        /// 读取关系文件。
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="tables"></param>
        void ReadRelationFile(string fileName, IEnumerable<Table> tables);
    }
}