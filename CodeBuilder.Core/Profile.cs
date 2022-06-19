// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace CodeBuilder.Core
{
    /// <summary>
    /// 变量。
    /// </summary>
    public class Profile : IProfileInfo
    {
        private string _fileName;

        string IProfileInfo.FileName { get => _fileName; set => _fileName = value; }
    }

    public interface IProfileInfo
    {
        string FileName { get; set; }
    }
}
