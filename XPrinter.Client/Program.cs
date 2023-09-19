using XPrinter.Client;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<WorkService>();

var host = builder.Build();
await host.RunAsync();