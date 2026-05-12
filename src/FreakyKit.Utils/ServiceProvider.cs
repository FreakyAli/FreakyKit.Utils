using System;

namespace FreakyKit.Utils;

public static class ServiceProvider
{
    /// <summary>
    /// Strongly-typed wrapper around <see cref="IServiceProvider.GetService(Type)"/>.
    /// Returns <c>null</c> when the service is not registered.
    /// </summary>
    /// <typeparam name="T">The service type to resolve.</typeparam>
    /// <param name="provider">The service provider; must not be <c>null</c>.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="provider"/> is <c>null</c>.</exception>
    public static T? GetService<T>(this IServiceProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider);
        return (T?)provider.GetService(typeof(T));
    }

    /// <summary>
    /// Strongly-typed resolver that throws when the service is not registered.
    /// Matches the contract of <c>Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService&lt;T&gt;</c>
    /// without taking the dependency.
    /// </summary>
    /// <typeparam name="T">The service type to resolve.</typeparam>
    /// <param name="provider">The service provider; must not be <c>null</c>.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="provider"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">Thrown when no service of type <typeparamref name="T"/> is registered.</exception>
    public static T GetRequiredService<T>(this IServiceProvider provider) where T : notnull
    {
        ArgumentNullException.ThrowIfNull(provider);
        var service = provider.GetService(typeof(T));
        if (service is null)
            throw new InvalidOperationException($"No service for type '{typeof(T)}' has been registered.");
        return (T)service;
    }
}