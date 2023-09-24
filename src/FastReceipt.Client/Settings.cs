using System.ComponentModel;
using Spectre.Console.Cli;

namespace FastReceipt.Client;

public sealed class Settings : CommandSettings
{
    [CommandOption("-p|--print")]
    [Description("Function name to print from server")]
    [DefaultValue("print-receipt")]
    public string Print { get; set; } = string.Empty;

    [CommandOption("-c|--connected")]
    [Description("Function name to call when printer is connection to the server.")]
    [DefaultValue("PrinterConnected")]
    public string Connected { get; set; } = string.Empty;
}