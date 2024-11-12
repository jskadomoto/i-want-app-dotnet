namespace IWantApp.Domain.Products;

public class Product : Entity
{
  public string Name { get; set; }
  public Guid CategoryId { get; set; }
  public Category Category { get; set; }
  public string Description { get; set; }
  public bool HasStock { get; set; }
  public bool IsActive { get; set; } = true;

  public decimal Price { get; private set; }

  private Product() { }

  public Product(string name, Category category, string description, bool hasStock, decimal price, string createdBy)
  {
    Name = name;
    Category = category;
    Description = description;
    Price = price;
    HasStock = hasStock;

    CreatedBy = createdBy;
    UpdatedBy = createdBy;
    CreatedAt = DateTime.Now;
    UpdatedAt = DateTime.Now;

    Validate();
  }

  private void Validate()
  {
    var contract = new Contract<Product>()
      .IsNotNullOrEmpty(Name, "Name")
      .IsGreaterOrEqualsThan(Name, 3, "Name")
      .IsNotNull(Category, "Category not found")
      .IsNotNullOrEmpty(Description, "Description")
      .IsGreaterOrEqualsThan(Description, 3, "Description")
      .IsGreaterOrEqualsThan(Price, 0.1, "Price")
      .IsNotNullOrEmpty(CreatedBy, "CreatedBy")
      .IsNotNullOrEmpty(UpdatedBy, "UpdatedBy");

    AddNotifications(contract);
  }
}
