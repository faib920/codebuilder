// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace CodeBuilder.Core.Source
{
    public class ChangedProperty
    {
        public ChangedProperty(string name, object oldValue, object newValue)
        {
            Name = name;
            OldValue = oldValue;
            NewValue = newValue;
        }

        public string Name { get; set; }

        public object OldValue { get; set; }

        public object NewValue { get; set; }
    }
}
