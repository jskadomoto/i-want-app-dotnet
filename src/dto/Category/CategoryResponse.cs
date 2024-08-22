namespace IWantApp.Domain.Products;
public class CategoryResponse
{
  public Guid Id { get; set; }
  public required string Name { get; set; }
  public required bool IsActive { get; set; }
}