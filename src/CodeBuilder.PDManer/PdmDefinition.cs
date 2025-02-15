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
using CodeBuilder.Core.Variable;
using Fireasy.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CodeBuilder.PDManer
{
    public class PdmAbstract
    {
        public string Id { get; set; }

        public string DefKey { get; set; }

        public string DefName { get; set; }
    }

    public class PdmDefinition
    {
        public List<PdMDomain> Domains { get; set; }

        public List<PdmViewGroup> ViewGroups { get; set; }

        public List<PdmEntity> Entities { get; set; }

        public PdmDataTypeMapping DataTypeMapping { get; set; }

        public PdmProfile Profile { get; set; }


        public Table ConvertToTable(IServiceProvider serviceProvider, PdmEntity entity)
        {
            var schemaExtManager = serviceProvider.TryGetService<ISchemaExtensionManager>();
            var newtable = schemaExtManager.Build<Table>();

            newtable.Name = entity.DefKey;
            newtable.Description = entity.DefName;

            foreach (var field in entity.Fields)
            {
                var column = schemaExtManager.Build<Column>(newtable);
                column.Name = field.DefKey;
                column.Description = field.DefName;
                column.IsPrimaryKey = field.PrimaryKey;
                column.AutoIncrement = field.AutoIncrement;
                column.IsNullable = !field.NotNull;
                column.DefaultValue = field.DefaultValue;

                if (field.Scale == null)
                {
                    column.Length = field.Len;
                }
                else
                {
                    column.Precision = field.Len;
                }

                column.Scale = field.Scale;
                column.DataType = field.Type;

                if (!string.IsNullOrWhiteSpace(field.Domain))
                {
                    var domain = Domains.FirstOrDefault(s => s.Id == field.Domain);
                    if (domain != null)
                    {
                        if (!string.IsNullOrWhiteSpace(domain.ApplyFor))
                        {
                            var dataTypes = DataTypeMapping.Mappings.FirstOrDefault(s => s["id"] == domain.ApplyFor);
                            if (dataTypes != null)
                            {
                                column.DataType = dataTypes[Profile.Default.DB];
                                column.DbType = DbTypeManager.GetDbType(dataTypes["defKey"]);
                            }
                        }
                    }
                }

                if (column.DbType == null)
                {
                    var dataTypes = DataTypeMapping.Mappings.FirstOrDefault(s => s[Profile.Default.DB] == field.Type);
                    if (dataTypes != null)
                    {
                        column.DbType = DbTypeManager.GetDbType(dataTypes["defKey"]);
                    }
                }

                if (field.Len != null && field.Scale != null)
                {
                    column.ColumnType = column.DataType + "(" + field.Len + ", " + field.Scale + ")";
                }
                else if (field.Len > 0)
                {
                    column.ColumnType = column.DataType + "(" + field.Len + ")";
                }
                else
                {
                    column.ColumnType = column.DataType;
                }

                newtable.Columns.Add(column);
            }

            foreach (var index in entity.Indexes)
            {
                var idx = new Index(index.DefKey);
                idx.IsUniqueKey = index.Unique;

                foreach (var field in index.Fields)
                {
                    Column column = null;
                    var obj = entity.Fields.FirstOrDefault(s => s.Id == field.FieldDefKey);
                    if (obj != null && (column = newtable.FindColumn(obj.DefKey)) != null)
                    {
                        column.IsUniqueKey = idx.IsUniqueKey;
                        var idxc = idx.AddColumn(column);
                        idxc.SortOrder = field.AscOrDesc == "D" ? "DESC" : (field.AscOrDesc == "A" ? "ASC" : string.Empty);
                    }
                }

                newtable.Indexes.Add(idx);
            }

            return newtable;
        }
    }

    public class PdmDataTypeMapping
    {
        public List<Dictionary<string, string>> Mappings { get; set; }
    }

    public class PdMDomain : PdmAbstract
    {
        public string ApplyFor { get; set; }

        public int? Len { get; set; }

        public int? Scale { get; set; }
    }

    public class PdmViewGroup : PdmAbstract
    {
        public List<PdmEntity> Entities { get; set; }

        public List<string> RefEntities { get; set; }
    }

    public class PdmEntity : PdmAbstract
    {
        public string Comment { get; set; }

        public List<PdmField> Fields { get; set; }

        public List<PdmIndex> Indexes { get; set; }

        public List<PdmRelation> Correlations { get; set; }
    }

    public class PdmField : PdmAbstract
    {
        public string Comment { get; set; }

        public bool PrimaryKey { get; set; }

        public bool NotNull { get; set; }

        public bool AutoIncrement { get; set; }

        public string DefaultValue { get; set; }

        public string Domain { get; set; }

        public string Type { get; set; }

        public int? Len { get; set; }

        public int? Scale { get; set; }
    }

    public class PdmRelation
    {
        public string MyField { get; set; }

        public string RefEntity { get; set; }

        public string RefField { get; set; }

        public string MyRows { get; set; }

        public string RefRows { get; set; }
    }

    public class PdmProfile
    {
        public List<PdmDataTypeSupport> DataTypeSupports { get; set; }

        public PdmDbProfile Default { get; set; }
    }

    public class PdmDbProfile
    {
        public string DB { get; set; }
    }

    public class PdmDataTypeSupport : PdmAbstract
    {
    }

    public class PdmIndex : PdmAbstract
    {
        public bool Unique { get; set; }

        public List<PdmIndexField> Fields { get; set; }
    }

    public class PdmIndexField
    {
        public string FieldDefKey { get; set; }

        public string AscOrDesc { get; set; }
    }
}
