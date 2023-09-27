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

namespace EFCore.Scaffolding.Extensions;

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
            _reporter.WriteInformation($"Ignoring table {table.Schema}.{table.Name}");
        }

        if (_settings.SortColumnsComparer != null)
        {
            foreach (var table in databaseModel.Tables)
            {
                _reporter.WriteVerbose($"Sorting columns for table {table.Schema}.{table.Name}");
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
            _reporter.WriteInformation($"Ignoring column {column.Table.Name}.{column.Name}");
        }
        return base.VisitTable(modelBuilder, table);
    }
}