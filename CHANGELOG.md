# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [3.1.0][3.1.0] - 2026-02-05

* Add support for custom CLR types. See [Customizing the CLR type of properties](https://github.com/0xced/EFCore.Scaffolding/tree/main#customizing-the-clr-type-of-properties) for more information.
* Ignore indexes when a column being part of that index is ignored. (Fixes issue #89)
* Update to the latest provider versions which are compatible with EF Core 9
  * [EntityFrameworkCore.Jet 9.0.0](https://www.nuget.org/packages/EntityFrameworkCore.Jet/9.0.0) (unchanged)
  * [Microsoft.EntityFrameworkCore.Sqlite 9.0.12](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite/9.0.12)
  * [Microsoft.EntityFrameworkCore.SqlServer 9.0.12](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer/9.0.12)
  * [Npgsql.EntityFrameworkCore.PostgreSQL 9.0.4](https://www.nuget.org/packages/Npgsql.EntityFrameworkCore.PostgreSQL/9.0.4) (unchanged)
  * [Oracle.EntityFrameworkCore 9.23.26100](https://www.nuget.org/packages/Oracle.EntityFrameworkCore/9.23.26100)
  * [Pomelo.EntityFrameworkCore.MySql 9.0.0](https://www.nuget.org/packages/Pomelo.EntityFrameworkCore.MySql/9.0.0) (unchanged)
  * [System.Data.Odbc 9.0.12](https://www.nuget.org/packages/System.Data.Odbc/9.0.12)
  * [System.Data.OleDb 9.0.12](https://www.nuget.org/packages/System.Data.OleDb/9.0.12)

## [3.0.0][3.0.0] - 2025-10-10

* Update to latest provider versions
    * [EntityFrameworkCore.Jet 9.0.0](https://www.nuget.org/packages/EntityFrameworkCore.Jet/9.0.0)
    * [Microsoft.EntityFrameworkCore.Sqlite 9.0.9](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite/9.0.9)
    * [Microsoft.EntityFrameworkCore.SqlServer 9.0.9](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer/9.0.9)
    * [Npgsql.EntityFrameworkCore.PostgreSQL 9.0.4](https://www.nuget.org/packages/Npgsql.EntityFrameworkCore.PostgreSQL/9.0.4)
    * [Oracle.EntityFrameworkCore 9.23.90](https://www.nuget.org/packages/Oracle.EntityFrameworkCore/9.23.90)
    * [Pomelo.EntityFrameworkCore.MySql 9.0.0](https://www.nuget.org/packages/Pomelo.EntityFrameworkCore.MySql/9.0.0)
    * [System.Data.Odbc 9.0.9](https://www.nuget.org/packages/System.Data.Odbc/9.0.9)
    * [System.Data.OleDb 9.0.9](https://www.nuget.org/packages/System.Data.OleDb/9.0.9)

## [2.1.0][2.1.0] - 2024-11-12

* Add possibility to rename dependent end and/or principal end navigation properties with the new `RenameDependentEndNavigation` and `RenamePrincipalEndNavigation` functions. 

## [2.0.0][2.0.0] - 2024-09-16

* Update to latest provider versions
  * [EntityFrameworkCore.Jet 8.0.0](https://www.nuget.org/packages/EntityFrameworkCore.Jet/8.0.0)
  * [Microsoft.EntityFrameworkCore.Sqlite 8.0.8](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite/8.0.8)
  * [Microsoft.EntityFrameworkCore.SqlServer 8.0.8](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer/8.0.8)
  * [Npgsql.EntityFrameworkCore.PostgreSQL 8.0.4](https://www.nuget.org/packages/Npgsql.EntityFrameworkCore.PostgreSQL/8.0.4)
  * [Oracle.EntityFrameworkCore 8.23.50](https://www.nuget.org/packages/Oracle.EntityFrameworkCore/8.23.50)
  * [Pomelo.EntityFrameworkCore.MySql 8.0.2](https://www.nuget.org/packages/Pomelo.EntityFrameworkCore.MySql/8.0.2)

## [2.0.0-rc.1][2.0.0-rc.1] - 2024-04-22

* Update to .NET 8 / Entity Framework Core 8 and drop support for .NET 6
* Improve scaffolding of SQLite databases, see [SQLite Scaffolding: Use column type and values to provide a better CLR type](https://github.com/dotnet/efcore/pull/30816)
* Add a `NewLine` property on the `ScaffolderSettings` to control the line ending of scaffolded files

## [1.0.0][1.0.0] - 2023-09-28

Initial release

[3.1.0]: https://github.com/0xced/EFCore.Scaffolding/compare/3.0.0...3.1.0
[3.0.0]: https://github.com/0xced/EFCore.Scaffolding/compare/2.1.0...3.0.0
[2.1.0]: https://github.com/0xced/EFCore.Scaffolding/compare/2.0.0...2.1.0
[2.0.0]: https://github.com/0xced/EFCore.Scaffolding/compare/2.0.0-rc.1...2.0.0
[2.0.0-rc.1]: https://github.com/0xced/EFCore.Scaffolding/compare/1.0.0...2.0.0-rc.1
[1.0.0]: https://github.com/0xced/EFCore.Scaffolding/releases/tag/1.0.0
