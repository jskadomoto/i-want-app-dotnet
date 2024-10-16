using IWantApp.Database;
using Microsoft.AspNetCore.Authorization;

namespace IWantApp.Domain.Products;

public class CategoryPost
{
  public static string Template => "/categories";
  public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
  public static Delegate Handler => Action;

  [Authorize]
  public static IResult Action(CategoryRequest categoryRequest, ApplicationDbContext context)
  {
    string testCreatedBy = "teste";
    var category = new Category(categoryRequest.Name, testCreatedBy, testCreatedBy);

    if (!category.IsValid)
      return Results.ValidationProblem(category.Notifications.ConvertToProblemDetails());


    context.Categories.Add(category);
    context.SaveChanges();
    return Results.Created($"/categories/{category.Id}", category);
  }
}