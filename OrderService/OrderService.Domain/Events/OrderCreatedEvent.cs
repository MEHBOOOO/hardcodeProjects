namespace OrderService.Domain.Events
{
    public class OrderCreatedEvent
    {
        public int OrderId { get; set; }
        public required string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public required string NewStatus { get; set; }

    }
}