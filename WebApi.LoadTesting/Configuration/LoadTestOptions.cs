namespace WebApi.LoadTesting.Configuration;

internal sealed record LoadTestOptions(string Url, int Requests, int Concurrency, int TimeoutSeconds);
