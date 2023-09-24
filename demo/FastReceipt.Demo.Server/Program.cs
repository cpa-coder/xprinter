using FastReceipt.Demo.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;

Settings.License = LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<PrinterHub>();
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
app.MapGet("api/printers", (PrinterHub hub) => hub.Printers);
app.MapPost("api/print", async ([FromQuery]string printer, [FromQuery]string connection, IHubContext<ServerHub> hub) =>
{
    var receipt = new Receipt();
    receipt.GeneratePdf();

     await hub.Clients.Client(connection!).SendAsync("print-receipt", receipt.GeneratePdf(), printer);
    return "Hello World!";
});

app.Run();