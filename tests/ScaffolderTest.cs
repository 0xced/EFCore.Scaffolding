using System.IO;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Design;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

namespace EFCore.Scaffolding.Tests;

[UsesVerify]
public class ScaffolderTest
{
    private readonly IOperationReportHandler _operationReportHandler;

    public ScaffolderTest(ITestOutputHelper testOutputHelper)
    {
        _operationReportHandler = new OperationReportHandler(
            errorHandler: message => testOutputHelper.WriteLine($"❌ {message}"),
            warningHandler: message => testOutputHelper.WriteLine($"⚠️ {message}"),
            informationHandler: message => testOutputHelper.WriteLine($"ℹ️ {message}"),
            verboseHandler: message => testOutputHelper.WriteLine($"💬 {message}")
        );
    }

    [Fact]
    public async Task ScaffoldSqlite()
    {
        // Arrange
        var outputDirectory = new DirectoryInfo("Chinook_Sqlite");
        var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "Chinook.sqlite" };
        await using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
        {
            await connection.OpenAsync();
            await using var command = connection.CreateCommand();
            command.CommandText = await File.ReadAllTextAsync(Path.Combine("Chinook", "Chinook_Sqlite.sql"));
            await command.ExecuteNonQueryAsync();
        }

        // Act
        var settings = new ScaffolderSettings(connectionStringBuilder)
        {
            ReportHandler = _operationReportHandler,
            ContextName = "ChinookContext",
            OutputDirectory = outputDirectory,
        };
        Scaffolder.Run(settings);

        // Assert
        await Verifier.VerifyDirectory(outputDirectory.FullName);
    }
}