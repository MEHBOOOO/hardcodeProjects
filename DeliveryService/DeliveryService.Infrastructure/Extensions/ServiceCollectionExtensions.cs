using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using DeliveryService.Application.Consumers;

namespace DeliveryService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMassTransitWithRabbitMq(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<OrderCreatedConsumer>(c =>
                {
                    c.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                });

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ReceiveEndpoint("order-created-event", e =>
                    {
                        e.ConfigureConsumer<OrderCreatedConsumer>(context);
                    });
                });
            });

            return services;
        }
    }
}