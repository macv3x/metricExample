using System.Collections.Concurrent;
using System.Diagnostics.Metrics;

namespace CounterTestApp;

public class ServicePerformanceMetrics<T> : IServicePerformanceMetrics<T> where T : class
{
    private readonly T _implementationType;
    private readonly ConcurrentDictionary<string, MethodMetrics> _methodMetrics = new();

    private readonly Meter _meter;

    public ServicePerformanceMetrics(T implementationType, IMeterFactory meterFactory)
    {
        _implementationType = implementationType ?? throw new ArgumentNullException(nameof(implementationType));
        var meterFactoryCheck = meterFactory ?? throw new ArgumentNullException(nameof(meterFactory));
        var baseName = GetBaseName();
        _meter = meterFactoryCheck.Create(baseName);
    }

    private string GetBaseName()
    {
        var genericType = _implementationType.GetType();

        var typeName = genericType.FullName ??
                       throw new InvalidOperationException(
                           "Initialization error. Type T does not have a fullname property value");

        return typeName;
    }

    public void RecordMethod(string methodName, long requestDurationInMilliseconds)
    {
        var methodMetrics = GetMethodMetrics(methodName);
        methodMetrics.RecordMethod(requestDurationInMilliseconds);
    }

    public void RecordError(string methodName)
    {
        var methodMetrics = GetMethodMetrics(methodName);
        methodMetrics.RecordError();
    }

    private MethodMetrics GetMethodMetrics(string methodName)
    {
        return _methodMetrics.GetOrAdd(methodName,
            (_) => new MethodMetrics(_meter, methodName));
    }
}

public interface IServicePerformanceMetrics<T>
{
    void RecordMethod(string methodName, long requestDurationInMilliseconds);
    void RecordError(string methodName);
}


