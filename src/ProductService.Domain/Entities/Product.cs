namespace ProductService.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }

    private Product() { }

    public Product(string name, decimal price)
    {
        Id = Guid.NewGuid();
        Name = name;
        Price = price;
    }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Name cannot be empty");

        Name = newName.Trim();
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new ArgumentException("Price must be positive");

        Price = newPrice;
    }
}
