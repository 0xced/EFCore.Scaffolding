using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace EFCore.Scaffolding;

/// <summary>
/// A comparer to order column by their name.
/// </summary>
public class ColumnNameComparer : IComparer<DatabaseColumn>
{
    private readonly StringComparison _stringComparison;

    /// <summary>
    /// Initialize a new instance of the <see cref="ColumnNameComparer"/> class with the <see cref="StringComparison.Ordinal"/> comparison.
    /// </summary>
    public ColumnNameComparer() : this(StringComparison.Ordinal)
    {
    }

    /// <summary>
    /// Initialize a new instance of the <see cref="ColumnNameComparer"/> class.
    /// </summary>
    /// <param name="stringComparison">The <see cref="StringComparison"/> used to compare column names.</param>
    public ColumnNameComparer(StringComparison stringComparison)
    {
        _stringComparison = stringComparison;
    }

    /// <inheritdoc />
    public int Compare(DatabaseColumn? x, DatabaseColumn? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (ReferenceEquals(null, y)) return 1;
        if (ReferenceEquals(null, x)) return -1;
        return string.Compare(x.Name, y.Name, _stringComparison);
    }
}