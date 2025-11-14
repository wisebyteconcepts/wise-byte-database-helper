using Microsoft.Data.Sqlite;

namespace WiseByteDatabaseHelper.Core.Providers;

public class SqliteProvider : IDatabaseProvider
{
    private readonly string _connection;

    public SqliteProvider(string connectionString)
    {
        _connection = connectionString;
    }

    public async Task ExecuteAsync(string sql)
    {
        using var con = new SqliteConnection(_connection);
        await con.OpenAsync();

        using var cmd = new SqliteCommand(sql, con);
        await cmd.ExecuteNonQueryAsync();
    }
}
