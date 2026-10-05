using Microsoft.EntityFrameworkCore;
using OjitexSystem_Backend.Data.Production;
using OjitexSystem_Backend.Repositories;
using OjitexSystem_Backend.Repositories.Interfaces;
using OjitexSystem_Backend.Services;
using OjitexSystem_Backend.Services.Interfaces;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ProductionContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OJITEXHP")));

builder.Services.AddScoped<ICurrentStockRepository, CurrentStockRepository>();
builder.Services.AddScoped<ICurrentStockService, CurrentStockService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
