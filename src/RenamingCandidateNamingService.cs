using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace EFCore.Scaffolding;

[SuppressMessage("Avoid uninstantiated internal classes", "CA1812", Justification = "It's instantiated through dependency injection")]
internal sealed class RenamingCandidateNamingService : CandidateNamingService
{
    private readonly ScaffolderSettings _settings;
    private readonly IOperationReporter _reporter;

    public RenamingCandidateNamingService(ScaffolderSettings settings, IOperationReporter reporter)
    {
        _settings = settings;
        _reporter = reporter;
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

    public override string GetDependentEndCandidateNavigationPropertyName(IReadOnlyForeignKey foreignKey)
    {
        var propertyName = base.GetDependentEndCandidateNavigationPropertyName(foreignKey);
        var result = _settings.RenameDependentEndNavigation(propertyName, foreignKey);
        if (result != propertyName)
        {
            _reporter.WriteInformation($"Renamed dependent end navigation {propertyName} to {result}");
        }
        return result;
    }

    public override string GetPrincipalEndCandidateNavigationPropertyName(IReadOnlyForeignKey foreignKey, string dependentEndNavigationPropertyName)
    {
        var propertyName = base.GetPrincipalEndCandidateNavigationPropertyName(foreignKey, dependentEndNavigationPropertyName);
        var result = _settings.RenamePrincipalEndNavigation(propertyName, foreignKey, dependentEndNavigationPropertyName);
        if (result != propertyName)
        {
            _reporter.WriteInformation($"Renamed principal end navigation {propertyName} to {result}");
        }
        return result;
    }
}