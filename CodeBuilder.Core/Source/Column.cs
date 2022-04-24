// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CodeBuilder.Core.Designer;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
namespace CodeBuilder.Core.Source
{
    /// <summary>
    /// 字段。
    /// </summary>
    public class Column : SchemaBase, IField, INotifyPropertyChanged
    {
        private string _name;
        private string _description;
        private bool _isPrimaryKey;

        public event PropertyChangedEventHandler PropertyChanged;

        public Column()
        {
        }

        public Column(Table owner)
        {
            Owner = owner;
        }

        /// <summary>
        /// 获取或设置字段的名称。
        /// </summary>
        [Description("字段的名称。")]
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
        /// 获取或设置生成的属性名称。
        /// </summary>
        [Description("生成的属性名称。")]
        [RequiredCheck]
        public string PropertyName { get; set; }

        /// <summary>
        /// 获取或设置生成的属性的类型名称。
        /// </summary>
        [Description("生成的属性的类型名称。")]
        [RequiredCheck]
        public string PropertyType { get; set; }

        /// <summary>
        /// 获取或设置字段的描述。
        /// </summary>
        [Description("字段的描述。")]
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
        /// 获取或设置字段的DbType。
        /// </summary>
        [Description("字段的DbType。")]
        public DbType? DbType { get; set; }

        /// <summary>
        /// 获取或设置字段的数据类型。
        /// </summary>
        [Description("字段的数据类型。")]
        public string DataType { get; set; }

        /// <summary>
        /// 获取或设置字段的类型(类型+长度/精度)。
        /// </summary>
        [Description("字段的类型(类型+长度/精度)。")]
        public string ColumnType { get; set; }

        /// <summary>
        /// 获取或设置字段的长度。
        /// </summary>
        [Description("字段的长度。")]
        public long? Length { get; set; }

        /// <summary>
        /// 获取或设置字段的默认值。
        /// </summary>
        [Description("字段的默认值。")]
        public string DefaultValue { get; set; }

        /// <summary>
        /// 获取或设置数值的小数位。
        /// </summary>
        [Description("数值的小数位。")]
        public int? Scale { get; set; }

        /// <summary>
        /// 获取或设置数值的精度。
        /// </summary>
        [Description("数值的精度。")]
        public int? Precision { get; set; }

        /// <summary>
        /// 获取或设置是否自增。
        /// </summary>
        [Description("是否自增。")]
        public bool AutoIncrement { get; set; }

        /// <summary>
        /// 获取或设置字段是否可为空。
        /// </summary>
        [Description("字段是否可为空。")]
        public bool IsNullable { get; set; }

        /// <summary>
        /// 获取或设置排列的序号。
        /// </summary>
        [Description("排列的序号。")]
        [Browsable(false)]
        public int Index { get; set; }

        /// <summary>
        /// 获取或设置字段是否为主键。
        /// </summary>
        [Description("字段是否为主键。")]
        public bool IsPrimaryKey
        {
            get { return _isPrimaryKey; }
            set
            {
                if (_isPrimaryKey != value)
                {
                    if (value)
                    {
                        Owner.PrimaryKeys.Add(this);
                    }
                    else
                    {
                        Owner.PrimaryKeys.Remove(this);
                    }
                }

                _isPrimaryKey = value;
                OnPropertyChanged(nameof(IsPrimaryKey));
            }
        }

        /// <summary>
        /// 获取关联的外键。
        /// </summary>
        [Description("关联的外键。")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Editor(typeof(ForeignKeyEditor), typeof(UITypeEditor))]
        public Reference ForeignKey { get; private set; }

        /// <summary>
        /// 获取所有者。
        /// </summary>
        [DisGenerate]
        [Browsable(false)]
        public Table Owner { get; private set; }

        /// <summary>
        /// 绑定外键。
        /// </summary>
        /// <param name="foreignKey"></param>
        public virtual void BindForeignKey(Reference foreignKey)
        {
            if ((ForeignKey == null && foreignKey == null) ||
                (ForeignKey != null && foreignKey != null && ForeignKey.PkColumn == foreignKey.PkColumn))
            {
                return;
            }

            //解除
            if (ForeignKey != null)
            {
                ForeignKey.PkTable.SubKeys.Remove(ForeignKey);
                Owner.ForeignKeys.Remove(ForeignKey);
            }

            if (foreignKey != null)
            {
                foreignKey.PkTable.SubKeys.Add(foreignKey);
                Owner.ForeignKeys.Add(foreignKey);
            }

            ForeignKey = foreignKey;
            OnPropertyChanged(nameof(ForeignKey));
        }

        /// <summary>
        /// 取消绑定外键。
        /// </summary>
        public virtual void UnbindForeignKey()
        {
            ForeignKey = null;
        }

        /// <summary>
        /// 重构。
        /// </summary>
        /// <param name="table"></param>
        /// <param name="column"></param>
        public void Refactoring(Table table, Column column)
        {
            Owner = table;
            _name = column.Name;
            _description = column.Description;
            PropertyName = column.PropertyName;
            PropertyType = column.PropertyType;
            DbType = column.DbType;
            DataType = column.DataType;
            ColumnType = column.ColumnType;
            Length = column.Length;
            DefaultValue = column.DefaultValue;
            Scale = column.Scale;
            Precision = column.Precision;
            AutoIncrement = column.AutoIncrement;
            IsNullable = column.IsNullable;
            Index = column.Index;
            _isPrimaryKey = column.IsPrimaryKey;
        }

        public override string ToString()
        {
            return Name;
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
