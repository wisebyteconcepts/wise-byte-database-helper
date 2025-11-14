namespace WiseByteDatabaseHelper.Core.Schema;

public class ForeignKeyDefinition
{
    public string ColumnName { get; set; } = "";
    public string ReferenceTable { get; set; } = "";
    public string ReferenceColumn { get; set; } = "";
}
