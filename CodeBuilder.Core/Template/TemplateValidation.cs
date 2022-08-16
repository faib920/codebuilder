// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CodeBuilder.Core.Source;
using CodeBuilder.Core.Variable;
using System.Collections.Generic;
using System.Linq;

namespace CodeBuilder.Core.Template
{
    public static class TemplateValidation
    {
        private static List<Table> _tables;

        public static void Validate(this IDevHosting hosting, TemplateFile template, string content)
        {
            var partition = hosting.Template.GetAllPartitions().FirstOrDefault(s => s.Name == template.Name);
            if (partition == null)
            {
                hosting.ShowError("找不到匹配的部件。");
                return;
            }

            var option = new TemplateOption();
            option.Template = hosting.Template;
            option.Partitions = new List<PartitionDefinition> { new ContentPartition(partition, content) };
            option.DynamicAssemblies.AddRange(StaticUnity.DynamicAssemblies);
            option.Profile = hosting.Profile;

            if (_tables == null)
            {
                _tables = GenerateTables().ToList();
            }

            var res = hosting.TemplateProvider.GenerateFiles(option, new List<Table> { _tables[1] }, (c, p) => { });
            if (res != null && res.Count > 0)
            {
                hosting.ShowInfo("恭喜你，模板未发现任何语法错误。");
            }
        }

        private static IEnumerable<Table> GenerateTables()
        {
            var table1 = CreateClass();
            var table2 = CreateStudent();
            var table3 = CreateLesson();

            table2.Columns.FirstOrDefault(s => s.Name == "ClassId")
                .BindForeignKey(new Reference(table1, table1.Columns.FirstOrDefault(s => s.Name == "Id"), table2, table2.Columns.FirstOrDefault(s => s.Name == "ClassId")) { Name = "ref1" });
            
            table3.Columns.FirstOrDefault(s => s.Name == "StudentId")
                .BindForeignKey(new Reference(table2, table2.Columns.FirstOrDefault(s => s.Name == "Id"), table2, table3.Columns.FirstOrDefault(s => s.Name == "StudentId")) { Name = "ref2" });

            yield return table1;
            yield return table2;
            yield return table3;
        }

        private static Table CreateClass()
        {
            var table = SchemaExtensionManager.Build<Table>();
            table.Name = "Class";
            table.ClassName = "Class";
            table.Description = "班级表";

            var column1 = SchemaExtensionManager.Build<Column>(table);
            column1.Name = "Id";
            column1.PropertyName = "Id";
            column1.Description = "Id";
            column1.IsPrimaryKey = true;
            column1.DbType = System.Data.DbType.Int32;
            column1.ColumnType = "int";
            column1.PropertyType = "int";
            table.Columns.Add(column1);

            var column2 = SchemaExtensionManager.Build<Column>(table);
            column2.Name = "Name";
            column2.PropertyName = "Name";
            column2.Description = "名称";
            column2.IsNullable = false;
            column2.Length = 20;
            column2.DbType = System.Data.DbType.String;
            column2.ColumnType = "varchar(20)";
            column2.PropertyType = "string";
            table.Columns.Add(column2);

            var column3 = SchemaExtensionManager.Build<Column>(table);
            column3.Name = "IsValid";
            column3.PropertyName = "IsValid";
            column3.Description = "是否有效";
            column3.IsNullable = false;
            column3.DbType = System.Data.DbType.Byte;
            column3.ColumnType = "tinyint";
            column3.PropertyType = "bool";
            table.Columns.Add(column3);

            var column4 = SchemaExtensionManager.Build<Column>(table);
            column4.Name = "CreateTime";
            column4.PropertyName = "CreateTime";
            column4.Description = "创建时间";
            column4.IsNullable = false;
            column4.DbType = System.Data.DbType.DateTime;
            column4.ColumnType = "date";
            column4.PropertyType = "DateTime";
            table.Columns.Add(column4);

            return table;
        }

        private static Table CreateStudent()
        {
            var table = SchemaExtensionManager.Build<Table>();
            table.Name = "Student";
            table.ClassName = "Student";
            table.Description = "学生表";

            var column1 = SchemaExtensionManager.Build<Column>(table);
            column1.Name = "Id";
            column1.PropertyName = "Id";
            column1.Description = "Id";
            column1.IsPrimaryKey = true;
            column1.DbType = System.Data.DbType.Int32;
            column1.ColumnType = "int";
            column1.PropertyType = "int";
            table.Columns.Add(column1);

            var column2 = SchemaExtensionManager.Build<Column>(table);
            column2.Name = "Name";
            column2.PropertyName = "Name";
            column2.Description = "姓名";
            column2.IsNullable = false;
            column2.Length = 20;
            column2.DbType = System.Data.DbType.String;
            column2.ColumnType = "varchar(20)";
            column2.PropertyType = "string";
            table.Columns.Add(column2);

            var column3 = SchemaExtensionManager.Build<Column>(table);
            column3.Name = "Balance";
            column3.PropertyName = "Balance";
            column3.Description = "余额";
            column3.IsNullable = true;
            column3.Precision = 12;
            column3.Scale = 2;
            column3.DbType = System.Data.DbType.Decimal;
            column3.ColumnType = "decimal(12,2)";
            column3.PropertyType = "decimal";
            table.Columns.Add(column3);

            var column4 = SchemaExtensionManager.Build<Column>(table);
            column4.Name = "ClassId";
            column4.PropertyName = "ClassId";
            column4.Description = "班级Id";
            column4.IsNullable = false;
            column4.DbType = System.Data.DbType.Int32;
            column4.ColumnType = "int";
            column4.PropertyType = "int";
            table.Columns.Add(column4);

            var column5 = SchemaExtensionManager.Build<Column>(table);
            column5.Name = "IsValid";
            column5.PropertyName = "IsValid";
            column5.Description = "是否有效";
            column5.IsNullable = false;
            column5.DbType = System.Data.DbType.Byte;
            column5.ColumnType = "tinyint";
            column5.PropertyType = "bool";
            table.Columns.Add(column5);

            var column6 = SchemaExtensionManager.Build<Column>(table);
            column6.Name = "CreateTime";
            column6.PropertyName = "CreateTime";
            column6.Description = "创建时间";
            column6.IsNullable = false;
            column6.DbType = System.Data.DbType.DateTime;
            column6.ColumnType = "date";
            column6.PropertyType = "DateTime";
            table.Columns.Add(column6);

            return table;
        }

        private static Table CreateLesson()
        {
            var table = SchemaExtensionManager.Build<Table>();
            table.Name = "Lesson";
            table.ClassName = "Lesson";
            table.Description = "课程表";

            var column1 = SchemaExtensionManager.Build<Column>(table);
            column1.Name = "Id";
            column1.PropertyName = "Id";
            column1.Description = "Id";
            column1.IsPrimaryKey = true;
            column1.DbType = System.Data.DbType.Int32;
            column1.ColumnType = "int";
            column1.PropertyType = "int";
            table.Columns.Add(column1);

            var column2 = SchemaExtensionManager.Build<Column>(table);
            column2.Name = "Name";
            column2.PropertyName = "Name";
            column2.Description = "名称";
            column2.IsNullable = false;
            column2.Length = 20;
            column2.DbType = System.Data.DbType.String;
            column2.ColumnType = "varchar(20)";
            column2.PropertyType = "string";
            table.Columns.Add(column2);

            var column3 = SchemaExtensionManager.Build<Column>(table);
            column3.Name = "StudentId";
            column3.PropertyName = "StudentId";
            column3.Description = "学生Id";
            column3.IsNullable = false;
            column3.DbType = System.Data.DbType.Int32;
            column3.ColumnType = "int";
            column3.PropertyType = "int";
            table.Columns.Add(column3);

            var column4 = SchemaExtensionManager.Build<Column>(table);
            column4.Name = "IsValid";
            column4.PropertyName = "IsValid";
            column4.Description = "是否有效";
            column4.IsNullable = false;
            column4.DbType = System.Data.DbType.Byte;
            column4.ColumnType = "tinyint";
            column4.PropertyType = "bool";
            table.Columns.Add(column4);

            var column5 = SchemaExtensionManager.Build<Column>(table);
            column5.Name = "CreateTime";
            column5.PropertyName = "CreateTime";
            column5.Description = "创建时间";
            column5.IsNullable = false;
            column5.DbType = System.Data.DbType.DateTime;
            column5.ColumnType = "date";
            column5.PropertyType = "DateTime";
            table.Columns.Add(column5);

            return table;
        }

        private class ContentPartition : PartitionDefinition
        {
            private string _content;

            public ContentPartition(PartitionDefinition definition, string content)
            {
                Name = definition.Name;
                Loop = definition.Loop;
                FileName = definition.FileName;
                FilePath = definition.FilePath;
                Syntax = definition.Syntax;

                _content = content;
            }

            public override string Content => _content;
        }
    }
}
