using System.Reflection;

using WiseByteDatabaseHelper.Core.Attributes;

namespace WiseByteDatabaseHelper.Core.Utils;

public static class ReflectionHelper
{
    public static TableAttribute GetTableAttribute(Type type)
        => type.GetCustomAttributes(typeof(TableAttribute), false).First() as TableAttribute
           ?? throw new Exception($"Class {type.Name} must have [Table] attribute.");

    public static string GetTableName(Type type)
        => GetTableAttribute(type).Name;

    public static bool IsPrimaryKey(PropertyInfo p)
        => Attribute.IsDefined(p, typeof(PrimaryKeyAttribute));

    public static bool IsAutoIncrement(PropertyInfo p)
        => Attribute.IsDefined(p, typeof(AutoIncrementAttribute));

    public static bool IsForeignKey(PropertyInfo p, out ForeignKeyAttribute attr)
    {
        attr = p.GetCustomAttribute<ForeignKeyAttribute>();
        return attr != null;
    }
}
