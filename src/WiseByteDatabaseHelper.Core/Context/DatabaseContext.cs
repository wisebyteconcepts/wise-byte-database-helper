using WiseByteDatabaseHelper.Core.Dialects;
using WiseByteDatabaseHelper.Core.Execution;
using WiseByteDatabaseHelper.Core.Schema;
using WiseByteDatabaseHelper.Core.Utils;

namespace WiseByteDatabaseHelper.Core.Context;

public abstract class DatabaseContext
{
    protected IDatabaseExecutor Executor { get; private set; }
    protected IDialect Dialect { get; private set; }

    private readonly List<TableDefinition> _tables = new();

    protected void UseSqlite(string file)
    {
        Dialect = new SqliteDialect();
        Executor = new SqliteExecutor(file);
    }

    protected TableBuilder Table => new(_tables);

    public async Task CreateDatabaseAsync()
    {
        var ordered = DependencySorter.Sort(_tables);

        foreach (var table in ordered)
        {
            var sql = Dialect.GenerateCreateTableSql(table);
            await Executor.ExecuteAsync(sql);
        }
    }
}
