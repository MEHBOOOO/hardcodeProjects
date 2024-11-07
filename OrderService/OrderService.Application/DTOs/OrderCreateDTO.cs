namespace OrderService.Application.DTOs
{
    public record OrderCreateDTO(string ProductName, int Quantity, decimal Price);
}