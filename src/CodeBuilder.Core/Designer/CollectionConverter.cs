// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using Fireasy.Common.Extensions;
using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace CodeBuilder.Core.Designer
{
    public class CollectionConverter : System.ComponentModel.CollectionConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is ICollection collection)
            {
                var name = value.GetType().GetEnumerableElementType().Name;
                return $"({collection.Count} {name.ToPlural()})";
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
