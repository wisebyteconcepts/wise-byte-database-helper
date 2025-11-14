namespace WiseByteDatabaseHelper.Core.Schema;

public class EntityDefinition
{
    public string TableName { get; set; } = string.Empty;
    public Type EntityType { get; set; } = null!;
    public List<ColumnDefinition> Columns { get; set; } = new();
    public List<ForeignKeyDefinition> ForeignKeys { get; set; } = new();
}