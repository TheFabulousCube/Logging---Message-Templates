using System.Diagnostics;
using Amazon.CloudWatchLogs;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.AwsCloudWatch;

class Program
{
    static void Main(string[] args)
    {
        var cloudWatchClient = new AmazonCloudWatchLogsClient();
        var options = new CloudWatchSinkOptions
        {
            LogGroupName = "MessageTemplateTest"
            LogStreamNameProvider = new DefaultLogStreamProvider(),
            TextFormatter = new Serilog.Formatting.Json.JsonFormatter()
        };

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.AmazonCloudWatch(options, cloudWatchClient)
            .CreateLogger();

        ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddSerilog();
        });

        // Create loggers with different SourceContext values
        Microsoft.Extensions.Logging.ILogger generalLogger = loggerFactory.CreateLogger("General");
        Microsoft.Extensions.Logging.ILogger colorsAndShapesLogger = loggerFactory.CreateLogger("Colors and Shapes");

        generalLogger.LogInformation("Application started.");

        Console.WriteLine("Hello, World!");

        string[] colors = new string[] { "Red", "Green", "Blue", "Yellow", "Orange", "Purple", "Pink", "Brown", "Black", "White" };
        string[] shapes = new string[] { "Square", "Circle", "Triangle", "Rectangle", "Pentagon", "Hexagon", "Octagon", "Star", "Heart", "Diamond" };

        var stopwatch = new Stopwatch();

        // Measure structured logging
        stopwatch.Start();
        foreach (string color in colors)
        {
            foreach (string shape in shapes)
            {
                colorsAndShapesLogger.LogInformation("A {Color} {Shape}", color, shape);
            }
        }
        stopwatch.Stop();
        Console.WriteLine($"Structured logging took: {stopwatch.ElapsedMilliseconds} ms");

        // Measure interpolated string logging
        stopwatch.Restart();
        foreach (string color in colors)
        {
            foreach (string shape in shapes)
            {
                colorsAndShapesLogger.LogInformation($"A {color} {shape}");
            }
        }
        stopwatch.Stop();
        Console.WriteLine($"Interpolated string logging took: {stopwatch.ElapsedMilliseconds} ms");

        generalLogger.LogInformation("Application ended.");

        Log.CloseAndFlush();
    }
}
