using AvaStorage.Application;
using AvaStorage.ByteArrayFormatting;
using AvaStorage.Infrastructure.ImageSharp;
using AvaStorage.Infrastructure.LocalDisk;
using AvaStorage.Middlewares;
using MyLab.HttpMetrics;
using MyLab.Log;
using MyLab.WebErrors;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(c =>
{
    c.InputFormatters.Add(new ByteArrayInputFormatter());
    c.OutputFormatters.Add(new ByteArrayOutputFormatter());
    c.AddExceptionProcessing();
});
builder.Services
    .AddLogging(b => b.AddMyLabConsole())
    .AddUrlBasedHttpMetrics()
    .AddAvaServiceLogic()
    .AddAvaStorageImageSharpInfrastructure()
    .AddLocalDiscPictureStorage()
    .ConfigureAvaServiceLogic(builder.Configuration, "AvaStorage")
    .ConfigureLocalDiscPictureStorage(builder.Configuration, "AvaStorage");

builder.WebHost.ConfigureKestrel((ctx, opt) =>
{
    opt.AddServerHeader = false;
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMiddleware<AddRequestInfoHeaderMiddleware>();
app.UseMiddleware<AddSenderHeaderMiddleware>();

app.UseUrlBasedHttpMetrics();

app.UseAuthorization();

app.MapControllers();
app.MapMetrics();

app.Run();

public partial class Program{};