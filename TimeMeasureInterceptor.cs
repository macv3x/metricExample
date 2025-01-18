using System.Diagnostics;
using Castle.DynamicProxy;

namespace CounterTestApp;

public class PerformanceMetricInterceptor<TImplementation>(IServicePerformanceMetrics<TImplementation> servicePerformanceMetrics) : IInterceptor where TImplementation : class
{
    public void Intercept(IInvocation invocation)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        invocation.Proceed();
        servicePerformanceMetrics.RecordMethod(invocation.Method.Name, stopwatch.ElapsedMilliseconds);
    }
}