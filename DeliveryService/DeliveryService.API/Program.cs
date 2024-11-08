using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Services;
using OrderService.Domain.Interfaces;
using OrderService.Infrastructure.Data;
using OrderService.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using OrderService.Application;
using OrderService.Infrastructure;
using AutoMapper;
using DeliveryService.Application.Consumers;

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

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddMassTransitWithRabbitMq();

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

// using MassTransit;
// using Microsoft.EntityFrameworkCore;
// using DeliveryService.Data;
// using DeliveryService.Consumers;
// using SharedEvents;
// using Microsoft.Extensions.Logging;
// using DeliveryService.Extensions;

// var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddDbContext<DeliveryContext>(options =>
//     options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// builder.Services.AddControllers();

// builder.Services.AddMassTransitWithRabbitMq();
// builder.Services.AddLogging(loggingBuilder =>
// {
//     loggingBuilder.AddConsole();
//     loggingBuilder.AddDebug();
// });
// // builder.Services.AddMassTransit(x =>
// // {
// //     x.AddConsumer<OrderCreatedConsumer>();

// //     x.UsingRabbitMq((context, cfg) =>
// //     {
// //         cfg.Host("localhost", "/", h =>
// //         {
// //             h.Username("guest");
// //             h.Password("guest");
// //         });

// //         cfg.ReceiveEndpoint("order-created-event", e =>
// //         {
// //             e.ConfigureConsumer<OrderCreatedConsumer>(context);
// //         });
// //     });
// // });

// var app = builder.Build();

// if (app.Environment.IsDevelopment())
// {
//     app.UseDeveloperExceptionPage();
// }

// // app.UseHttpsRedirection();
// app.MapControllers();

// app.Run();