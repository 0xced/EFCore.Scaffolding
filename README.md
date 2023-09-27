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

## Sample code

Here's how to scaffold a real PostgreSQL database using the `EFCore.Scaffolding` package.

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

You can actually try to run this code, I have hosted the [Chinook](https://github.com/lerocha/chinook-database/) sample database on the free [Neon](https://neon.tech) tier.