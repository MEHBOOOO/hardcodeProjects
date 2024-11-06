using MassTransit;
using SharedEvents;
using DeliveryService.Data;
using DeliveryService.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DeliveryService.Consumers
{
    public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly DeliveryContext _context;
        private readonly ILogger<OrderCreatedConsumer> _logger;

        public OrderCreatedConsumer(DeliveryContext context, ILogger<OrderCreatedConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var cancellationToken = context.CancellationToken;

            try
            {
                var order = context.Message;

                var deliveryRequest = new DeliveryRequest
                {
                    OrderId = order.OrderId,
                    Status = "testPending2"
                };

                _context.DeliveryRequests.Add(deliveryRequest);
                await _context.SaveChangesAsync(cancellationToken);
                // throw new Exception("error test");

                // System.Console.WriteLine($"Delivery request created for order {order.OrderId}");
                
                _logger.LogInformation($"Delivery request created for order {order.OrderId}");

            }
            catch (Exception ex)
            {
                //ошибка
                // System.Console.WriteLine($"Error processing order event: {context.Message.OrderId}, {ex.Message}");

                _logger.LogError(ex, $"Error processing order event: {context.Message.OrderId}");
            }
        }
    }
}