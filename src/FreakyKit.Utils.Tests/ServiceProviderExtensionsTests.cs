namespace FreakyKit.Utils.Tests;

public class ServiceProviderExtensionsTests
{
    private sealed class SimpleServiceProvider : IServiceProvider
    {
        private readonly Dictionary<Type, object> _services = [];

        public void Register<T>(T service) where T : notnull => _services[typeof(T)] = service;

        public object? GetService(Type serviceType) =>
            _services.TryGetValue(serviceType, out var svc) ? svc : null;
    }

    [Fact]
    public void GetService_RegisteredService_ReturnsInstance()
    {
        var provider = new SimpleServiceProvider();
        provider.Register("hello world");

        var result = provider.GetService<string>();

        Assert.Equal("hello world", result);
    }

    [Fact]
    public void GetService_UnregisteredService_ReturnsNull()
    {
        var provider = new SimpleServiceProvider();

        var result = provider.GetService<string>();

        Assert.Null(result);
    }

    [Fact]
    public void GetService_NullProvider_ThrowsArgumentNullException()
    {
        IServiceProvider provider = null!;

        Assert.Throws<ArgumentNullException>(() => provider.GetService<string>());
    }
}
