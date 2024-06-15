// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace CodeBuilder.Core.Validations
{
    public class ValidateResult
    {
        public static readonly ValidateResult Success = new ValidateResult();

        private List<ValidateEntry> _entries = new List<ValidateEntry>();

        public ValidateResult AddMessage(string message)
        {
            _entries.Add(new ValidateEntry(null, null, message));
            return this;
        }

        public ValidateResult AddMessage(object obj, string propertyName, string message)
        {
            _entries.Add(new ValidateEntry(obj, propertyName, message));
            return this;
        }

        public ValidateResult AddEntry(ValidateEntry entry)
        {
            _entries.Add(entry);
            return this;
        }

        public ValidateResult AddEntries(IEnumerable<ValidateEntry> entries)
        {
            _entries.AddRange(entries);
            return this;
        }

        public static ValidateResult Fail(string message)
        {
            return new ValidateResult().AddMessage(message);
        }

        public IEnumerable<ValidateEntry> GetEntries()
        {
            return _entries;
        }

        public virtual bool IsSuccess => _entries.Count == 0;
    }

    public class ValidateEntry
    {
        public ValidateEntry(object obj, string propertyName, string message)
        {
            Object = obj;
            PropertyName = propertyName;
            Message = message;
        }

        public object Object { get; private set; }

        public string PropertyName { get; private set; }

        public string Message { get; private set; }
    }
}
