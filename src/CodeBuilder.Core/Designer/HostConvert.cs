// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Source;
using System;
using System.ComponentModel;
using System.Globalization;

namespace CodeBuilder.Core.Designer
{
    public class HostConvert : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is Host)
            {
                return "(Host)";
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            var properties = TypeDescriptor.GetProperties(value.GetType());
            return new PropertyDescriptorCollection(new PropertyDescriptor[] { properties[nameof(Host.Tables)] });
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context) => true;
    }
}
