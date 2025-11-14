using WiseByteDatabaseHelper.Core.Schema;

namespace WiseByteDatabaseHelper.Core.Context;

public class TableBuilder
{
    private readonly List<TableDefinition> _tables;

    public TableBuilder(List<TableDefinition> tables)
    {
        _tables = tables;
    }

    public void Create<T>()
    {
        var table = SchemaBuilder.BuildTable<T>();
        _tables.Add(table);
    }

    public void Update<T>()
    {
        // Step 1: Build definition again
        var newTable = SchemaBuilder.BuildTable<T>();

        // Step 2: Compare existing table
        // Step 3: Check FK breakages
        // Step 4: Issue ALTER TABLE ADD COLUMN commands

        throw new NotImplementedException("Update logic coming soon.");
    }
}
