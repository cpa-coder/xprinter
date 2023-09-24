using Serilog;
using Spectre.Console.Cli;

namespace FastReceipt.Client;

public class WorkService : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var command = new CommandApp();
        command.Configure(config =>
        {
            config.AddCommand<RunCommand>("run")
                .WithDescription("Run the client");
        });

       var result = await command.RunAsync(Environment.GetCommandLineArgs()[1..]);
       if (result != 0)
       {
           Log.Error("An error occured while running the client");
           Environment.Exit(result);
       }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}