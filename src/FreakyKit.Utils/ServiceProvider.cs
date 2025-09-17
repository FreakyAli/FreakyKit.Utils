using System;

namespace FreakyKit.Utils;

public static class ServiceProvider
{
    public static T? GetService<T>(this IServiceProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider);
        return (T?)provider.GetService(typeof(T));
    }
}