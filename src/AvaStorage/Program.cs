using System.Net;
using AvaStorage;
using AvaStorage.Application;
using AvaStorage.ByteArrayFormatting;
using AvaStorage.Infrastructure.ImageSharp;
using MyLab.WebErrors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(c =>
{
    c.InputFormatters.Add(new ByteArrayInputFormatter());
    c.OutputFormatters.Add(new ByteArrayOutputFormatter());
    c.AddExceptionProcessing();
});
builder.Services.AddAvaServiceLogic();
builder.Services.AddAvaStorageImageSharpInfrastructure();
builder.Services.ConfigureAvaServiceLogic(builder.Configuration);

builder.WebHost.ConfigureKestrel((ctx, opt) =>
{
    opt.Listen(IPAddress.Loopback, ListenConstants.PublicPort);
    opt.Listen(IPAddress.Loopback, ListenConstants.AdminPort);
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program{};