[assembly: System.CLSCompliant(false)]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/0xced/EFCore.Scaffolding")]
[assembly: System.Runtime.Versioning.TargetFramework(".NETCoreApp,Version=v8.0", FrameworkDisplayName=".NET 8.0")]
namespace EFCore.Scaffolding
{
    public class ColumnNameComparer : System.Collections.Generic.IComparer<Microsoft.EntityFrameworkCore.Scaffolding.Metadata.DatabaseColumn>
    {
        public ColumnNameComparer() { }
        public ColumnNameComparer(System.StringComparison stringComparison) { }
        public int Compare(Microsoft.EntityFrameworkCore.Scaffolding.Metadata.DatabaseColumn? x, Microsoft.EntityFrameworkCore.Scaffolding.Metadata.DatabaseColumn? y) { }
    }
    public static class Scaffolder
    {
        public static Microsoft.EntityFrameworkCore.Scaffolding.SavedModelFiles Run(EFCore.Scaffolding.ScaffolderSettings settings) { }
    }
    public class ScaffolderSettings
    {
        public ScaffolderSettings(System.Data.Common.DbConnectionStringBuilder connectionStringBuilder) { }
        public System.Data.Common.DbConnectionStringBuilder ConnectionStringBuilder { get; }
        public System.Action<Microsoft.Extensions.DependencyInjection.IServiceCollection>? ConfigureServices { get; init; }
        public string? ContextName { get; init; }
        public string? ContextNamespace { get; init; }
        public System.IO.DirectoryInfo? ContextOutputDirectory { get; init; }
        public System.Predicate<Microsoft.EntityFrameworkCore.Scaffolding.Metadata.DatabaseColumn> FilterColumn { get; init; }
        public System.Predicate<Microsoft.EntityFrameworkCore.Scaffolding.Metadata.DatabaseTable> FilterTable { get; init; }
        public System.Func<System.Data.Common.DbConnectionStringBuilder, string> GetDisplayableConnectionString { get; init; }
        public string? ModelNamespace { get; init; }
        public System.IO.DirectoryInfo OutputDirectory { get; init; }
        public System.Func<string, Microsoft.EntityFrameworkCore.Scaffolding.Metadata.DatabaseTable, string> RenameEntity { get; init; }
        public System.Func<string, Microsoft.EntityFrameworkCore.Scaffolding.Metadata.DatabaseColumn, string> RenameProperty { get; init; }
        public Microsoft.EntityFrameworkCore.Design.IOperationReportHandler ReportHandler { get; init; }
        public System.Collections.Generic.IEnumerable<string> Schemas { get; init; }
        public System.Func<System.IO.FileInfo, bool> ShouldDeleteFile { get; init; }
        public System.Collections.Generic.IComparer<Microsoft.EntityFrameworkCore.Scaffolding.Metadata.DatabaseColumn>? SortColumnsComparer { get; init; }
        public System.Collections.Generic.IEnumerable<string> Tables { get; init; }
        public static string DefaultGetDisplayableConnectionString(System.Data.Common.DbConnectionStringBuilder builder) { }
        public static bool DefaultShouldDeleteFile(System.IO.FileInfo file) { }
    }
    public static class ServiceCollectionExtensions
    {
        public static void Replace(this Microsoft.Extensions.DependencyInjection.IServiceCollection serviceCollection, System.Type serviceType, System.Type implementationType) { }
        public static void Replace<TService, TImplementation>(this Microsoft.Extensions.DependencyInjection.IServiceCollection serviceCollection) { }
    }
}