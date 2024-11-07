namespace DeliveryService.Domain.Events
{
    public class DeliveryCreatedEvent
    {
        public int DeliveryRequestId { get; set; }
        public int OrderId { get; set; }
        public required string Status { get; set; }
    }
}