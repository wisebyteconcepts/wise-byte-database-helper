using Microsoft.Data.Sqlite;

namespace WiseByteDatabaseHelper.Core.Execution;

public class SqliteExecutor : IDatabaseExecutor
{
    private readonly string _connectionString;

    public SqliteExecutor(string file)
    {
        _connectionString = $"Data Source={file}";
    }

    public async Task ExecuteAsync(string sql)
    {
        using var conn = new SqliteConnection(_connectionString);
        await conn.OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;

        await cmd.ExecuteNonQueryAsync();
    }
}
