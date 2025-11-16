using WiseByteDatabaseHelper.Core.Schema;

namespace WiseByteDatabaseHelper.Core.Dialects;

public interface IDialect
{
    string GenerateCreateTableSql(TableDefinition table);
    string GenerateAlterTableAddColumnSql(string table, ColumnDefinition column);
    string TypeToSql(Type type, bool isPrimaryKey, bool autoIncrement, bool nonNull);
}