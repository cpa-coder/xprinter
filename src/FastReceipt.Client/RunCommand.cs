using System.Diagnostics;
using System.Drawing.Printing;
using Microsoft.AspNetCore.SignalR.Client;
using Serilog;
using Spectre.Console;
using Spectre.Console.Cli;

namespace FastReceipt.Client;

public sealed class RunCommand : AsyncCommand<Settings>, IAsyncDisposable
{
    private HubConnection? _connection;

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var machineName = Environment.MachineName;
        var url = GetServerUrl();
        var connectedFunc = string.IsNullOrEmpty(settings.Connected) ? "PrinterConnected" : settings.Connected;
        var printFunc = string.IsNullOrEmpty(settings.Print) ? "print-receipt" : settings.Print;

        var initialMsg = $"{machineName} is starting connection on {url}";
        Log.Information("{Message}", initialMsg);
        AnsiConsole.MarkupLineInterpolated($"[blue]{initialMsg}[/]");

        _connection = new HubConnectionBuilder()
            .WithUrl(url)
            .WithAutomaticReconnect()
            .Build();

        _connection.Reconnected += _ =>
        {
            AnsiConsole.MarkupLine("[green1]Reconnected[/]");
            return _connection.SendAsync(connectedFunc, _connection.ConnectionId, GetInstalledPrinters(), machineName);
        };

        _connection.Closed += _ =>
        {
            AnsiConsole.MarkupLine("[red]Connection closed[/]");
            return Task.CompletedTask;
        };

        _connection.On<byte[], string>(printFunc, async (data, printerName) =>
        {
            await StartPrintingProcessAsync(printerName, data);
        });

        try
        {
            await _connection.StartAsync();
        }
        catch (Exception e)
        {
            Log.Error(e, "An error occured while starting the connection");
            AnsiConsole.WriteLine(e.ToString());
            Environment.Exit(1);
            return 1;
        }

        await _connection.SendAsync(connectedFunc, _connection.ConnectionId, GetInstalledPrinters(), machineName);
        AnsiConsole.WriteLine($"Connected to {url} with connection id: {_connection.ConnectionId}");
        return 0;
    }

    private static List<string> GetInstalledPrinters()
    {
        var printers = new List<string>();
        for (var i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
        {
            var printer = PrinterSettings.InstalledPrinters[i];
            printers.Add(printer);
        }
        return printers;
    }

    private static string GetServerUrl()
    {
        var envUrl = Environment.GetEnvironmentVariable("FASTRECEIPT_URL");
        return string.IsNullOrEmpty(envUrl) ? "http://localhost:5000/notification-hub" : envUrl;
    }

    private static async Task StartPrintingProcessAsync(string printer, byte[] data)
    {
        var pdf = $"{Guid.NewGuid().ToString()}.pdf";
        var temp = $"{Path.Combine(Path.GetTempPath(), "FastReceipt")}";

        DeleteOldFiles(temp);

        var pdfPath = Path.Combine(temp, pdf);
        await File.WriteAllBytesAsync(pdfPath, data);

        var startInfo = new ProcessStartInfo
        {
            WindowStyle = ProcessWindowStyle.Hidden,
            RedirectStandardOutput = true,
            FileName = "printer",
            CreateNoWindow = true
        };
        startInfo.ArgumentList.Add($"{pdfPath}");
        startInfo.ArgumentList.Add($"{printer}");

        var process = new Process { EnableRaisingEvents = true, StartInfo = startInfo };
        process.Start();
        Extensions.OpenDrawer(printer);
    }

    private static void DeleteOldFiles(string temp)
    {
        try
        {
            Directory.CreateDirectory(temp);
            var dirInfo = new DirectoryInfo(temp);
            foreach (var file in dirInfo.GetFiles())
                if (file.Name.Contains("pdf"))
                    file.Delete();
        }
        catch (Exception)
        {
            //ignore
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection != null) await _connection.DisposeAsync();
    }
}