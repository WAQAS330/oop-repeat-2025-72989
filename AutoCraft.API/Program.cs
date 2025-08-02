using AutoCraft.API.Handlers;
using AutoCraft.API.Interfaces;
using AutoCraft.Domain;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// we are adding the controllers to the builder services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var provider = builder.Services.BuildServiceProvider();
var config = provider.GetRequiredService<IConfiguration>();

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IExceptionHandler, DatabaseExceptionHandler>();
builder.Services.AddScoped<IExceptionHandler, ValidationExceptionHandler>();
builder.Services.AddScoped<IExceptionHandler, NotFoundExceptionHandler>();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var app = builder.Build();

// we are adding the swagger to the app
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
