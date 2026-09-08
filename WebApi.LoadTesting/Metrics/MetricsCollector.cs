using WebApi.LoadTesting.Models;

namespace WebApi.LoadTesting.Metrics;

internal sealed class MetricsCollector
{
    private readonly object _syncRoot = new();
    private readonly Dictionary<LoadTestOperation, OperationMetrics> _operations = new();
    private readonly Dictionary<string, long> _errors = new();
    private long _count;
    private long _successful;
    private double _totalMilliseconds;
    private double _minimum = double.MaxValue;
    private double _maximum;

    public long Failed
    {
        get { lock (_syncRoot) return _count - _successful; }
    }

    public void Add(RequestResult result)
    {
        ArgumentNullException.ThrowIfNull(result);
        lock (_syncRoot)
        {
            _count++;
            _successful += result.Success ? 1 : 0;
            _totalMilliseconds += result.Milliseconds;
            _minimum = Math.Min(_minimum, result.Milliseconds);
            _maximum = Math.Max(_maximum, result.Milliseconds);

            if (!_operations.TryGetValue(result.Operation, out var operation))
            {
                operation = new OperationMetrics();
                _operations[result.Operation] = operation;
            }
            operation.Add(result);

            if (!result.Success)
                _errors[result.Status] = _errors.GetValueOrDefault(result.Status) + 1;
        }
    }

    public MetricsSnapshot CreateSnapshot()
    {
        lock (_syncRoot)
        {
            return new(
                _count,
                _successful,
                _count - _successful,
                _count == 0 ? 0 : _totalMilliseconds / _count,
                _count == 0 ? 0 : _minimum,
                _maximum,
                _operations.ToDictionary(pair => pair.Key, pair => pair.Value.CreateSnapshot()),
                new Dictionary<string, long>(_errors)
            );
        }
    }

    private sealed class OperationMetrics
    {
        private long _count;
        private long _successful;
        private double _totalMilliseconds;

        public void Add(RequestResult result)
        {
            _count++;
            _successful += result.Success ? 1 : 0;
            _totalMilliseconds += result.Milliseconds;
        }

        public OperationMetricsSnapshot CreateSnapshot() =>
            new(_count, _successful, _count == 0 ? 0 : _totalMilliseconds / _count);
    }
}
