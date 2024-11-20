namespace OrderService.Application.DTOs
{
    public record OrderUpdateDTO(string ProductName, int Quantity, decimal Price);
}