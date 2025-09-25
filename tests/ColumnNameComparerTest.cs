using System;
using AwesomeAssertions;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Xunit;

namespace EFCore.Scaffolding.Tests;

public class ColumnNameComparerTest
{
    [Fact]
    public void ColumnNameComparer_Ordinal()
    {
        var target = new ColumnNameComparer(StringComparison.Ordinal);
        var result = target.Compare(new DatabaseColumn { Name = "HELLO" }, new DatabaseColumn { Name = "hello" });
        result.Should().BeNegative();
    }

    [Fact]
    public void ColumnNameComparer_OrdinalIgnoreCase()
    {
        var target = new ColumnNameComparer(StringComparison.OrdinalIgnoreCase);
        var result = target.Compare(new DatabaseColumn { Name = "HELLO" }, new DatabaseColumn { Name = "hello" });
        result.Should().Be(0);
    }
}