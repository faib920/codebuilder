// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace CodeBuilder.Core.Designer
{
    internal class ChangedPropertyConverter : System.ComponentModel.CollectionConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is ICollection collection)
            {
                return $"({collection.Count} Changes)";
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}