using System.CommandLine;
using Microsoft.AspNetCore.SignalR.Client;

namespace XPrinter.Client;

public class WorkService : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var printerOption = new Option<string>("--printer", "Printer name");
        var serverOption = new Option<string>("--server", "Server name or IP address ");
        var command = new RootCommand();
        command.AddOption(printerOption);
        command.AddOption(serverOption);

        command.SetHandler(async (printer, server)=>
        {
            Console.WriteLine(printer);
            Console.WriteLine(server);
            
            var connection = new HubConnectionBuilder()
                .WithUrl($"https://localhost:7025/notification-hub")
                .Build();

            connection.On<string>("hello", message =>
            {
                Console.WriteLine(message);
            });
            
            await connection.StartAsync(cancellationToken);

            await connection.SendAsync("Connected", connection.ConnectionId, printer, cancellationToken: cancellationToken);
            
            Console.WriteLine("Connected to server");

        }, printerOption, serverOption);

        await command.InvokeAsync(Environment.GetCommandLineArgs()[1..]);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}