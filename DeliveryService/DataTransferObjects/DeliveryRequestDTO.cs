namespace DeliveryService.DataTransferObjects
{
    public class DeliveryRequestDTO
    {
        public int OrderId { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}