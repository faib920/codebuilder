// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using System.Drawing;

namespace CodeBuilder
{
    internal static class Consts
    {
        public static readonly string ApiUrl = WebHelper.HomeUrl + "/api";

        public static readonly string PluginServerUrl = ApiUrl + "/codebuilder/plugins";

        public static readonly string TemplateServerUrl = ApiUrl + "/codebuilder/templates";

        public static readonly Color AddedColor = Color.FromArgb(236, 255, 236);
        public static readonly Color AddedColor1 = Color.FromArgb(68, 200, 68);

        public static readonly Color ModifiedColor = Color.FromArgb(236, 236, 255);
        public static readonly Color ModifiedColor1 = Color.FromArgb(90, 90, 245);

        public static readonly Color RemovedColor = Color.FromArgb(255, 236, 236);
        public static readonly Color RemovedColor1 = Color.FromArgb(236, 68, 68);

    }
}
