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

    public static bool IsForeignKey(PropertyInfo p, out ForeignKeyAttribute? attr)
    {
        attr = p.GetCustomAttribute<ForeignKeyAttribute>();
        return attr != null;
    }
    public static bool IsColumn(PropertyInfo p, out ColumnAttribute? attr)
    {
        attr = p.GetCustomAttribute<ColumnAttribute>();
        return attr != null;
    }

    public static string GetColumnName(PropertyInfo property)
    {
        var attr = property.GetCustomAttribute<ColumnAttribute>();

        // If attribute exists but Name is null → fallback to property name
        if (attr is not null)
            return attr.Name ?? property.Name;

        // If no attribute, you decide: maybe return property.Name or skip
        return property.Name;
    }


    public static bool IsDatabaseField(PropertyInfo prop)
    {
        return IsPrimaryKey(prop)
            || IsAutoIncrement(prop)
            || IsForeignKey(prop, out _)
            || IsColumn(prop, out _);
    }

    public static PropertyInfo GetForeignKeyReferenceProperty(ForeignKeyAttribute fkAttr)
    {
        return fkAttr.ReferenceType.GetProperty(fkAttr.ReferencePrimaryKey)
               ?? throw new Exception(
                   $"Property '{fkAttr.ReferencePrimaryKey}' not found in '{fkAttr.ReferenceType.Name}'.");
    }

    internal static bool IsNotNull(PropertyInfo prop)
    {
        var context = new NullabilityInfoContext();
        var nullability = context.Create(prop);

        return nullability.WriteState == NullabilityState.Nullable
            || nullability.ReadState == NullabilityState.Nullable;

    }
}
