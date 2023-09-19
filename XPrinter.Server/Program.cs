using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapHub<ServerHub>("notification-hub");
app.MapGet("/", async (IHubContext<ServerHub> hub) =>
{
    await hub.Clients.All.SendAsync("hello", "Hello World!");
    return "Hello World!";
});

app.Run();


public sealed class ServerHub : Hub
{
    public Task Connected(string connectionId, string printerName)
    {
        Console.WriteLine($"Connected: {connectionId} - {printerName}");
        return Task.CompletedTask;
    }
}