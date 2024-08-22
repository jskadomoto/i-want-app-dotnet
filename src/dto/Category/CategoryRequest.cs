namespace IWantApp.Domain.Products;
public class CategoryRequest
{
  public required string Name { get; set; }
  public bool IsActive { get; set; }
}