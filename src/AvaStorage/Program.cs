using AvaStorage.Application;
using AvaStorage.ByteArrayFormatting;
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
builder.Services.ConfigureAvaServiceLogic(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program{};