using WebApi.LoadTesting.Metrics;

namespace WebApi.LoadTesting.Models;

internal sealed record LoadTestSummary(MetricsSnapshot Metrics, TimeSpan Elapsed);
