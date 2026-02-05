using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace EFCore.Scaffolding;

[SuppressMessage("Avoid uninstantiated internal classes", "CA1812", Justification = "It's instantiated through dependency injection")]
internal sealed class FilteringScaffoldingModelFactory : RelationalScaffoldingModelFactory
{
    private readonly IOperationReporter _reporter;
    private readonly ScaffolderSettings _settings;

    public FilteringScaffoldingModelFactory(ScaffolderSettings settings, IOperationReporter reporter, ICandidateNamingService candidateNamingService, IPluralizer pluralizer, ICSharpUtilities cSharpUtilities, IScaffoldingTypeMapper scaffoldingTypeMapper, IModelRuntimeInitializer modelRuntimeInitializer)
        : base(reporter, candidateNamingService, pluralizer, cSharpUtilities, scaffoldingTypeMapper, modelRuntimeInitializer)
    {
        _settings = settings;
        _reporter = reporter;
    }

    protected override ModelBuilder VisitDatabaseModel(ModelBuilder modelBuilder, DatabaseModel databaseModel)
    {
        foreach (var table in databaseModel.Tables.ToList().Where(t => !_settings.FilterTable(t)))
        {
            databaseModel.Tables.Remove(table);
            _reporter.WriteInformation($"Ignoring table {TableDescription(table)}");
        }

        if (_settings.SortColumnsComparer != null)
        {
            foreach (var table in databaseModel.Tables)
            {
                _reporter.WriteVerbose($"Sorting columns for table {TableDescription(table)}");
                ((List<DatabaseColumn>)table.Columns).Sort(_settings.SortColumnsComparer);
            }
        }

        return base.VisitDatabaseModel(modelBuilder, databaseModel);
    }

    protected override EntityTypeBuilder? VisitTable(ModelBuilder modelBuilder, DatabaseTable table)
    {
        foreach (var column in table.Columns.ToList().Where(c => !_settings.FilterColumn(c)))
        {
            table.Columns.Remove(column);
            _reporter.WriteInformation($"Ignoring column {ColumnDescription(column)}");
            AdjustIndexes(column);
        }
        return base.VisitTable(modelBuilder, table);
    }

    private void AdjustIndexes(DatabaseColumn column)
    {
        foreach (var index in column.Table.Indexes.ToList().Where(index => index.Columns.IndexOf(column) != -1))
        {
            column.Table.Indexes.Remove(index);
            _reporter.WriteInformation($"Ignoring index {IndexDescription(index)} because {ColumnDescription(column)} (which is part of that index) is ignored");
        }
    }

    private static string TableDescription(DatabaseTable table) => table.Schema == null ? table.Name : $"{table.Schema}.{table.Name}";

    private static string ColumnDescription(DatabaseColumn column) => $"{TableDescription(column.Table)}.{column.Name}";

    private static string IndexDescription(DatabaseIndex index) => index.Table == null ? index.Name ?? "<unknown>" : $"{TableDescription(index.Table)}.{index.Name}";
}