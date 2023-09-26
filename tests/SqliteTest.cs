using System.Data.Common;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Xunit.Abstractions;

namespace EFCore.Scaffolding.Tests;

public class SqliteTest : ScaffolderTest
{
    public SqliteTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
    }

    protected override async Task<DbConnectionStringBuilder> GetConnectionStringBuilderAsync()
    {
        var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "Chinook.sqlite" };
        await using var connection = new SqliteConnection(connectionStringBuilder.ConnectionString);
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = await File.ReadAllTextAsync(Path.Combine("Chinook", "Chinook_Sqlite.sql"));
        await command.ExecuteNonQueryAsync();
        return connectionStringBuilder;
    }
}