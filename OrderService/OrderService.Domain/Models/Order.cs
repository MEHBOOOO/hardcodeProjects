namespace OrderService.Domain.Models
{
    public class Order
    {
        public int Id { get; set; }
        public required string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public required string Status { get; set; }

        public Order() { }

        public Order(string productName, int quantity, decimal price, decimal discount)
        {
            ProductName = productName;
            Quantity = quantity;
            Price = price;
        }
    }
}