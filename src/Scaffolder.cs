using System;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFCore.Scaffolding.Extensions;
using EntityFrameworkCore.Jet.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using Npgsql;
using Oracle.ManagedDataAccess.Client;

namespace EFCore.Scaffolding;

/// <summary>
/// Creates an EntityFramework Core DbContext class and associated entities from an existing database.
/// </summary>
public static class Scaffolder
{
    [SuppressMessage("Avoid uninstantiated internal classes", "CA1812", Justification = "It's instantiated through dependency injection")]
    private sealed class DesignTimeServices : IDesignTimeServices
    {
        [ThreadStatic]
        public static ScaffolderSettings? CurrentSettings;

        public void ConfigureDesignTimeServices(IServiceCollection serviceCollection)
        {
            if (CurrentSettings == null)
                throw new InvalidOperationException("The current settings must not be null");

            serviceCollection.AddSingleton(CurrentSettings);
            serviceCollection.AddFiltering();
            serviceCollection.AddRenaming();
            serviceCollection.AddWorkarounds();

            CurrentSettings = null;
        }
    }

    /// <summary>
    /// Generates a DbContext subclass and its associated entities according to the <paramref name="settings"/>.
    /// </summary>
    /// <param name="settings">The <see cref="ScaffolderSettings"/> that controls how the classes are generated.</param>
    /// <returns>An object that contains the path of the generated files.</returns>
    public static SavedModelFiles Run(ScaffolderSettings settings)
    {
        DeleteExistingFiles(settings ?? throw new ArgumentNullException(nameof(settings)));
        var providerName = GetProviderName(settings.ConnectionStringBuilder);
        var savedModelFiles = ScaffoldContextAndEntities(settings, providerName);
        AddAutogeneratedComment(settings, savedModelFiles, providerName);
        return savedModelFiles;
    }

    private static void DeleteExistingFiles(ScaffolderSettings settings)
    {
        if (!settings.OutputDirectory.Exists)
        {
            return;
        }

        foreach (var file in settings.OutputDirectory.GetFiles("*.cs").Where(settings.ShouldDeleteFile))
        {
            file.Delete();
        }
    }

    private static SavedModelFiles ScaffoldContextAndEntities(ScaffolderSettings settings, string providerName)
    {
        var designAssembly = typeof(DesignTimeServices).Assembly;

        var operations = new DatabaseOperations(
            new OperationReporter(settings.ReportHandler),
            assembly: designAssembly,
            startupAssembly: designAssembly,
            projectDir: "",
            rootNamespace: "",
            language: null,
            nullable: true,
            args: Array.Empty<string>()
        );

        var modelNamespace = settings.ModelNamespace ?? settings.OutputDirectory.Name;
        var contextNamespace = settings.ContextOutputDirectory == null ? modelNamespace : settings.ContextNamespace ?? settings.ContextOutputDirectory.Name;

        DesignTimeServices.CurrentSettings = settings;
        return operations.ScaffoldContext(
            provider: providerName,
            connectionString: settings.ConnectionStringBuilder.ConnectionString,
            outputDir: settings.OutputDirectory.FullName,
            outputContextDir: (settings.ContextOutputDirectory ?? settings.OutputDirectory).FullName,
            dbContextClassName: settings.ContextName,
            schemas: settings.Schemas,
            tables: settings.Tables,
            modelNamespace: modelNamespace,
            contextNamespace: contextNamespace,
            useDataAnnotations: false,
            overwriteFiles: false,
            useDatabaseNames: false,
            suppressOnConfiguring: true,
            noPluralize: false
        );
    }

    private static string GetProviderName(DbConnectionStringBuilder connectionStringBuilder)
    {
        using var dbContext = CreateDbContext(connectionStringBuilder);
        return dbContext.Database.ProviderName ?? throw new InvalidOperationException($"Provider name for {connectionStringBuilder.GetType().Name} could not be determined.");
    }

    private static DbContext CreateDbContext(DbConnectionStringBuilder connectionStringBuilder)
    {
        var connectionStringBuilderName = connectionStringBuilder.GetType().Name;
        var builder = new DbContextOptionsBuilder();
        var isWindows = OperatingSystem.IsWindows();
        var optionsBuilder = connectionStringBuilder switch
        {
            JetConnectionStringBuilder jet => isWindows ? builder.UseJet(".", jet.ProviderType) : builder,
            MySqlConnectionStringBuilder => builder.UseMySql(MySqlServerVersion.LatestSupportedServerVersion),
            NpgsqlConnectionStringBuilder => builder.UseNpgsql(),
            OracleConnectionStringBuilder => builder.UseOracle(),
            SqliteConnectionStringBuilder => builder.UseSqlite(),
            SqlConnectionStringBuilder => builder.UseSqlServer(),
            _ => builder,
        };
        if (!optionsBuilder.IsConfigured)
        {
            var jetConnectionStringBuilder = isWindows ? $"{nameof(JetConnectionStringBuilder)}, " : "";
            throw new NotSupportedException($"Unsupported connection string builder: {connectionStringBuilderName}. " +
                                            "The supported connection string builders are " +
                                            jetConnectionStringBuilder +
                                            $"{nameof(MySqlConnectionStringBuilder)}, " +
                                            $"{nameof(NpgsqlConnectionStringBuilder)}, " +
                                            $"{nameof(OracleConnectionStringBuilder)}, " +
                                            $"{nameof(SqliteConnectionStringBuilder)} and {nameof(SqlConnectionStringBuilder)}.");
        }
        return new DbContext(optionsBuilder.Options);
    }

    private static DbConnectionStringBuilder Copy(DbConnectionStringBuilder connectionStringBuilder)
    {
        using var dbContext = CreateDbContext(connectionStringBuilder);
        using var connection = dbContext.Database.GetDbConnection();
        var factory = DbProviderFactories.GetFactory(connection)
                      ?? throw new InvalidOperationException($"DbProviderFactories.GetFactory({connection}) returned null.");
        var builder = factory.CreateConnectionStringBuilder()
                      ?? throw new InvalidOperationException($"CreateConnectionStringBuilder() returned null for {factory}.");
        builder.ConnectionString = connectionStringBuilder.ConnectionString;
        return builder;
    }

    private static void AddAutogeneratedComment(ScaffolderSettings settings, SavedModelFiles savedModelFiles, string providerName)
    {
        Parallel.ForEach(savedModelFiles.AdditionalFiles.Append(savedModelFiles.ContextFile).Distinct(), filePath =>
        {
            var connectionString = settings.GetDisplayableConnectionString(Copy(settings.ConnectionStringBuilder));
            var maxLength = Math.Max(providerName.Length, connectionString.Length);
            var lines = new StringBuilder();
            lines.AppendLine("// <auto-generated>");
            lines.AppendLine("//");
            var dashesPadding = new string('─', maxLength);
            lines.AppendLine(CultureInfo.InvariantCulture, $"// ┌───────────────────┬─{dashesPadding}─┐");
            lines.AppendLine(CultureInfo.InvariantCulture, $"// │ Provider          │ {providerName}{new string(' ', 1 + maxLength - providerName.Length)}│");
            lines.AppendLine(CultureInfo.InvariantCulture, $"// ├───────────────────┼─{dashesPadding}─┤");
            lines.AppendLine(CultureInfo.InvariantCulture, $"// │ Connection String │ {connectionString}{new string(' ', 1 + maxLength - connectionString.Length)}│");
            lines.AppendLine(CultureInfo.InvariantCulture, $"// └───────────────────┴─{dashesPadding}─┘");
            lines.AppendLine("// ");
            lines.AppendLine("// </auto-generated>");
            lines.AppendLine();
            lines.AppendLine("#nullable enable");
            lines.AppendLine("#pragma warning disable");
            lines.AppendLine();
            foreach (var line in File.ReadAllLines(filePath))
            {
                lines.AppendLine(line);
            }
            File.WriteAllText(filePath, lines.ToString());
        });
    }
}