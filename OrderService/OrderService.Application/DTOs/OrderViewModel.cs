// namespace OrderService.DataTransferObjects
// {
//     public record OrderViewModel(int Id, string ProductName, int Quantity, decimal Price);
// }

namespace OrderService.Application.DTOs
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public required string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}