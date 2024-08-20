namespace IWantApp.Domain.Products;

public class Product : Entity
{
  public required string Name { get; set; }
  public required Category Category { get; set; }
  public string? Description { get; set; }
  public bool HasStock { get; set; }
}
