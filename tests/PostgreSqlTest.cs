using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Npgsql;
using Testcontainers.PostgreSql;
using Testcontainers.Xunit;
using Xunit.Abstractions;

namespace EFCore.Scaffolding.Tests;

[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "xUnit instantiates this class")]
public class PostgreSqlTest(PostgreSqlTest.PostgreSqlFixture dbFixture, ITestOutputHelper testOutputHelper) : ScaffolderTest<PostgreSqlTest.PostgreSqlFixture>(dbFixture, testOutputHelper)
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "xUnit instantiates this class")]
    public class PostgreSqlFixture(IMessageSink messageSink) : ContainerFixture<PostgreSqlBuilder, PostgreSqlContainer>(messageSink), IDbFixture
    {
        public DbConnectionStringBuilder ConnectionStringBuilder => new NpgsqlConnectionStringBuilder(Container.GetConnectionString());

        protected override PostgreSqlBuilder Configure(PostgreSqlBuilder builder)
        {
            return base.Configure(builder).WithResourceMapping(Path.Combine("Chinook", "Chinook_PostgreSql.sql"), "/docker-entrypoint-initdb.d/");
        }
    }
}