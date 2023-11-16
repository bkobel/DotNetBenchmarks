using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Logging;

namespace PartialVsLogger;

[MemoryDiagnoser]
public class LoggingBenchmark
{
    private const int ExecTimes = 100 * 1000 * 1000;

    private static readonly ILogger Logger = LoggerFactory.Create(builder => builder.AddDebug()).CreateLogger("Default");
    
    [Benchmark]
    public void LogSimple()
    {
        for (var i = 0; i < ExecTimes; i++)
        {
            var count = 10;
            var address = 123123123L;
            var message = "Moving";

            Logger.LogInformation("{Message} {Count} items from {Address}", message, count, address);
        }
    }

    [Benchmark]
    public void LogViaPartialClass()
    {
        for (var i = 0; i < ExecTimes; i++)
        {
            var count = 10;
            var address = 123123123L;
            var message = "Moving";

            Logger.LogDelivery(message, count, address);
        }
    }
}

public static partial class ConsoleLogger
{
    [LoggerMessage(LogLevel.Information, "{Message} {Count} items from {Address}")]
    public static partial void LogDelivery(this ILogger logger, string message, int count, long address);
}