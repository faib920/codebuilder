// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace CodeBuilder.Core.Template
{
    /// <summary>
    /// 部件输出路径的解析器。
    /// </summary>
    public interface IPartitionOutputParser
    {
        void Parse(OutputParseContext context);
    }

    public class OutputParseContext
    {
        public OutputParseContext(string output, dynamic schema, dynamic profile)
        {
            Schema = schema;
            Profile = profile;
            Result = output;
        }

        public dynamic Schema { get; }

        public dynamic Profile { get; }

        public string Result { get; set; }
    }
}
