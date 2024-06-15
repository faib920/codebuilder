// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace CodeBuilder.Database
{
    public class DbSourceStruct
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string ConnectionString { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is DbSourceStruct str)
            {
                return Type == str.Type && ConnectionString == str.ConnectionString;
            }

            return false;
        }

        public override string ToString()
        {
            return "(" + Type + "): " + ConnectionString;
        }
    }
}
