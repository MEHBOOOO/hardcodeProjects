using MassTransit;
using Microsoft.Extensions.Logging;
using OrderService.Domain.Events;
using OrderService.Domain.Interfaces;
using System.Threading.Tasks;

namespace OrderService.Application.Handlers
{
    public class OrderCreatedHandler : IConsumer<OrderCreatedEvent>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrderCreatedHandler> _logger;

        public OrderCreatedHandler(IOrderRepository orderRepository, ILogger<OrderCreatedHandler> logger)
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