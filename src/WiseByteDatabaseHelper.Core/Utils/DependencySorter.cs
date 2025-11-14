using WiseByteDatabaseHelper.Core.Schema;

namespace WiseByteDatabaseHelper.Core.Utils;

public static class DependencySorter
{
    public static List<TableDefinition> Sort(List<TableDefinition> tables)
    {
        var sorted = new List<TableDefinition>();
        var pending = tables.ToList();

        while (pending.Any())
        {
            var free = pending
                .Where(t => !t.ForeignKeys
                    .Any(fk => pending.Any(p => p.Name == fk.ReferenceTable)))
                .ToList();

            if (!free.Any())
                throw new Exception("Circular or invalid FK relationships.");

            sorted.AddRange(free);
            foreach (var f in free)
                pending.Remove(f);
        }

        return sorted;
    }
}
