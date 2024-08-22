using IWantApp.Database;
using Microsoft.AspNetCore.Mvc;

namespace IWantApp.Domain.Products;

public class CategoryDelete
{
  public static string Template => "/categories/{id:guid}";
  public static string[] Methods => new string[] { HttpMethod.Delete.ToString() };
  public static Delegate Handler => Action;
  public static IResult Action([FromRoute] Guid id, ApplicationDbContext context)
  {
    var category = context.Categories.Where(c => c.Id == id).FirstOrDefault();
    if (category == null)
    {
      return Results.NotFound("Categoria não encontrada.");
    }
    context.Categories.Remove(category);
    context.SaveChanges();
    return Results.Ok($"Categoria Id: {category.Id}, Nome: {category.Name} deletada com sucesso.");
  }
}