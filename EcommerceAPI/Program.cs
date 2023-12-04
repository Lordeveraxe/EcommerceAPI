using EcommerceAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PayPal; // Add this line if there's a specific namespace for PayPal SDK

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<EcommerceContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    Microsoft.EntityFrameworkCore.ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

// PayPal Settings
var paypalSettings = builder.Configuration.GetSection("PayPalSettings").Get<PayPalSettings>();
builder.Services.AddSingleton(paypalSettings);

// Add PayPal service
builder.Services.AddScoped<IPayPalService, PayPalService>();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();