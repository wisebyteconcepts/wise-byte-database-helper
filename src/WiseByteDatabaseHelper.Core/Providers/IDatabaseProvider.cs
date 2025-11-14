namespace WiseByteDatabaseHelper.Core.Providers;

public interface IDatabaseProvider
{
    Task ExecuteAsync(string sql);
}
