// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Forms;
using Fireasy.Common.DependencyInjection;
using System.Linq;

namespace CodeBuilder
{
    public class WindowSetManager : IWindowSetManager, ISingletonService
    {
        public int? GetFlag(string name)
        {
            if (Config.Instance.WindowSets.TryGetValue(name, out var flag))
            {
                return flag;
            }

            return null;
        }

        public void SetFlag(string name, int flag)
        {
            if (Config.Instance.WindowSets.TryGetValue(name, out var flag1) && flag1 == flag)
            {
                return;
            }

            Config.Instance.WindowSets[name] = flag;
            Config.Instance.Save();
        }
    }
}
