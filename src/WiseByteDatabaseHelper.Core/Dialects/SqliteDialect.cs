using WiseByteDatabaseHelper.Core.Schema;

namespace WiseByteDatabaseHelper.Core.Dialects;

public class SqliteDialect : IDialect
{
    public string TypeToSql(Type type, bool isPrimaryKey, bool autoIncrement, bool notNull)
    {
        if (isPrimaryKey && autoIncrement)
            return "INTEGER";

        if (type == typeof(int)) return "INTEGER";
        if (type == typeof(long)) return "INTEGER";
        if (type == typeof(string)) return "TEXT";
        if (type == typeof(bool)) return "INTEGER";
        if (type == typeof(DateTime)) return "TEXT";

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
