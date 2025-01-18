using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;

namespace CounterTestApp;

public static class CounterServiceExtension
{
    public static IServiceCollection AddMeasurementTransient<TService, TImplementation>(this IServiceCollection collection) where TService : class
        where TImplementation : class, TService
    {
        collection.AddSingleton<IInterceptor, PerformanceMetricInterceptor<TImplementation>>();
        collection.AddSingleton<IServicePerformanceMetrics<TImplementation>, ServicePerformanceMetrics<TImplementation>>();
        collection.AddSingleton<TImplementation>();
        collection.Add(new ServiceDescriptor(typeof(TService), provider =>
        {
            var generator = new ProxyGenerator();
            var interceptor = provider.GetRequiredService<IInterceptor>();
            return generator.CreateInterfaceProxyWithTarget<TService>(ActivatorUtilities.CreateInstance<TImplementation>(provider), interceptor);
        }, ServiceLifetime.Singleton));

        return collection;
    }
}