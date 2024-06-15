// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;

namespace CodeBuilder.Core
{
    /// <summary>
    /// 变量。
    /// </summary>
    public class Profile : MarshalByRefObject, IProfileInfo
    {
        private string _fileName;

        string IProfileInfo.FileName { get => _fileName; set => _fileName = value; }
    }

    public interface IProfileInfo
    {
        string FileName { get; set; }
    }
}
