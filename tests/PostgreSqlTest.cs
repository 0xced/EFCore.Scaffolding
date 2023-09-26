using System.Data.Common;
using System.IO;
using System.Threading.Tasks;
using Npgsql;
using Testcontainers.PostgreSql;
using Xunit.Abstractions;

namespace EFCore.Scaffolding.Tests;

public class PostgreSqlTest : ScaffolderTest
{
    private readonly PostgreSqlContainer _container;

    public PostgreSqlTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper) => _container = new PostgreSqlBuilder().Build();

    public override Task InitializeAsync() => _container.StartAsync();

    public override Task DisposeAsync() => _container.DisposeAsync().AsTask();

    protected override async Task<DbConnectionStringBuilder> GetConnectionStringBuilderAsync()
    {
        await _container.ExecScriptAsync(await File.ReadAllTextAsync(Path.Combine("Chinook", "Chinook_PostgreSql.sql")));
        return new NpgsqlConnectionStringBuilder { ConnectionString = _container.GetConnectionString() };
    }
}