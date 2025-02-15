// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using Fireasy.Windows.Forms;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CodeBuilder.Core.Source
{
    /// <summary>
    /// 数据源助手。
    /// </summary>
    public interface ISourceAssistant
    {
        string Name { get; }

        bool PreHandle(IEnumerable<Table> tables);

        Task HandleAsync(IEnumerable<Table> tables, CancellationToken calcelToken);

        void PostHandle(SourceAssistantPostHandleContext context);
    }

    public class SourceAssistantPostHandleContext
    {
        public SourceAssistantPostHandleContext(TreeList treeList)
        {
            TreeList = treeList;
        }

        public TreeList TreeList { get; set; }
    }
}
