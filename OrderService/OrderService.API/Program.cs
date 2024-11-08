// using MassTransit;
// using Microsoft.EntityFrameworkCore;
// using OrderService.Application.Consumers;
// using OrderService.Application.Services;
// using OrderService.Domain.Interfaces;
// using OrderService.Infrastructure.Data;
// using OrderService.Infrastructure.Extensions;
// using Microsoft.Extensions.Logging;

// var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddControllers();
// builder.Services.ConfigureDbContext(builder.Configuration);

// builder.Services.AddScoped<IOrderRepository, OrderRepository>();
// builder.Services.AddScoped<OrderService>();

// builder.Services.AddMassTransitWithRabbitMq();

// builder.Services.AddLogging(loggingBuilder =>
// {
//     loggingBuilder.AddConsole();
//     loggingBuilder.AddDebug();
// });

// var app = builder.Build();

// if (app.Environment.IsDevelopment())
// {
//     app.UseDeveloperExceptionPage();
// }

// app.UseRouting();
// app.UseAuthorization();
// app.MapControllers();

// app.Run();

using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Services;
using OrderService.Domain.Interfaces;
using OrderService.Infrastructure.Data;
using OrderService.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using OrderService.Application.Mapping;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

builder.Services.AddControllers();

builder.Services.AddDbContext<OrderContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<OrderAppService>();

builder.Services.AddMassTransitWithRabbitMq();

builder.Services.AddAutoMapper(typeof(OrderProfile));

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();