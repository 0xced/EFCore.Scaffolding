using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using PublicApiGenerator;
using VerifyXunit;
using Xunit;

namespace EFCore.Scaffolding.Tests;

public class PublicApi
{
    [Theory]
    [ClassData(typeof(TargetFrameworksTheoryData))]
    public Task ApprovePublicApi(string targetFramework)
    {
        var testAssembly = typeof(PublicApi).Assembly;
        var configuration = testAssembly.GetCustomAttribute<AssemblyConfigurationAttribute>()?.Configuration
                            ?? throw new InvalidDataException($"{nameof(AssemblyConfigurationAttribute)} not found in {testAssembly.Location}");
        var assemblyPath = Path.Combine(GetSrcDirectoryPath(), "bin", configuration, targetFramework, "EFCore.Scaffolding.dll");
        var assembly = Assembly.LoadFile(assemblyPath);
        var publicApi = assembly.GeneratePublicApi();
        return Verifier.Verify(publicApi, "cs").UseFileName($"PublicApi.{targetFramework}");
    }

    private static string GetSrcDirectoryPath([CallerFilePath] string path = "") => Path.Combine(Path.GetDirectoryName(path)!, "..", "src");

    private class TargetFrameworksTheoryData : TheoryData<string>
    {
        public TargetFrameworksTheoryData()
        {
            var csprojPath = Path.Combine(GetSrcDirectoryPath(), "EFCore.Scaffolding.csproj");
            var project = XDocument.Load(csprojPath);
            var targetFrameworks = project.XPathSelectElement("/Project/PropertyGroup/TargetFrameworks")?.Value.Split(';', StringSplitOptions.RemoveEmptyEntries)
                                   ?? [project.XPathSelectElement("/Project/PropertyGroup/TargetFramework")?.Value ?? throw new InvalidDataException($"TargetFramework(s) element not found in {csprojPath}")];
            AddRange(targetFrameworks);
        }
    }
}