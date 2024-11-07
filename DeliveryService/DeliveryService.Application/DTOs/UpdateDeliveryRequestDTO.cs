namespace DeliveryService.Application.DTOs
{
    public class UpdateDeliveryRequestDTO
    {
        public int OrderId { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}