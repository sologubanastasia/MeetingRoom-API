using System.Diagnostics;
using WebApi.LoadTesting.Clients;
using WebApi.LoadTesting.Configuration;
using WebApi.LoadTesting.Metrics;
using WebApi.LoadTesting.Models;

namespace WebApi.LoadTesting.Managers;

internal sealed class LoadTestManager
{
    private readonly MeetingRoomApiClient _apiClient;

    public LoadTestManager(MeetingRoomApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<LoadTestSummary> RunAsync(LoadTestOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        await _apiClient.InitializeAsync();

        var metrics = new MetricsCollector();
        using var semaphore = new SemaphoreSlim(options.Concurrency);
        var stopwatch = Stopwatch.StartNew();

        var tasks = Enumerable.Range(0, options.Requests)
            .Select(index => ExecuteRequestAsync(index, semaphore, metrics))
            .ToArray();

        await Task.WhenAll(tasks);
        stopwatch.Stop();

        return new LoadTestSummary(metrics.CreateSnapshot(), stopwatch.Elapsed);
    }

    private async Task ExecuteRequestAsync(
        int requestIndex,
        SemaphoreSlim semaphore,
        MetricsCollector metrics
    )
    {
        await semaphore.WaitAsync();
        try
        {
            metrics.Add(await _apiClient.SendAsync(requestIndex));
        }
        finally
        {
            semaphore.Release();
        }
    }
}
