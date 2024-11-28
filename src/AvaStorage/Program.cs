using AvaStorage.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<ApplicationLibAnchor>());

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
