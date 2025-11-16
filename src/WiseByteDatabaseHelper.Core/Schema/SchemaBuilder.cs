using System.Reflection;

using WiseByteDatabaseHelper.Core.Attributes;
using WiseByteDatabaseHelper.Core.Utils;

namespace WiseByteDatabaseHelper.Core.Schema;

public static class SchemaBuilder
{
    public static TableDefinition BuildTable<T>()
    {
        var type = typeof(T);
        var tableAttr = ReflectionHelper.GetTableAttribute(type);

        var table = new TableDefinition
        {
            Name = tableAttr.Name,
            ClrType = type
        };

        foreach (var prop in type.GetProperties())
        {
            if (!ReflectionHelper.IsDatabaseField(prop))
                continue;

            // Resolve fk if present
            ReflectionHelper.IsForeignKey(prop, out var fkAttr);

            // Resolve column attribute (may be missing)
            ReflectionHelper.IsColumn(prop, out var clmAttr);

            // Generate final column name
            var columnName = clmAttr?.Name ?? prop.Name;

            table.Columns.Add(new ColumnDefinition
            {
                Name = columnName,
                Type = prop.PropertyType,
                IsPrimaryKey = ReflectionHelper.IsPrimaryKey(prop),
                AutoIncrement = ReflectionHelper.IsAutoIncrement(prop),
                IsNull = ReflectionHelper.IsNotNull(prop),
            });

            // Foreign key gets added to foreign key list
            if (fkAttr is not null)
            {
                table.ForeignKeys.Add(new ForeignKeyDefinition
                {
                    ColumnName = columnName,
                    ReferenceTable = ReflectionHelper.GetTableName(fkAttr.ReferenceType),
                    ReferenceColumn = ReflectionHelper.GetColumnName(ReflectionHelper.GetForeignKeyReferenceProperty(fkAttr))
                });
            }
        }

        return table;
    }
}
