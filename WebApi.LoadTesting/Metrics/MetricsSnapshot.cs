using WebApi.LoadTesting.Models;

namespace WebApi.LoadTesting.Metrics;

internal sealed record MetricsSnapshot(
    long Count,
    long Successful,
    long Failed,
    double Average,
    double Minimum,
    double Maximum,
    IReadOnlyDictionary<LoadTestOperation, OperationMetricsSnapshot> Operations,
    IReadOnlyDictionary<string, long> Errors
);

internal sealed record OperationMetricsSnapshot(long Count, long Successful, double Average);
