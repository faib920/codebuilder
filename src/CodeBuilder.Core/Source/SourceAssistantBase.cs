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
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;

namespace CodeBuilder.Core.Source
{
    public abstract class SourceAssistantBase : ISourceAssistant
    {
        private Dictionary<string, Column> _matchReferences = new Dictionary<string, Column>();
        private List<string> _invalidColumns = new List<string>();

        private static List<Func<Column, Table, bool>> _tbValidators = new List<Func<Column, Table, bool>>
            {
                (c, t) => Regex.IsMatch(c.Name, $"({t.Name}|{t.Name.ToSingular()})(_id|id)$", RegexOptions.IgnoreCase)
            };
        private static List<Func<Column, Column, bool>> _pkValidators = new List<Func<Column, Column, bool>>
            {
                (c, pk) => c.Name.Equals(pk.Name, StringComparison.OrdinalIgnoreCase),
                (c, pk) => Regex.IsMatch(c.Name, $"({pk.Owner.Name}|{pk.Owner.Name.ToSingular()})(_id|id)$", RegexOptions.IgnoreCase)
            };
        private static List<Func<Column, bool>> _pkcValidators = new List<Func<Column, bool>>
            {
                c => c.Name.Equals("id", StringComparison.OrdinalIgnoreCase),
                c => Regex.IsMatch(c.Name, $"({c.Owner.Name}|{c.Owner.Name.ToSingular()})(_id|id)$", RegexOptions.IgnoreCase),
                c => c.Index == 1
            };

        public abstract string Name { get; }

        public virtual bool PreHandle(IEnumerable<Table> tables)
        {
            return true;
        }

        public abstract Task HandleAsync(IEnumerable<Table> tables, CancellationToken calcelToken);

        public virtual void PostHandle(SourceAssistantPostHandleContext context)
        {
        }

        public bool IsPrimaryKey(Column column)
        {
            return _pkcValidators.Any(s => s(column));
        }

        public Reference FindForeignKey(Column column, IEnumerable<Table> tables)
        {
            if (!MaybeForeignKey(column))
            {
                return null;
            }

            if (_matchReferences.TryGetValue(column.Name, out var pk))
            {
                var reference = new Reference(pk.Owner, pk, column.Owner, column);
                return reference;
            }

            var findtables = tables.Where(s => s != column.Owner && _tbValidators.Any(t => t(column, s)));
            if (!findtables.Any())
            {
                _invalidColumns.Add(column.Name);
                return null;
            }

            var reference1 = InternalFindReference(column, tables);
            if (reference1 != null)
            {
                return reference1;
            }

            _invalidColumns.Add(column.Name);
            return null;
        }

        private bool MaybeForeignKey(Column column)
        {
            return !column.IsPrimaryKey && Regex.IsMatch(column.Name, "(_id|id)$", RegexOptions.IgnoreCase) &&
                !_invalidColumns.Contains(column.Name);
        }

        private Reference InternalFindReference(Column column, IEnumerable<Table> tables)
        {
            foreach (var table in tables.Where(s => s.PrimaryKeys.Count > 0))
            {
                foreach (var pk in table.PrimaryKeys)
                {
                    if (_pkValidators.Any(s => s(column, pk)))
                    {
                        _matchReferences.Add(column.Name, pk);

                        return new Reference(pk.Owner, pk, column.Owner, column);
                    }
                }
            }

            return null;
        }
    }
}
