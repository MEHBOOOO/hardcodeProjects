namespace DeliveryService.Domain.Models
{
    public class DeliveryRequest
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public required string Status { get; set; }
        public required string Address { get; set; }

        public DeliveryRequest() { }

        public DeliveryRequest(int orderId, string status)
        {
            OrderId = orderId;
            Status = status;
        }
    }
}