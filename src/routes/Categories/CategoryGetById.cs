using IWantApp.Database;
using Microsoft.AspNetCore.Mvc;

namespace IWantApp.Domain.Products;

public class CategoryGetById
{
  public static string Template => "/categories/{id}";
  public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
  public static Delegate Handler => Action;
  public static IResult Action([FromRoute] Guid id, ApplicationDbContext context)
  {
    var category = context.Categories.Where(c => c.Id == id).FirstOrDefault();

    if (category == null)
    {
      return Results.NotFound("Categoria não encontrada");
    }

    return Results.Ok(category);
  }
}