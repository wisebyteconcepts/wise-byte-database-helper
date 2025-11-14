using WiseByteDatabaseHelper.Core.Dialects;

namespace WiseByteDatabaseHelper.Core.Schema;

public class ColumnDefinition
{
    public string Name { get; set; } = "";
    public Type Type { get; set; }
    public bool IsPrimaryKey { get; set; }
    public bool AutoIncrement { get; set; }

    public string ToSql(IDialect dialect)
    {
        var typeSql = dialect.TypeToSql(Type, IsPrimaryKey, AutoIncrement);

        var pk = IsPrimaryKey ? " PRIMARY KEY" : "";
        var ai = AutoIncrement ? " AUTOINCREMENT" : "";

        return $"{Name} {typeSql}{pk}{ai}";
    }
}
