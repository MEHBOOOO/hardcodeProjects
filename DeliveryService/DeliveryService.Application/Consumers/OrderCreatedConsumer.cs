using MassTransit;
using Microsoft.Extensions.Logging;
using OrderService.Domain.Events;
using OrderService.Domain.Interfaces;
using System.Threading.Tasks;

namespace DeliveryService.Application.Consumers
{
    public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrderCreatedConsumer> _logger;

        public OrderCreatedConsumer(IOrderRepository orderRepository, ILogger<OrderCreatedConsumer> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var cancellationToken = context.CancellationToken;

            try
            {
                var orderCreatedEvent = context.Message;

                var order = await _orderRepository.GetByIdAsync(orderCreatedEvent.OrderId, cancellationToken);
                if (order == null)
                {
                    _logger.LogWarning($"Order not found for order ID: {orderCreatedEvent.OrderId}");
                    return;
                }

                order.Status = orderCreatedEvent.NewStatus;
                await _orderRepository.UpdateAsync(order, cancellationToken);

                _logger.LogInformation($"Order status updated for order ID: {orderCreatedEvent.OrderId} to {orderCreatedEvent.NewStatus}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing order status update event: {context.Message.OrderId}");
            }
        }
    }
}