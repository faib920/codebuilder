// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
//using CodeBuilder.Core.Designer;
using CodeBuilder.Core.Designer;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
namespace CodeBuilder.Core.Source
{
    /// <summary>
    /// 字段。
    /// </summary>
    public class Column : SchemaBase, IField, INotifyPropertyChanged, IIdentity, IModeView
    {
        private string _name;
        private string _propertyName;
        private string _propertyType;
        private string _description;
        private DbType? _dbType;
        private string _dataType;
        private string _columnType;
        private long? _length;
        private string _defaultValue;
        private int? _scale;
        private int? _precision;
        private bool _autoIncrement;
        private bool _isNullable;
        private bool _isPrimaryKey;
        private bool _isUniqueKey;

        public event PropertyChangedEventHandler PropertyChanged;

        public Column()
        {
        }

        public Column(Table owner)
        {
            Owner = owner;
        }

        [Description("原始名称。")]
        [Category(CategoryConsts.Attribute)]
        public string _Name { get; internal set; }

        /// <summary>
        /// 获取或设置字段的名称。
        /// </summary>
        [Description("数据字段的名称。")]
        [Category(CategoryConsts.Attribute)]
        [UICustomized("名称", 340)]
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
        /// 获取或设置生成的属性名称。
        /// </summary>
        [Description("生成的属性名称。")]
        [Category(CategoryConsts.Attribute)]
        [UICustomized("属性名称", 200)]
        public string PropertyName
        {
            get { return _propertyName; }
            set
            {
                _propertyName = value;
                OnPropertyChanged(nameof(PropertyName));
            }
        }

        /// <summary>
        /// 获取或设置生成的属性的类型名称。
        /// </summary>
        [Description("生成的属性的类型名称。")]
        [Category(CategoryConsts.Attribute)]
        [UICustomized("属性类型", 100)]
        public string PropertyType
        {
            get { return _propertyType; }
            set
            {
                _propertyType = value;
                OnPropertyChanged(nameof(PropertyType));
            }
        }

        /// <summary>
        /// 获取或设置字段的描述。
        /// </summary>
        [Description("字段的描述。")]
        [Category(CategoryConsts.Attribute)]
        [UICustomized("描述", 300)]
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
        [Category(CategoryConsts.Attribute)]
        [UICustomized("DbType", 100)]
        public DbType? DbType
        {
            get { return _dbType; }
            set
            {
                _dbType = value;
                OnPropertyChanged(nameof(DbType));
            }
        }

        /// <summary>
        /// 获取或设置字段的数据类型。
        /// </summary>
        [Description("字段的数据类型。")]
        [Category(CategoryConsts.Attribute)]
        [UICustomized("数据类型", 100)]
        public string DataType
        {
            get { return _dataType; }
            set
            {
                _dataType = value;
                OnPropertyChanged(nameof(DataType));
            }
        }

        /// <summary>
        /// 获取或设置字段的类型(类型+长度/精度)。
        /// </summary>
        [Description("字段的类型(类型+长度/精度)。")]
        [Category(CategoryConsts.Attribute)]
        [UICustomized("字段类型", 100)]
        public string ColumnType
        {
            get { return _columnType; }
            set
            {
                _columnType = value;
                OnPropertyChanged(nameof(ColumnType));
            }
        }

        /// <summary>
        /// 获取或设置字段的长度。
        /// </summary>
        [Description("字段的长度。")]
        [Category(CategoryConsts.Attribute)]
        [UICustomized("字段长度", 80)]
        public long? Length
        {
            get { return _length; }
            set
            {
                _length = value;
                OnPropertyChanged(nameof(Length));
            }
        }

        /// <summary>
        /// 获取或设置字段的默认值。
        /// </summary>
        [Description("字段的默认值。")]
        [Category(CategoryConsts.Attribute)]
        [UICustomized("默认值", 80)]
        public string DefaultValue
        {
            get { return _defaultValue; }
            set
            {
                _defaultValue = value;
                OnPropertyChanged(nameof(DefaultValue));
            }
        }

        /// <summary>
        /// 获取或设置数值的小数位。
        /// </summary>
        [Description("数值的小数位。")]
        [Category(CategoryConsts.Attribute)]
        [UICustomized("数值小数位", 60)]
        public int? Scale
        {
            get { return _scale; }
            set
            {
                _scale = value;
                OnPropertyChanged(nameof(Scale));
            }
        }

        /// <summary>
        /// 获取或设置数值的精度。
        /// </summary>
        [Description("数值的精度。")]
        [Category(CategoryConsts.Attribute)]
        [UICustomized("数值精度", 60)]
        public int? Precision
        {
            get { return _precision; }
            set
            {
                _precision = value;
                OnPropertyChanged(nameof(Precision));
            }
        }

        /// <summary>
        /// 获取或设置是否自增。
        /// </summary>
        [Description("是否自增。")]
        [Category(CategoryConsts.Attribute)]
        [UICustomized("是否自增", 80)]
        public bool AutoIncrement
        {
            get { return _autoIncrement; }
            set
            {
                _autoIncrement = value;
                OnPropertyChanged(nameof(AutoIncrement));
            }
        }

        /// <summary>
        /// 获取或设置字段是否可为空。
        /// </summary>
        [Description("字段是否可为空。")]
        [Category(CategoryConsts.Attribute)]
        [UICustomized("是否可空", 80)]
        public bool IsNullable
        {
            get { return _isNullable; }
            set
            {
                _isNullable = value;
                OnPropertyChanged(nameof(IsNullable));
            }
        }

        /// <summary>
        /// 获取或设置排列的序号。
        /// </summary>
        [Description("排列的序号(生成前会自动排序)。")]
        [Category(CategoryConsts.Attribute)]
        public int Index { get; set; }

        /// <summary>
        /// 获取或设置字段是否为主键。
        /// </summary>
        [Description("字段是否为主键。")]
        [Category(CategoryConsts.Attribute)]
        [UICustomized("是否主键", 100)]
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
        /// 获取或设置字段是否为唯一键。
        /// </summary>
        [Description("字段是否为唯一键。")]
        [Category(CategoryConsts.Attribute)]
        [UICustomized("是否唯一键", 100)]
        public bool IsUniqueKey
        {
            get { return _isUniqueKey; }
            set
            {
                if (_isUniqueKey != value)
                {
                    if (value)
                    {
                        Owner.UniqueKeys.Add(this);
                    }
                    else
                    {
                        Owner.UniqueKeys.Remove(this);
                    }
                }

                _isUniqueKey = value;
                OnPropertyChanged(nameof(IsUniqueKey));
            }
        }

        /// <summary>
        /// 获取关联的外键。
        /// </summary>
        [Description("关联的外键。")]
        [Category(CategoryConsts.Attribute)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Editor(typeof(ForeignKeyEditor), typeof(UITypeEditor))]
        [UnPersistently]
        public Reference ForeignKey { get; private set; }

        /// <summary>
        /// 获取字段的所有者。
        /// </summary>
        [Description("字段的所有者。")]
        [TypeConverter(typeof(ColumnOwnerConvert))]
        [Category(CategoryConsts.Auxiliary)]
        [UnPersistently]
        public Table Owner { get; private set; }

        /// <summary>
        /// 绑定外键。
        /// </summary>
        /// <param name="foreignKey"></param>
        /// <returns></returns>
        public virtual bool BindForeignKey(Reference foreignKey)
        {
            if ((ForeignKey == null && foreignKey == null) ||
                (ForeignKey != null && foreignKey != null && ForeignKey.PkColumn == foreignKey.PkColumn))
            {
                return false;
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

            return true;
        }

        /// <summary>
        /// 取消绑定外键。
        /// </summary>
        public virtual void UnbindForeignKey()
        {
            //解除
            if (ForeignKey != null)
            {
                ForeignKey.PkTable.SubKeys.Remove(ForeignKey);
                Owner.ForeignKeys.Remove(ForeignKey);
            }

            ForeignKey = null;
            OnPropertyChanged(nameof(ForeignKey));
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
            _Name = column._Name;
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
            _isUniqueKey = column.IsUniqueKey;
        }

        public override string ToString()
        {
            return Name;
        }

        string IModeView.GetDisplayName(string view)
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
