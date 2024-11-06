namespace DeliveryService.DataTransferObjects
{
    public class CreateDeliveryRequestDTO
    {
        public int OrderId { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}