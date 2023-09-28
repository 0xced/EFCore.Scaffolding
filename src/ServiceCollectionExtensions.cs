using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EFCore.Scaffolding;

/// <summary>
/// Provides convenience methods to replace a service with another implementation.
/// </summary>
public static class ServiceCollectionExtensions
{
    internal static void AddFiltering(this IServiceCollection serviceCollection)
    {
        serviceCollection.Replace<IScaffoldingModelFactory, FilteringScaffoldingModelFactory>();
    }

    internal static void AddRenaming(this IServiceCollection serviceCollection)
    {
        serviceCollection.Replace<ICandidateNamingService, RenamingCandidateNamingService>();
    }

    internal static void AddWorkarounds(this IServiceCollection serviceCollection)
    {
        serviceCollection.Replace<ICSharpUtilities, WorkaroundCSharpUtilities>();
    }

    /// <summary>
    /// Replace the services identified by <typeparamref name="TService"/> with the implementation provided by <typeparamref name="TImplementation"/>.
    /// </summary>
    /// <param name="serviceCollection">The design-time service collection.</param>
    /// <typeparam name="TService">The type of the service to replace.</typeparam>
    /// <typeparam name="TImplementation">The implementation type for the replaced service.</typeparam>
    public static void Replace<TService, TImplementation>(this IServiceCollection serviceCollection)
    {
        serviceCollection.Replace(typeof(TService), typeof(TImplementation));
    }

    /// <summary>
    /// Replace the services identified by <paramref name="serviceType"/> with the implementation provided by <paramref name="implementationType"/>.
    /// </summary>
    /// <param name="serviceCollection">The design-time service collection.</param>
    /// <param name="serviceType">The type of the service to replace.</param>
    /// <param name="implementationType">The implementation type for the replaced service.</param>
    public static void Replace(this IServiceCollection serviceCollection, Type serviceType, Type implementationType)
    {
        foreach (var originalServiceDescriptor in serviceCollection.Where(e => e.ServiceType == serviceType).ToList())
        {
            var serviceDescriptor = new ServiceDescriptor(serviceType, implementationType, originalServiceDescriptor.Lifetime);
            serviceCollection.Replace(serviceDescriptor);
        }
    }
}