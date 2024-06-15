// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace CodeBuilder
{
    public class CommResult<T>
    {
        public bool Succeed { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }
    }
}
