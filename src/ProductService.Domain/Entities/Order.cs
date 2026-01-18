namespace ProductService.Domain.Entities;

public class Order
{
    public Order(Guid orderId, string productName, decimal amount, DateTime createdAt)
    {
        OrderId = orderId;
        ProductName = productName;
        Amount = amount;
        CreatedAt = createdAt;
    }

    public Guid OrderId { get; set; }
    public string ProductName { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
}
