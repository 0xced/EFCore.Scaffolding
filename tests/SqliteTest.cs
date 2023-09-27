using System.Data.Common;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Xunit;
using Xunit.Abstractions;

namespace EFCore.Scaffolding.Tests;

public class SqliteTest : ScaffolderTest<SqliteTest.SqliteFixture>
{
    public SqliteTest(SqliteFixture dbFixture, ITestOutputHelper testOutputHelper) : base(dbFixture, testOutputHelper)
    {
    }

    public class SqliteFixture : IDbFixture, IAsyncLifetime
    {
        public DbConnectionStringBuilder ConnectionStringBuilder { get; private set; } = null!;

        public async Task InitializeAsync()
        {
            ConnectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "Chinook.sqlite" };

            await using var connection = new SqliteConnection(ConnectionStringBuilder.ConnectionString);
            await connection.OpenAsync();
            await using var command = connection.CreateCommand();
            command.CommandText = await File.ReadAllTextAsync(Path.Combine("Chinook", "Chinook_Sqlite.sql"));
            await command.ExecuteNonQueryAsync();
        }

        public Task DisposeAsync() => Task.CompletedTask;
    }
}