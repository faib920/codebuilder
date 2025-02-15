// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using CodeBuilder.Core.Source;
using Fireasy.Common.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CodeBuilder
{
    public class PrimaryKeyAssistant : SourceAssistantBase, ISourceAssistant, ITransientService
    {
        private int _count;
        private readonly IDevHosting _hosting;

        public PrimaryKeyAssistant(IDevHosting hosting)
        {
            _hosting = hosting;
        }

        public override string Name => "自动创建主键";

        public override async Task HandleAsync(IEnumerable<Table> tables, CancellationToken calcelToken)
        {
            var columns = new List<Column>();
            _count = 0;

            foreach (var table in tables)
            {
                if (calcelToken.IsCancellationRequested)
                {
                    return;
                }

                foreach (var column in table.Columns.Where(s => !s.IsPrimaryKey))
                {
                    if (calcelToken.IsCancellationRequested)
                    {
                        return;
                    }

                    if (IsPrimaryKey(column))
                    {
                        columns.Add(column);
                    }
                }
            }

            foreach (var column in columns)
            {
                column.IsPrimaryKey = true;
                _count++;
            }
        }

        public override void PostHandle(SourceAssistantPostHandleContext context)
        {
            _hosting.ShowInfo($"一共发现并自动创建了 {_count} 个主键。");
        }
    }

    public class ForeignKeyAssistant : SourceAssistantBase, ISourceAssistant, ITransientService
    {
        private int _count;
        private readonly IDevHosting _hosting;

        public ForeignKeyAssistant(IDevHosting hosting)
        {
            _hosting = hosting;
        }

        public override string Name => "自动创建外键";

        public override async Task HandleAsync(IEnumerable<Table> tables, CancellationToken calcelToken)
        {
            var references = new Dictionary<Column, Reference>();
            var index = 0;
            _count = 0;
            var total = tables.Count();

            foreach (var table in tables)
            {
                if (calcelToken.IsCancellationRequested)
                {
                    return;
                }

                var p = (int)((++index / (total * 1.0)) * 100);
                _hosting.ShowProgress($"{p}% 正在检索表 {table.Name}...", p);

                foreach (var column in table.Columns)
                {
                    if (calcelToken.IsCancellationRequested)
                    {
                        return;
                    }

                    var reference = FindForeignKey(column, tables);
                    if (reference != null)
                    {
                        references.Add(column, reference);
                    }
                }
            }

            foreach (var kvp in references)
            {
                if (!kvp.Key.BindForeignKey(kvp.Value))
                {
                    continue;
                }
                _count++;
            }
        }

        public override void PostHandle(SourceAssistantPostHandleContext context)
        {
            _hosting.ShowInfo($"一共发现并自动创建了 {_count} 个外键。");
        }
    }

    public class ClearForeignKeyAssistant : SourceAssistantBase, ISourceAssistant, ITransientService
    {
        private int _count;
        private readonly IDevHosting _hosting;

        public ClearForeignKeyAssistant(IDevHosting hosting)
        {
            _hosting = hosting;
        }

        public override string Name => "清除所有外键";

        public override bool PreHandle(IEnumerable<Table> tables)
        {
            return _hosting.ShowConfirm("是否清除所有外键?") == ShowMsgButton.Yes;
        }

        public override async Task HandleAsync(IEnumerable<Table> tables, CancellationToken calcelToken)
        {
            _count = 0;

            foreach (var table in tables)
            {
                foreach (var column in table.Columns)
                {
                    if (column.ForeignKey != null)
                    {
                        column.UnbindForeignKey();
                        _count++;
                    }
                }
            }
        }

        public override void PostHandle(SourceAssistantPostHandleContext context)
        {
            _hosting.ShowInfo("所有外键关系已被清理，你可以使用【创建外键关系】来重新生成。");
        }
    }
}
