using System.Data.Common;
using System.IO;
using System.Threading.Tasks;
using Npgsql;
using Testcontainers.PostgreSql;
using Xunit;
using Xunit.Abstractions;

namespace EFCore.Scaffolding.Tests;

public class PostgreSqlTest : ScaffolderTest<PostgreSqlTest.PostgreSqlFixture>
{
    public PostgreSqlTest(PostgreSqlFixture dbFixture, ITestOutputHelper testOutputHelper) : base(dbFixture, testOutputHelper)
    {
    }

    public class PostgreSqlFixture : IDbFixture, IAsyncLifetime
    {
        public DbConnectionStringBuilder ConnectionStringBuilder { get; private set; } = null!;

        private readonly PostgreSqlContainer _container = new PostgreSqlBuilder().Build();

        public async Task InitializeAsync()
        {
            await _container.StartAsync();
            await _container.ExecScriptAsync(await File.ReadAllTextAsync(Path.Combine("Chinook", "Chinook_PostgreSql.sql")));
            ConnectionStringBuilder = new NpgsqlConnectionStringBuilder(_container.GetConnectionString());
        }

        public async Task DisposeAsync()
        {
            await _container.DisposeAsync();
        }
    }
}