// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CodeBuilder.Core;

namespace CodeBuilder
{
    internal static class Consts
    {
        public static readonly string ApiUrl = WebHelper.HomeUrl + "/api";

        public static readonly string PluginServerUrl = ApiUrl + "/codebuilder/plugins";

        public static readonly string TemplateServerUrl = ApiUrl + "/codebuilder/templates";
    }
}
