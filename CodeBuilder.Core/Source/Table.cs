// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CodeBuilder.Core.Source
{
    /// <summary>
    /// 数据表。
    /// </summary>
    public class Table : SchemaBase, IObject, INotifyPropertyChanged
    {
        private string _name;
        private string _description;

        public event PropertyChangedEventHandler PropertyChanged;

        public Table()
        {
            SubKeys = new List<Reference>();
            ForeignKeys = new List<Reference>();
            Columns = new List<Column>();
            PrimaryKeys = new List<Column>();
        }

        public Table(bool isView)
            : this()
        {
            IsView = isView;
        }

        /// <summary>
        /// 获取或设置表的名称。
        /// </summary>
        [Description("表的名称。")]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        /// <summary>
        /// 获取或设置生成的类名称。
        /// </summary>
        [Description("生成的类名称。")]
        [RequiredCheck]
        public string ClassName { get; set; }

        /// <summary>
        /// 获取或设置表的描述。
        /// </summary>
        [Description("表的描述。")]
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        /// <summary>
        /// 获取或设置排列的序号。
        /// </summary>
        [Description("排列的序号。")]
        [Browsable(false)]
        public int Index { get; set; }

        /// <summary>
        /// 获取外键集合。
        /// </summary>
        [Browsable(false)]
        public List<Reference> ForeignKeys { get; private set; }

        /// <summary>
        /// 获取子键集合。
        /// </summary>
        [Browsable(false)]
        public List<Reference> SubKeys { get; private set; }

        /// <summary>
        /// 获取主键集合。
        /// </summary>
        [Browsable(false)]
        public List<Column> PrimaryKeys { get; private set; }

        /// <summary>
        /// 获取字段集合。
        /// </summary>
        [Browsable(false)]
        public List<Column> Columns { get; private set; }

        List<IField> IObject.Fields
        {
            get
            {
                return Columns.Cast<IField>().ToList();
            }
        }

        [DisGenerate]
        [Browsable(false)]
        public Host Host { get; set; }

        /// <summary>
        /// 获取或设置是否是视图。
        /// </summary>
        [DisGenerate]
        public bool IsView { get; private set; }

        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// 克隆一个数据表。
        /// </summary>
        /// <returns></returns>
        public Table Clone()
        {
            return (Table)MemberwiseClone();
        }

        /// <summary>
        /// 重构。
        /// </summary>
        /// <param name="table"></param>
        /// <param name="columnFactory"></param>
        public void Refactoring(Table table, Func<Column> columnFactory)
        {
            _name = table.Name;
            _description = table.Description;
            ClassName = table.ClassName;
            IsView = table.IsView;
            Index = table.Index;
            Host = table.Host;

            foreach (var column in table.Columns)
            {
                var newColumn = columnFactory();
                newColumn.Refactoring(this, column);
                Columns.Add(newColumn);
            }

            foreach (var pk in table.PrimaryKeys)
            {
                var newPk = Columns.FirstOrDefault(s => s.Name == pk.Name);
                if (newPk != null)
                {
                    PrimaryKeys.Add(newPk);
                }
            }
        }

        /// <summary>
        /// 属性就更时通知外部应用。
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
