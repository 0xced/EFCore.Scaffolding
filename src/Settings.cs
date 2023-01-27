using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace EFCore.Scaffolding;

/// <summary>
/// Settings defining how a database is scaffolded into entity and context files.
/// </summary>
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class Settings
{
    public Settings(DbConnectionStringBuilder connectionStringBuilder)
    {
        ConnectionStringBuilder = connectionStringBuilder;
    }

    /// <summary>
    /// The connection string builder of the database to scaffold.
    /// </summary>
    public DbConnectionStringBuilder ConnectionStringBuilder { get; }

    /// <summary>
    /// The name of DbContext to generate.
    /// Defaults to the database name.
    /// </summary>
    public string? ContextName { get; init; }

    /// <summary>
    /// The path where to write the entity files.
    /// Defaults to the current directory.
    /// </summary>
    public DirectoryInfo OutputDirectory { get; init; } = new(Environment.CurrentDirectory);

    /// <summary>
    /// The path where to write the DbContext file. Uses the <see cref="OutputDirectory"/> if <c>null</c>.
    /// </summary>
    public DirectoryInfo? ContextOutputDirectory { get; init; }

    /// <summary>
    /// The tables to generate entity types for. Use an empty array to scaffold all available tables.
    /// </summary>
    public string[] Tables { get; init; } = Array.Empty<string>();

    /// <summary>
    /// The schemas of tables to generate entity types for. Use an empty array to scaffold all available schemas.
    /// </summary>
    public string[] Schemas { get; init; } = Array.Empty<string>();

    /// <summary>
    /// The namespace to use for entities. Matches the directory by default.
    /// </summary>
    public string? ModelNamespace { get; init; }

    /// <summary>
    /// The namespace to use for the DbContext. Matches the directory by default.
    /// </summary>
    public string? ContextNamespace { get; init; }

    /// <summary>
    /// A predicate that returns <c>true</c> if the existing file must be deleted prior to scaffolding, <c>false</c> otherwise.
    /// Defaults to delete all files that contain <c>&lt;auto-generated&gt;</c>.
    /// </summary>
    public Func<FileInfo, bool> ShouldDeleteFile { get; init; } = DefaultShouldDeleteFile;

    /// <summary>
    /// A function that returns a connection string appropriate to save into the scaffolded files.
    /// Defaults to remove the <c>password</c> from the connection string builder.
    /// </summary>
    public Func<DbConnectionStringBuilder, string> GetDisplayableConnectionString { get; init; } = DefaultGetDisplayableConnectionString;

    /// <summary>
    /// A predicate that returns <c>true</c> if the table must be added to the scaffolded model, <c>false</c> otherwise.
    /// Defaults to include all tables in the scaffolded model.
    /// </summary>
    public Predicate<DatabaseTable> FilterTable { get; init; } = _ => true;

    /// <summary>
    /// A function to potentially rename the proposed entity name.
    /// Defaults to using the proposed entity name.
    /// </summary>
    public Func<string, DatabaseTable, string> RenameEntity { get; init; } = (entityName, _) => entityName;

    /// <summary>
    /// A predicate that returns <c>true</c> if the column must be added to the scaffolded model, <c>false</c> otherwise.
    /// Defaults to include all columns in the scaffolded model.
    /// </summary>
    public Predicate<DatabaseColumn> FilterColumn { get; init; } = _ => true;

    /// <summary>
    /// A function to potentially rename the proposed property name.
    /// Defaults to using the proposed property name.
    /// </summary>
    public Func<string, DatabaseColumn, string> RenameProperty { get; init; } = (propertyName, _) => propertyName;

    /// <summary>
    /// An optional <see cref="IComparer{T}"/> used to sort columns.
    /// Defaults to <c>null</c>, i.e. the columns are scaffolded in the order they appear in the table.
    /// <para/>
    /// Use <see cref="ColumnNameComparer"/> to sort columns by name.
    /// </summary>
    public IComparer<DatabaseColumn>? SortColumnsComparer { get; init; }

    /// <summary>
    /// Reports scaffolding operations.
    /// Defaults to report errors, warnings and information messages on the console with the <see cref="Console.WriteLine()"/> method.
    /// </summary>
    public IOperationReportHandler ReportHandler { get; init; } = new OperationReportHandler(Console.WriteLine, Console.WriteLine, Console.WriteLine);

    /// <summary>
    /// The default implementation of the <see cref="ShouldDeleteFile"/> predicate.
    /// </summary>
    /// <param name="file">The file to test for deletion.</param>
    /// <returns><c>true</c> if the file contains <c>&lt;auto-generated&gt;</c>, <c>false</c> otherwise.</returns>
    public static bool DefaultShouldDeleteFile(FileInfo file)
    {
        return File.ReadAllText(file.FullName).Contains("<auto-generated>");
    }

    /// <summary>
    /// The default implementation of the <see cref="GetDisplayableConnectionString"/> method.
    /// </summary>
    /// <param name="builder">The <see cref="DbConnectionStringBuilder"/> out of which to get a displayable connection string.</param>
    /// <returns>A connection string appropriate to save into the scaffolded files.</returns>
    public static string DefaultGetDisplayableConnectionString(DbConnectionStringBuilder builder)
    {
        builder.Remove("password");
        return builder.ConnectionString;
    }
}