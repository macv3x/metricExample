using System.Diagnostics.Metrics;

namespace CounterTestApp;

public class MethodMetrics
{
    private readonly Counter<int> _counterErrors;
    private readonly Counter<int> _counterCalls;
    private readonly Histogram<double> _histogramMilliseconds;

    public MethodMetrics(Meter meter, string baseName)
    {
        var errorCounterName = $"{baseName}.errors";
        _counterErrors = meter.CreateCounter<int>(errorCounterName);

        var callCounterName = $"{baseName}.count";
        _counterCalls = meter.CreateCounter<int>(callCounterName);

        var durationName = $"{baseName}.processing_time";
        _histogramMilliseconds = meter.CreateHistogram<double>(durationName, "ms");
    }

    public void RecordMethod(long requestDurationInMilliseconds)
    {
        _counterCalls.Add(1);
        _histogramMilliseconds.Record(requestDurationInMilliseconds);
    }

    public void RecordError()
    {
        _counterErrors.Add(1);
    }
}