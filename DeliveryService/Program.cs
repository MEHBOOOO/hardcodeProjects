using MassTransit;
using Microsoft.EntityFrameworkCore;
using DeliveryService.Data;
using DeliveryService.Consumers;
using SharedEvents;
using Microsoft.Extensions.Logging;
using DeliveryService.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DeliveryContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

builder.Services.AddMassTransitWithRabbitMq();
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
});
// builder.Services.AddMassTransit(x =>
// {
//     x.AddConsumer<OrderCreatedConsumer>();

//     x.UsingRabbitMq((context, cfg) =>
//     {
//         cfg.Host("localhost", "/", h =>
//         {
//             h.Username("guest");
//             h.Password("guest");
//         });

//         cfg.ReceiveEndpoint("order-created-event", e =>
//         {
//             e.ConfigureConsumer<OrderCreatedConsumer>(context);
//         });
//     });
// });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// app.UseHttpsRedirection();
app.MapControllers();

app.Run();