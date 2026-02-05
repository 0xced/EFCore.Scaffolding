**EFCore.Scaffolding** is a configurable alternative to the `dotnet ef dbcontext scaffold` command for generating a `DbContext` and its entities.

[![NuGet](https://img.shields.io/nuget/v/EFCore.Scaffolding.svg?label=NuGet&logo=NuGet)](https://www.nuget.org/packages/EFCore.Scaffolding/) [![Continuous Integration](https://img.shields.io/github/actions/workflow/status/0xced/EFCore.Scaffolding/continuous-integration.yml?branch=main&label=Continuous%20Integration&logo=GitHub)](https://github.com/0xced/EFCore.Scaffolding/actions/workflows/continuous-integration.yml) [![Coverage](https://img.shields.io/codecov/c/github/0xced/EFCore.Scaffolding?label=Coverage&logo=Codecov&logoColor=f5f5f5)](https://codecov.io/gh/0xced/EFCore.Scaffolding)

## Getting started

Add the [EFCore.Scaffolding](https://www.nuget.org/packages/EFCore.Scaffolding/) NuGet package to your project using the NuGet Package Manager or run the following command:

```sh
dotnet add package EFCore.Scaffolding
```

Reverse engineering a database, also known as [scaffolding](https://learn.microsoft.com/en-us/ef/core/managing-schemas/scaffolding/), is usually performed with the `dotnet ef dbcontext scaffold` command. This command has a few shortcomings that this project aims to address.

### EF Core providers support

The following EF Core providers are supported out of the box. Pull requests are welcome to support additional providers.

* [EntityFrameworkCore.Jet](https://www.nuget.org/packages/EntityFrameworkCore.Jet) (on Windows only)
* [Microsoft.EntityFrameworkCore.Sqlite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite)
* [Microsoft.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer)
* [Npgsql.EntityFrameworkCore.PostgreSQL](https://www.nuget.org/packages/Npgsql.EntityFrameworkCore.PostgreSQL)
* [Oracle.EntityFrameworkCore](https://www.nuget.org/packages/Oracle.EntityFrameworkCore)
* [Pomelo.EntityFrameworkCore.MySql](https://www.nuget.org/packages/Pomelo.EntityFrameworkCore.MySql)

Scaffolding requires to use a typed connection string builder so that the scaffolder knows which provider to use.

```csharp
using EFCore.Scaffolding;
using Npgsql;

var connectionStringBuilder = new NpgsqlConnectionStringBuilder
{
    Host = "localhost",
    Database = "postgres",
    Username = "postgres",
    Password = "postgres",
};
Scaffolder.Run(new ScaffolderSettings(connectionStringBuilder));
```

### Filtering

Both tables/entities and columns/properties can be filtered programmatically with the `FilterTable` and `FilterColumn` predicates of the `ScaffolderSettings` object. Sometimes it's easier to exclude a few tables rather than explicitly list dozen of tables.

### Renaming

Databases in the real world are not perfect and often table and/or column names are less than ideal. The `RenameEntity` and `RenameProperty` functions are here to tweak the names as appropriate.
It is also possible to rename dependent end and/or principal end navigation properties through the `RenameDependentEndNavigation` and `RenamePrincipalEndNavigation` functions.

### Sorting properties

By default, properties are scaffolded in the same order as columns appear in the tables. If you prefer to keep them sorted alphabetically, it's possible by setting `SortColumnsComparer = new ColumnNameComparer()`. It's even possible to write your own comparer if you need to sort them in any other way.

### Customizing the CLR type of properties

The type of the generated properties can be customized by using the `ClrType` annotation on the columns. For example, the [BinaryData](https://learn.microsoft.com/en-us/dotnet/api/system.binarydata) type (from [System.Memory.Data](https://www.nuget.org/packages/System.Memory.Data)) can be generated instead of the default `byte[]` type by using a custom `FilterColumn` delegate to intercept and modify the columns.

```csharp
using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

var settings = new ScaffolderSettings(connectionStringBuilder)
{
    FilterColumn = column =>
    {
        // 👇 Identifies binary data on PostgreSQL. Another option could be to identify the column based on its name.
        if (column.StoreType is "bytea")
        {
            column.SetAnnotation(ScaffoldingAnnotationNames.ClrType, typeof(BinaryData));
        }
        return true;
    },
};
```

> [!NOTE]
> Depending on the custom CLR type chosen, a custom [value converter](https://learn.microsoft.com/en-us/ef/core/modeling/value-conversions) might be needed.

For `BinaryData`, implement this converter:

```csharp
using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

internal class BinaryDataConverter() : ValueConverter<BinaryData, byte[]>(v => v.ToArray(), v => BinaryData.FromBytes(v));
```

Finally, register it in your DbContext subclass:

```csharp
protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
{
    configurationBuilder.Properties<BinaryData>().HaveConversion<BinaryDataConverter>();
}
```

> [!TIP]
> Custom CLR types work fine for enums, too.

### And more…

The `ScaffolderSettings` class has a few more properties that will help you generate a perfect `DbContext` and its entities. All the public API is documented with XML comments.

## Sample code

Here's how to scaffold a real PostgreSQL database using the `EFCore.Scaffolding` package. This demonstrates how to filter out the `fax` columns and rename `Fulltext` with proper `FullText` casing. This also demonstrates how the files can be saved relative to the current C# file.

```csharp
using System;
using System.IO;
using System.Runtime.CompilerServices;
using EFCore.Scaffolding;
using Npgsql;

var connectionStringBuilder = new NpgsqlConnectionStringBuilder
{
    Host = "ep-square-wildflower-00220644.us-east-2.aws.neon.tech",
    Database = "chinook",
    Username = "AzureDiamond",
    Password = "correct horse battery staple",
};

var settings = new ScaffolderSettings(connectionStringBuilder)
{
    OutputDirectory = GetOutputDirectory(),
    FilterColumn = column => column.Name != "fax",
    RenameProperty = (propertyName, _) => propertyName.Replace("Fulltext", "FullText"),
};

Scaffolder.Run(settings);

Console.WriteLine($"✅ The database was scaffolded in {new Uri(settings.OutputDirectory.FullName)}");

return;

static DirectoryInfo GetOutputDirectory([CallerFilePath] string path = "")
    => new(Path.GetFullPath(Path.Combine(Path.GetDirectoryName(path)!, "..", "ChinookDatabase")));
```

You can actually try to run this code, I have hosted the [Chinook](https://github.com/lerocha/chinook-database/) sample database on the [free Neon tier](https://neon.tech/docs/introduction/free-tier).

## Database creation

Here's how to create the Chinook database on [Neon](https://neon.tech). These instructions are adapted from the [Neon documentation](https://neon.tech/docs/import/import-sample-data#chinook-database) but using the latest version of the Chinook database that uses snake_case table and column names.

1. Checkout the [chinook-database](https://github.com/lerocha/chinook-database/) repository

```shell
git clone https://github.com/lerocha/chinook-database/
```

2. Run a docker container to load the database (performing so many inserts directly on Neon is impossibly slow)

```shell
pg_container_id=`docker run -p 5432:5432 -e POSTGRES_USER=postgres -e POSTGRES_HOST_AUTH_METHOD=trust -d --rm --mount type=bind,source=${PWD}/ChinookDatabase/DataSources/Chinook_PostgreSql_AutoIncrementPKs.sql,destination=/docker-entrypoint-initdb.d/chinook.sql,readonly postgres:alpine`
```

3. Dump the database into `chinook_dump.sql`

```shell
docker exec ${pg_container_id} pg_dump --username postgres --no-owner --file chinook_dump.sql
```

ℹ️ This generates an SQL script with `COPY` statements which are _much_ faster than a series of `INSERT` statements.

4. Optionally extract this file out of the Docker container to have a look at it

```shell
docker cp ${pg_container_id}:/chinook_dump.sql .
```

5. Execute the generated script after creating a database named `chinook` in the Neon [console](http://console.neon.tech) and replacing the connection string with the one provided in the Neon dashboard


```shell
docker exec ${pg_container_id} psql -d "postgres://[user]:[password]@[neon_hostname]/chinook" -f chinook_dump.sql
```

6. Dispose the Docker container after checking that the database has been successfully imported

```shell
docker stop ${pg_container_id}
```

### Database permissions

```sql
-- Create the database (can also be done interactively in the Neon console)
CREATE DATABASE chinook;

-- Create the user and grant SELECT
CREATE USER "AzureDiamond" LOGIN PASSWORD 'correct horse battery staple';
GRANT SELECT ON ALL TABLES IN SCHEMA public TO "AzureDiamond";
```

Additional commands to check privileges and drop the user.

```sql
-- Verify the privileges
SELECT * FROM information_schema.role_table_grants WHERE grantee = 'AzureDiamond';

-- Drop user
REVOKE ALL PRIVILEGES ON ALL TABLES IN SCHEMA public FROM "AzureDiamond";
-- REVOKE ALL PRIVILEGES ON ALL FUNCTIONS IN SCHEMA public FROM "AzureDiamond";
-- REVOKE ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public FROM "AzureDiamond";
-- REVOKE ALL PRIVILEGES ON SCHEMA public FROM "AzureDiamond";
-- REVOKE ALL PRIVILEGES ON DATABASE chinook FROM "AzureDiamond";
DROP USER "AzureDiamond";
```

