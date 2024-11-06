using MassTransit;
using SharedEvents;
using OrderService.Data;
using OrderService.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace OrderService.Consumers
{
    public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly OrderContext _context;
        private readonly ILogger<OrderCreatedConsumer> _logger;

        public OrderCreatedConsumer(OrderContext context, ILogger<OrderCreatedConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var cancellationToken = context.CancellationToken;

            try
            {
                var orderCreatedEvent = context.Message;

                var order = await _context.Orders.FindAsync(new object[] { orderCreatedEvent.OrderId }, cancellationToken);
                if (order == null)
                {
                    _logger.LogWarning($"Order not found for order ID: {orderCreatedEvent.OrderId}");
                    return;
                }

                order.Status = orderCreatedEvent.NewStatus;
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation($"Order status updated for order ID: {orderCreatedEvent.OrderId} to {orderCreatedEvent.NewStatus}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing order status update event: {context.Message.OrderId}");
            }
        }
    }
}