namespace OrderService.DataTransferObjects
{
    public record OrderUpdateDTO(string ProductName, int Quantity, decimal Price);
}
