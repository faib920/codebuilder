// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core.Designer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Xml;

namespace CodeBuilder.Core.Source
{
    /// <summary>
    /// 数据表。
    /// </summary>
    public class Table : SchemaBase, IObject, INotifyPropertyChanged, IIdentity
    {
        private string _name;
        private string _className;
        private string _description;
        private Dictionary<string, Column> _columns;

        public event PropertyChangedEventHandler PropertyChanged;

        public Table()
        {
            SubKeys = new List<Reference>();
            ForeignKeys = new List<Reference>();
            Columns = new List<Column>();
            PrimaryKeys = new List<Column>();
            UniqueKeys = new List<Column>();
            Indexes = new List<Index>();
        }

        public Table(bool isView)
            : this()
        {
            IsView = isView;
        }

        [Description("原始名称。")]
        [Category(CategoryConsts.Attribute)]
        public string _Name { get; internal set; }

        /// <summary>
        /// 获取或设置表的名称。
        /// </summary>
        [Description("数据表的名称。")]
        [Category(CategoryConsts.Attribute)]
        [RequiredCheck]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                if (string.IsNullOrEmpty(_Name))
                {
                    _Name = value;
                }

                OnPropertyChanged(nameof(Name));
            }
        }

        /// <summary>
        /// 获取或设置生成的类名称。
        /// </summary>
        [Description("生成的类的名称。")]
        [Category(CategoryConsts.Attribute)]
        public string ClassName
        {
            get { return _className; }
            set
            {
                _className = value;
                OnPropertyChanged(nameof(ClassName));
            }
        }

        /// <summary>
        /// 获取或设置表的描述。
        /// </summary>
        [Description("数据表的描述、备注。")]
        [Category(CategoryConsts.Attribute)]
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
        [Description("排列的序号(生成前会自动排序)。")]
        [Category(CategoryConsts.Attribute)]
        public int Index { get; set; }

        /// <summary>
        /// 数据表的架构。
        /// </summary>
        [Description("数据表的架构。")]
        [Category(CategoryConsts.Attribute)]
        public string Schema { get; set; }

        /// <summary>
        /// 获取外键集合。
        /// </summary>
        [Description("外键集合。")]
        [Category(CategoryConsts.Auxiliary)]
        [TypeConverter(typeof(Designer.CollectionConverter))]
        [Editor(typeof(Designer.ForeignKeyViewEditor), typeof(UITypeEditor))]
        [UnPersistently]
        public List<Reference> ForeignKeys { get; private set; }

        /// <summary>
        /// 获取子键集合。
        /// </summary>
        [Description("子键集合。")]
        [Category(CategoryConsts.Auxiliary)]
        [TypeConverter(typeof(Designer.CollectionConverter))]
        [Editor(typeof(Designer.SubKeyViewEditor), typeof(UITypeEditor))]
        [UnPersistently]
        public List<Reference> SubKeys { get; private set; }

        /// <summary>
        /// 获取主键集合。
        /// </summary>
        [Description("主键集合。")]
        [Category(CategoryConsts.Auxiliary)]
        [TypeConverter(typeof(Designer.CollectionConverter))]
        [Editor(typeof(Designer.CollectionViewEditor), typeof(UITypeEditor))]
        [UnPersistently]
        public List<Column> PrimaryKeys { get; private set; }

        /// <summary>
        /// 获取字段集合。
        /// </summary>
        [Description("字段集合。")]
        [Category(CategoryConsts.Auxiliary)]
        [TypeConverter(typeof(Designer.CollectionConverter))]
        [Editor(typeof(Designer.CollectionViewEditor), typeof(UITypeEditor))]
        [UnPersistently]
        public List<Column> Columns { get; private set; }

        /// <summary>
        /// 唯一键集合。
        /// </summary>
        [Description("唯一键集合。")]
        [Category(CategoryConsts.Auxiliary)]
        [TypeConverter(typeof(Designer.CollectionConverter))]
        [Editor(typeof(Designer.CollectionViewEditor), typeof(UITypeEditor))]
        [UnPersistently]
        public List<Column> UniqueKeys { get; private set; }

        /// <summary>
        /// 索引集合。
        /// </summary>
        [Description("索引集合。")]
        [Category(CategoryConsts.Auxiliary)]
        [TypeConverter(typeof(Designer.CollectionConverter))]
        [Editor(typeof(Designer.IndexViewEditor), typeof(UITypeEditor))]
        [UnPersistently]
        public List<Index> Indexes { get; private set; }

        List<IField> IObject.Fields
        {
            get
            {
                return Columns.Cast<IField>().ToList();
            }
        }

        [Category(CategoryConsts.Auxiliary)]
        [TypeConverter(typeof(Designer.HostConvert))]
        [UnPersistently]
        [Browsable(false)]
        [DisGenerate]
        public Host Host { get; internal set; }

        /// <summary>
        /// 是否是视图。
        /// </summary>
        [Description("是否视图。")]
        [Category(CategoryConsts.Attribute)]
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
        public void Refactoring(Table table, Func<Table, Column> columnFactory)
        {
            _name = table.Name;
            _Name = table._Name;
            _description = table.Description;
            ClassName = table.ClassName;
            IsView = table.IsView;
            Index = table.Index;
            Schema = table.Schema;
            Indexes = table.Indexes;
            Host = table.Host;

            foreach (var column in table.Columns)
            {
                var newColumn = columnFactory(this);
                newColumn.Refactoring(this, column);
                Columns.Add(newColumn);
            }

            foreach (var pk in table.PrimaryKeys)
            {
                var newPk = FindColumn(pk._Name);
                if (newPk != null)
                {
                    PrimaryKeys.Add(newPk);
                }
            }
        }

        public Column FindColumn(string name)
        {
            if (_columns?.Count != Columns.Count)
            {
                _columns = Columns.ToDictionary(s => s._Name);
            }

            _columns.TryGetValue(name, out var column);
            return column;
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

        internal void SetIsView()
        {
            IsView = true;
        }
    }
}
