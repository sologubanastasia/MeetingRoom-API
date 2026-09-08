namespace WebApi.LoadTesting.Configuration;

internal static class CommandLineOptionsParser
{
    public static LoadTestOptions Parse(string[] args)
    {
        ArgumentNullException.ThrowIfNull(args);
        if (args.Contains("--help", StringComparer.OrdinalIgnoreCase))
        {
            PrintHelp();
            Environment.Exit(0);
        }

        return new(
            GetValue(args, "--url", "http://localhost:5119"),
            GetPositiveInteger(args, "--requests", 1000),
            GetPositiveInteger(args, "--concurrency", 10),
            GetPositiveInteger(args, "--timeout", 30)
        );
    }

    private static string GetValue(string[] args, string key, string defaultValue)
    {
        var index = Array.FindIndex(args,
            argument => string.Equals(argument, key, StringComparison.OrdinalIgnoreCase));
        if (index < 0) return defaultValue;
        if (index + 1 >= args.Length || args[index + 1].StartsWith("--", StringComparison.Ordinal))
            throw new ArgumentException($"A value is required for {key}.");
        return args[index + 1];
    }

    private static int GetPositiveInteger(string[] args, string key, int defaultValue)
    {
        var rawValue = GetValue(args, key, defaultValue.ToString());
        if (!int.TryParse(rawValue, out var value) || value <= 0)
            throw new ArgumentException($"{key} must be a positive integer.");
        return value;
    }

    private static void PrintHelp()
    {
        Console.WriteLine("Usage: dotnet run -c Release -- [options]");
        Console.WriteLine("--url <url> --requests <number> --concurrency <number> --timeout <seconds>");
    }
}
