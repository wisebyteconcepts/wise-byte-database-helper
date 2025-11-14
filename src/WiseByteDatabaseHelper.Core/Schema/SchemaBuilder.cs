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
            if (ReflectionHelper.IsForeignKey(prop, out var fkAttr))
            {
                table.ForeignKeys.Add(new ForeignKeyDefinition
                {
                    ColumnName = prop.Name,
                    ReferenceTable = ReflectionHelper.GetTableName(fkAttr.ReferenceType),
                    ReferenceColumn = fkAttr.ReferencePrimaryKey
                });
            }

            table.Columns.Add(new ColumnDefinition
            {
                Name = prop.Name,
                Type = prop.PropertyType,
                IsPrimaryKey = ReflectionHelper.IsPrimaryKey(prop),
                AutoIncrement = ReflectionHelper.IsAutoIncrement(prop)
            });
        }

        return table;
    }
}
