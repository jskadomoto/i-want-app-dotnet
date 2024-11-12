namespace IWantApp.Domain.Products;

public class Product : Entity
{
  public string Name { get; set; }
  public Guid CategoryId { get; set; }
  public Category Category { get; set; }
  public string Description { get; set; }
  public bool HasStock { get; set; }
  public bool IsActive { get; set; } = true;

  private Product() { }

  public Product(string name, Category category, string description, bool hasStock, string createdBy)
  {
    Name = name;
    Category = category;
    Description = description;
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
      .IsNotNullOrEmpty(CreatedBy, "CreatedBy")
      .IsNotNullOrEmpty(UpdatedBy, "UpdatedBy");

    AddNotifications(contract);
  }
}
