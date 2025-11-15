using System.Linq.Expressions;
using System.Reflection;

namespace WiseByteDatabaseHelper.Core.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class ForeignKeyAttribute : Attribute
{
    public Type ReferenceType { get; }
    public string ReferencePrimaryKey { get; }

    public ForeignKeyAttribute(Type referenceType, string referencePrimaryKey)
    {
        ReferenceType = referenceType;
        ReferencePrimaryKey = referencePrimaryKey;
    }


}