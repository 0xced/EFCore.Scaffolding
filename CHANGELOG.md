# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.0.0-rc.1][2.0.0-rc.1] - 2024-04-22

* Update to .NET 8 / Entity Framework Core 8 and drop support for .NET 6
* Improve scaffolding of SQLite databases, see [SQLite Scaffolding: Use column type and values to provide a better CLR type](https://github.com/dotnet/efcore/pull/30816)
* Add a `NewLine` property on the `ScaffolderSettings` to control the line ending of scaffolded files

## [1.0.0][1.0.0] - 2023-09-28

Initial release

[2.0.0-rc.1]: https://github.com/0xced/EFCore.Scaffolding/compare/1.0.0...2.0.0-rc.1
[1.0.0]: https://github.com/0xced/EFCore.Scaffolding/releases/tag/1.0.0
