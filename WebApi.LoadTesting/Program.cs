using System.Text.Json;
using WebApi.LoadTesting.Clients;
using WebApi.LoadTesting.Configuration;
using WebApi.LoadTesting.Managers;
using WebApi.LoadTesting.Output;
using WebApi.LoadTesting.Requests;

var options = CommandLineOptionsParser.Parse(args);
var reportWriter = new ConsoleReportWriter();
reportWriter.WriteHeader(options);

using var handler = new SocketsHttpHandler
{
    MaxConnectionsPerServer = options.Concurrency,
};
using var httpClient = new HttpClient(handler)
{
    BaseAddress = new Uri(options.Url.TrimEnd('/') + "/"),
    Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds),
};

var requestFactory = new LoadTestRequestFactory();
var apiClient = new MeetingRoomApiClient(httpClient, requestFactory);
var manager = new LoadTestManager(apiClient);

try
{
    var summary = await manager.RunAsync(options);
    reportWriter.WriteResults(summary.Metrics, summary.Elapsed, options);
    return summary.Metrics.Failed > 0 ? 2 : 0;
}
catch (Exception exception) when (
    exception is HttpRequestException or TaskCanceledException or JsonException
)
{
    Console.Error.WriteLine($"The load test could not start: {exception.Message}");
    return 1;
}
