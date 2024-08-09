using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using Npgsql;
using Testcontainers.PostgreSql;
using Xunit;
using Xunit.Abstractions;

namespace EFCore.Scaffolding.Tests;

[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "xUnit instantiates this class")]
public class PostgreSqlTest : ScaffolderTest<PostgreSqlTest.PostgreSqlFixture>
{
    public PostgreSqlTest(PostgreSqlFixture dbFixture, ITestOutputHelper testOutputHelper) : base(dbFixture, testOutputHelper)
    {
    }

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "xUnit instantiates this class")]
    public class PostgreSqlFixture : IDbFixture, IAsyncLifetime
    {
        public DbConnectionStringBuilder ConnectionStringBuilder { get; private set; } = null!;

        private readonly PostgreSqlContainer _container = new PostgreSqlBuilder()
            .WithResourceMapping(Path.Combine("Chinook", "Chinook_PostgreSql.sql"), "/docker-entrypoint-initdb.d/")
            .Build();

        public async Task InitializeAsync()
        {
            await _container.StartAsync();
            ConnectionStringBuilder = new NpgsqlConnectionStringBuilder(_container.GetConnectionString());
        }

        public async Task DisposeAsync()
        {
            await _container.DisposeAsync();
        }
    }
}