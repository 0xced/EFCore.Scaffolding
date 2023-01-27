using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace EFCore.Scaffolding;

public class ColumnNameComparer : IComparer<DatabaseColumn>
{
    private readonly StringComparison _stringComparison;

    public ColumnNameComparer() : this(StringComparison.Ordinal)
    {
    }

    public ColumnNameComparer(StringComparison stringComparison)
    {
        _stringComparison = stringComparison;
    }

    public int Compare(DatabaseColumn? x, DatabaseColumn? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (ReferenceEquals(null, y)) return 1;
        if (ReferenceEquals(null, x)) return -1;
        return string.Compare(x.Name, y.Name, _stringComparison);
    }
}