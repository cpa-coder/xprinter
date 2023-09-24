using FastReceipt.Client;
using Serilog;
using Serilog.Events;

var path = AppDomain.CurrentDomain.BaseDirectory;
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.File(Path.Combine(path, "service-log.txt"))
    .CreateLogger();

try
{
    Log.Information("Fast receipt is starting...");
    var builder = Host.CreateDefaultBuilder(args)
        .UseWindowsService()
        .ConfigureServices(s => s.AddHostedService<WorkService>())
        .UseSerilog();

    var host = builder.Build();
    await host.RunAsync();
}
catch (Exception e)
{
    Log.Fatal(e, "Fatal error occured");
}
finally
{
    Log.CloseAndFlush();
}