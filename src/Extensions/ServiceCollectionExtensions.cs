using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EFCore.Scaffolding.Extensions;

internal static class ServiceCollectionExtensions
{
    public static void AddFiltering(this IServiceCollection serviceCollection)
    {
        Replace(serviceCollection, typeof(IScaffoldingModelFactory), typeof(FilteringScaffoldingModelFactory));
    }

    public static void AddRenaming(this IServiceCollection serviceCollection)
    {
        Replace(serviceCollection, typeof(ICandidateNamingService), typeof(RenamingCandidateNamingService));
    }

    public static void AddWorkarounds(this IServiceCollection serviceCollection)
    {
        Replace(serviceCollection, typeof(ICSharpUtilities), typeof(WorkaroundCSharpUtilities));
    }

    private static void Replace(IServiceCollection serviceCollection, Type serviceType, Type implementationType)
    {
        var originalServiceDescriptor = serviceCollection.Single(e => e.ServiceType == serviceType);
        var serviceDescriptor = new ServiceDescriptor(serviceType, implementationType, originalServiceDescriptor.Lifetime);
        serviceCollection.Replace(serviceDescriptor);
    }
}