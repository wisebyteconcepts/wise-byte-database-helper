namespace WiseByteDatabaseHelper.Core.Schema;

public class TableDefinition
{
    public string Name { get; set; } = "";
    public Type ClrType { get; set; }
    public List<ColumnDefinition> Columns { get; set; } = new();
    public List<ForeignKeyDefinition> ForeignKeys { get; set; } = new();
}