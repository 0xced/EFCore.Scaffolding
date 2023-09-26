using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace EFCore.Scaffolding.Extensions;

[SuppressMessage("Avoid uninstantiated internal classes", "CA1812", Justification = "It's instantiated through dependency injection")]
internal class RenamingCandidateNamingService : CandidateNamingService
{
    private readonly ScaffolderSettings _settings;
    private readonly IOperationReporter _reporter;

    public RenamingCandidateNamingService(ScaffolderSettings settings, IOperationReporter reporter)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _reporter = reporter ?? throw new ArgumentNullException(nameof(reporter));
    }

    public override string GenerateCandidateIdentifier(DatabaseTable originalTable)
    {
        var entityName = base.GenerateCandidateIdentifier(originalTable);
        var result = _settings.RenameEntity(entityName, originalTable);
        if (result != entityName)
        {
            _reporter.WriteInformation($"Renamed entity {entityName} to {result}");
        }
        return result;
    }

    public override string GenerateCandidateIdentifier(DatabaseColumn originalColumn)
    {
        var propertyName = base.GenerateCandidateIdentifier(originalColumn);
        var result = _settings.RenameProperty(propertyName, originalColumn);
        if (result != propertyName)
        {
            _reporter.WriteInformation($"Renamed property {originalColumn.Table.Name}.{propertyName} to {result}");
        }
        return result;
    }
}