namespace WebApi.LoadTesting.Models;

internal sealed record RequestResult(
    LoadTestOperation Operation,
    double Milliseconds,
    bool Success,
    string Status
);
