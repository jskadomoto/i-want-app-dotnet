namespace IWantApp.Domain.Products;

public class ProductGetShowCase
{
  public static string Template => "/products/showcase";
  public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
  public static Delegate Handler => Action;

  [AllowAnonymous]
  public static async Task<IResult> Action(ApplicationDbContext context)
  {
    var products = context.Products.Include(p => p.Category).OrderBy(p => p.Name).ToList();
    var response = products.Select(p => new ProductResponse(p.Id, p.Name, p.Category.Name, p.Description, p.HasStock, p.Price, p.IsActive));

    return Results.Ok(response);
  }
}