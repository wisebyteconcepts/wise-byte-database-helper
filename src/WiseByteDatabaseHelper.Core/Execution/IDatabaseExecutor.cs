namespace WiseByteDatabaseHelper.Core.Execution;

public interface IDatabaseExecutor
{
    Task ExecuteAsync(string sql);
}
