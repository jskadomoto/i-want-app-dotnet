namespace IWantApp.Domain.Products;

public class CategoryGetAll
{
  public static string Template => "/categories";
  public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
  public static Delegate Handler => Action;
  public static async Task<IResult> Action(ApplicationDbContext context)
  {
    var categories = await context.Categories.ToListAsync();

    if (categories.Count == 0)
    {
      return Results.NotFound("Nenhuma categoria encontrada");
    }

    var response = categories.Select(c => new CategoryResponse(c.Id, c.Name, c.IsActive));
    return Results.Ok(response);
  }
}