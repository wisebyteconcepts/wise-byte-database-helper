using WiseByteDatabaseHelper.Core.Schema;

namespace WiseByteDatabaseHelper.Core.Dialects;

public class SqliteDialect : IDialect
{
    public string TypeToSql(Type type, bool isPrimaryKey, bool autoIncrement, bool notNull)
    {
        if (isPrimaryKey && autoIncrement)
            return "INTEGER";

        // Nullable<T> unwrap
        var underlying = Nullable.GetUnderlyingType(type);
        if (underlying != null) type = underlying;

        if (type == typeof(int)) return "INTEGER";
        if (type == typeof(long)) return "INTEGER";
        if (type == typeof(short)) return "INTEGER";
        if (type == typeof(byte)) return "INTEGER";
        if (type == typeof(bool)) return "INTEGER";
        if (type == typeof(char)) return "INTEGER";

        if (type.IsEnum) return "INTEGER";

        if (type == typeof(float)) return "REAL";
        if (type == typeof(double)) return "REAL";
        if (type == typeof(decimal)) return "REAL"; // or TEXT for precision

        if (type == typeof(string)) return "TEXT";
        if (type == typeof(Guid)) return "TEXT";
        if (type == typeof(DateTime)) return "TEXT"; // recommended: ISO8601
        if (type == typeof(DateTimeOffset)) return "TEXT";
        if (type == typeof(TimeSpan)) return "TEXT";

        if (type == typeof(byte[])) return "BLOB";

        throw new NotSupportedException($"Unsupported type: {type}");
    }

    public string GenerateCreateTableSql(TableDefinition table)
    {
        var columns = string.Join(", ", table.Columns.Select(c => c.ToSql(this)));

        var fks = string.Join(", ",
            table.ForeignKeys.Select(f =>
                $"FOREIGN KEY ({f.ColumnName}) REFERENCES {f.ReferenceTable}({f.ReferenceColumn})"));

        var constraints = string.IsNullOrWhiteSpace(fks) ? "" : $", {fks}";

        return $"CREATE TABLE IF NOT EXISTS {table.Name} ({columns}{constraints});";
    }

    public string GenerateAlterTableAddColumnSql(string table, ColumnDefinition c)
    {
        return $"ALTER TABLE {table} ADD COLUMN {c.ToSql(this)};";
    }
}
