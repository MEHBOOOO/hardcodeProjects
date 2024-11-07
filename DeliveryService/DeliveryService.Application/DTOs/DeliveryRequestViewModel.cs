namespace DeliveryService.Application.DTOs
{
    public class DeliveryRequestViewModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Status { get; set; } = string.Empty;
        public required string Address { get; set; }

    }
}