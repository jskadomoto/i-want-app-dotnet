using IWantApp.Database;

namespace IWantApp.Domain.Products;

public class CategoryGetAll
{
  public static string Template => "/categories";
  public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
  public static Delegate Handler => Action;
  public static IResult Action(ApplicationDbContext context)
  {
    var categories = context.Categories.ToList();

    if (categories.Count == 0)
    {
      return Results.NotFound("Nenhuma categoria encontrada");
    }

    var response = categories.Select(c => new CategoryResponse { Id = c.Id, Name = c.Name, IsActive = c.IsActive });
    return Results.Ok(response);
  }
}