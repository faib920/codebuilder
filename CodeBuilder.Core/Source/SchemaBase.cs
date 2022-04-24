// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.ComponentModel;

namespace CodeBuilder.Core.Source
{
    public interface _IIdentity
    {
        string _ID { get; set; }
    }

    public abstract class SchemaBase : _IIdentity
    {
        protected SchemaBase()
        {
            _ID = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 获取或设置唯一标识。
        /// </summary>
        [Browsable(false)]
        public string _ID { get; set; }

        public override bool Equals(object obj)
        {
            return ((SchemaBase)obj)._ID == _ID;
        }

        public override int GetHashCode()
        {
            return _ID.GetHashCode();
        }
    }
}
