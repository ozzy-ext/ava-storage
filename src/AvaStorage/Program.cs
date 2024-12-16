using System.Net;
using AvaStorage;
using AvaStorage.Application;
using AvaStorage.ByteArrayFormatting;
using AvaStorage.Infrastructure.ImageSharp;
using AvaStorage.Infrastructure.LocalDisk;
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
builder.Services.AddLocalDiscPictureStorage();
builder.Services.ConfigureAvaServiceLogic(builder.Configuration, "AvaStorage");
builder.Services.ConfigureLocalDiscPictureStorage(builder.Configuration, "AvaStorage");

builder.WebHost.ConfigureKestrel((ctx, opt) =>
{
    opt.Listen(IPAddress.Any, ListenConstants.PublicPort);
    opt.Listen(IPAddress.Any, ListenConstants.AdminPort);
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program{};