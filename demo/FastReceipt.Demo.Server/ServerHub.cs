using Microsoft.AspNetCore.SignalR;

namespace FastReceipt.Demo.Server;

public sealed class ServerHub : Hub
{
    private readonly PrinterHub _printerHub;

    public ServerHub(PrinterHub printerHub)
    {
        _printerHub = printerHub;
    }
    
    public Task PrinterConnected(string connectionId, string[] printers, string device)
    {
        Console.WriteLine($"Connected: {connectionId} - {device}");
        foreach (var printer in printers)
        {
            Console.WriteLine(printer);
        }

        var existing = _printerHub.Printers.FirstOrDefault(x => x.Name == device);
        if (existing is not null)
        {
            _printerHub.Printers.Remove(existing);
        }
      
        _printerHub.Printers.Add(new ClientPrinter
        {
            Id = connectionId,
            Name = device,
            InstalledPrinters = printers
        });

        return Task.CompletedTask;
    }
}

public class PrinterHub
{
    public List<ClientPrinter> Printers { get; } = new();
}

public class ClientPrinter
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public IEnumerable<string> InstalledPrinters { get; set; } = Array.Empty<string>();
}