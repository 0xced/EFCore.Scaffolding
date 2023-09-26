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
#elif false
var connectionStringBuilder = new MySqlConnector.MySqlConnectionStringBuilder
{
    Server = "sqlprostudio-mysql.csyg8tkobue6.us-west-2.rds.amazonaws.com",
    Database = "northwind",
    UserID = "sqlprostudio-ro",
    Password = "password123",
};
#elif false
const string dbFileName = "GardenCompany01.accdb";
using var httpClient = new System.Net.Http.HttpClient();
await using (var source = await httpClient.GetStreamAsync($"https://resources.oreilly.com/examples/9780735669086-files/-/raw/2028cbd72de96040988172d954d0437779a4c269/Chapter01/{dbFileName}"))
await using (var destination = new FileStream(dbFileName, FileMode.Create, FileAccess.Write))
{
    await source.CopyToAsync(destination);
}
var connectionStringBuilder = new EntityFrameworkCore.Jet.Data.JetConnectionStringBuilder { DataSource = dbFileName };
#else
const string dbFileName = "Chinook_Sqlite.sqlite";
using var httpClient = new System.Net.Http.HttpClient();
await using (var source = await httpClient.GetStreamAsync($"https://github.com/lerocha/chinook-database/raw/master/ChinookDatabase/DataSources/{dbFileName}"))
await using (var destination = new FileStream(dbFileName, FileMode.Create, FileAccess.Write))
{
    await source.CopyToAsync(destination);
}
var connectionStringBuilder = new Microsoft.Data.Sqlite.SqliteConnectionStringBuilder { DataSource = dbFileName };
#endif
var settings = new ScaffolderSettings(connectionStringBuilder)
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