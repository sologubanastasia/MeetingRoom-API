using WebApi.LoadTesting.Configuration;
using WebApi.LoadTesting.Metrics;

namespace WebApi.LoadTesting.Output;

internal sealed class ConsoleReportWriter
{
    public void WriteHeader(LoadTestOptions options)
    {
        Console.WriteLine($"Load test: {options.Requests} tasks / {options.Concurrency} concurrent requests");
        Console.WriteLine($"Target: {options.Url.TrimEnd('/')}/\n");
    }

    public void WriteResults(MetricsSnapshot snapshot, TimeSpan elapsed, LoadTestOptions options)
    {
        Console.WriteLine("Results");
        Console.WriteLine($"Total time:       {elapsed.TotalSeconds:F3} s");
        Console.WriteLine($"Average response: {snapshot.Average:F2} ms");
        Console.WriteLine($"Minimum response: {snapshot.Minimum:F2} ms");
        Console.WriteLine($"Maximum response: {snapshot.Maximum:F2} ms");
        Console.WriteLine($"Throughput:       {snapshot.Count / elapsed.TotalSeconds:F2} req/s");
        Console.WriteLine($"Successful:       {snapshot.Successful}");
        Console.WriteLine($"Failed:           {snapshot.Failed}\n");

        foreach (var operation in snapshot.Operations.OrderBy(pair => pair.Key))
            Console.WriteLine($"{operation.Key,-14} count={operation.Value.Count,10} " +
                $"ok={operation.Value.Successful,10} avg={operation.Value.Average,8:F2} ms");

        foreach (var error in snapshot.Errors.OrderByDescending(pair => pair.Value).Take(10))
            Console.WriteLine($"ERROR {error.Value,10} x {error.Key}");

        Console.WriteLine($"\nConfiguration: {options.Requests} tasks / " +
            $"{options.Concurrency} concurrent requests");
    }
}
