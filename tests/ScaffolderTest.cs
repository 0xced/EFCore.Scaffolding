using System;
using System.Data.Common;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

namespace EFCore.Scaffolding.Tests;

public interface IDbFixture
{
    DbConnectionStringBuilder ConnectionStringBuilder { get; }
}

public abstract class ScaffolderTest<TFixture> : IClassFixture<TFixture> where TFixture : class, IDbFixture
{
    private readonly IOperationReportHandler _operationReportHandler;
    private readonly DbConnectionStringBuilder _connectionStringBuilder;

    protected ScaffolderTest(TFixture dbFixture, ITestOutputHelper testOutputHelper)
    {
        _connectionStringBuilder = dbFixture.ConnectionStringBuilder;
        _operationReportHandler = new OperationReportHandler(
            errorHandler: message => testOutputHelper.WriteLine($"❌ {message}"),
            warningHandler: message => testOutputHelper.WriteLine($"⚠️ {message}"),
            informationHandler: message => testOutputHelper.WriteLine($"ℹ️ {message}"),
            verboseHandler: message => testOutputHelper.WriteLine($"💬 {message}")
        );
    }

    private static string GetStableConnectionString(DbConnectionStringBuilder builder)
    {
        builder.Remove("password");
        builder.Remove("port");
        return builder.ConnectionString;
    }

    private DirectoryInfo GetOutputDirectory([CallerMemberName] string testName = "") => new(Path.Combine(GetType().Name, testName));

    [Fact]
    public async Task Scaffold()
    {
        // Arrange
        var outputDirectory = GetOutputDirectory();

        // Act
        var settings = new ScaffolderSettings(_connectionStringBuilder)
        {
            ReportHandler = _operationReportHandler,
            ContextName = "ChinookContext",
            OutputDirectory = outputDirectory,
            GetDisplayableConnectionString = GetStableConnectionString,
        };
        _ = Scaffolder.Run(settings);

        // Assert
        await Verifier.VerifyDirectory(outputDirectory);
    }

    [Fact]
    public async Task ScaffoldOneTableOrderedColumns()
    {
        // Arrange
        var outputDirectory = GetOutputDirectory();

        // Act
        var settings = new ScaffolderSettings(_connectionStringBuilder)
        {
            ReportHandler = _operationReportHandler,
            ContextName = "ChinookContext",
            OutputDirectory = outputDirectory,
            GetDisplayableConnectionString = GetStableConnectionString,
            FilterTable = table => string.Equals(table.Name, "Customer", StringComparison.OrdinalIgnoreCase),
            FilterColumn = column => !string.Equals(column.Name, "Fax", StringComparison.OrdinalIgnoreCase),
            SortColumnsComparer = new ColumnNameComparer(),
        };
        Scaffolder.Run(settings);

        // Assert
        await Verifier.VerifyDirectory(outputDirectory);
    }

    [Fact]
    public async Task ScaffoldRename()
    {
        // Arrange
        var outputDirectory = GetOutputDirectory();

        // Act
        var settings = new ScaffolderSettings(_connectionStringBuilder)
        {
            ReportHandler = _operationReportHandler,
            ContextName = "ChinookContext",
            OutputDirectory = outputDirectory,
            GetDisplayableConnectionString = GetStableConnectionString,
            RenameEntity = (entityName, _) => entityName == "Customer" ? "Client" : entityName,
            RenameProperty = (propertyName, _) => propertyName.Replace("PostalCode", "ZipCode"),
        };
        Scaffolder.Run(settings);

        // Assert
        await Verifier.VerifyDirectory(outputDirectory);
    }
}