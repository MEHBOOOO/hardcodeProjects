using MassTransit;
using SharedEvents;
using DeliveryService.Data;
using DeliveryService.Models;
using System.Threading.Tasks;

namespace DeliveryService.Consumers
{
    public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly DeliveryContext _context;

        public OrderCreatedConsumer(DeliveryContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            try
            {
                var order = context.Message;

                var deliveryRequest = new DeliveryRequest
                {
                    OrderId = order.OrderId,
                    Status = "Pending"
                };

                _context.DeliveryRequests.Add(deliveryRequest);
                await _context.SaveChangesAsync();
                // throw new Exception("error test");

                System.Console.WriteLine($"Delivery request created for order {order.OrderId}");
            }
            catch (Exception ex)
            {
                //ошибка
                System.Console.WriteLine($"Error processing order event: {context.Message.OrderId}, {ex.Message}");
            }
        }
    }
}