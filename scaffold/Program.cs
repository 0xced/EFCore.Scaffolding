using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using EFCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

#if false
var connectionStringBuilder = new Npgsql.NpgsqlConnectionStringBuilder
{
    Host = "sqlprostudio-postgres.csyg8tkobue6.us-west-2.rds.amazonaws.com",
    Port = 5432,
    Database = "booktown",
    Username = "sqlprostudio-ro",
    Password = "password123",
};
#elif false
var connectionStringBuilder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder
{
    DataSource = "sqlprosample.database.windows.net",
    InitialCatalog = "sqlprosample",
    UserID = "sqlproro",
    Password = "nh{Zd?*8ZU@Y}Bb#",
};
#else
var connectionStringBuilder = new MySqlConnector.MySqlConnectionStringBuilder
{
    Server = "sqlprostudio-mysql.csyg8tkobue6.us-west-2.rds.amazonaws.com",
    Database = "northwind",
    UserID = "sqlprostudio-ro",
    Password = "password123",
};
#endif
var settings = new Settings(connectionStringBuilder)
{
    OutputDirectory = GetOutputDirectory(),
    FilterTable = FilterTable,
    FilterColumn = FilterColumn,
    RenameEntity = RenameEntity,
    RenameProperty = RenameProperty,
    SortColumnsComparer = new ColumnNameComparer(),
};
Scaffolder.Run(settings);
Console.WriteLine($"Successfully scaffolded into {new Uri(settings.OutputDirectory.FullName)}");

static bool FilterTable(DatabaseTable table)
{
    var ignored = new[] { "p2p", "teacher", "subject", "projekt", "projektmitarbeiter" };
    return !ignored.Contains(table.Name);
}

static bool FilterColumn(DatabaseColumn column)
{
    return true;
}

static string RenameEntity(string entityName, DatabaseTable table)
{
    return entityName;
}

static string RenameProperty(string propertyName, DatabaseColumn column)
{
    return propertyName;
}

static DirectoryInfo GetOutputDirectory([CallerFilePath] string path = "")
{
    return new DirectoryInfo(Path.GetFullPath(Path.Combine(Path.GetDirectoryName(path)!, "..", "..", "MyDatabase")));
}