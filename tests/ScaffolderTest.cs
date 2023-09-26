using System.Data.Common;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

namespace EFCore.Scaffolding.Tests;

[UsesVerify]
public abstract class ScaffolderTest : IAsyncLifetime
{
    private readonly IOperationReportHandler _operationReportHandler;

    private readonly DirectoryInfo _outputDirectory;

    protected ScaffolderTest(ITestOutputHelper testOutputHelper)
    {
        _outputDirectory = new DirectoryInfo(GetType().Name);
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

    [Fact]
    public async Task Scaffold()
    {
        // Arrange
        var connectionStringBuilder = await GetConnectionStringBuilderAsync();

        // Act
        var settings = new ScaffolderSettings(connectionStringBuilder)
        {
            ReportHandler = _operationReportHandler,
            ContextName = "ChinookContext",
            OutputDirectory = _outputDirectory,
            GetDisplayableConnectionString = GetStableConnectionString,
        };
        Scaffolder.Run(settings);

        // Assert
        await Verifier.VerifyDirectory(_outputDirectory);
    }

    protected abstract Task<DbConnectionStringBuilder> GetConnectionStringBuilderAsync();

    public virtual Task InitializeAsync() => Task.CompletedTask;

    public virtual Task DisposeAsync() => Task.CompletedTask;
}