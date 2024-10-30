namespace OrderService.DataTransferObjects
{
    public record OrderCreateDTO(string ProductName, int Quantity, decimal Price);
}
