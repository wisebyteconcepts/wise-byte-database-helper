using WiseByteDatabaseHelper.Core.Dialects;

namespace WiseByteDatabaseHelper.Core.Schema;

public class ColumnDefinition
{
    public string Name { get; set; } = "";
    public Type Type { get; set; } = typeof(object);
    public bool IsPrimaryKey { get; set; }
    public bool AutoIncrement { get; set; }
    public bool IsNull { get; set; }

    public string ToSql(IDialect dialect)
    {
        var typeSql = dialect.TypeToSql(Type, IsPrimaryKey, AutoIncrement, IsNull);

        var nn = !IsNull ? " NOT NULL" : "";
        var pk = IsPrimaryKey ? " PRIMARY KEY" : "";
        var ai = AutoIncrement ? " AUTOINCREMENT" : "";

        return $"{Name} {typeSql}{nn}{pk}{ai}";
    }
}
