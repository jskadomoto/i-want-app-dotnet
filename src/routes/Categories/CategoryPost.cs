using System.Security.Claims;
using IWantApp.Database;
using Microsoft.AspNetCore.Authorization;

namespace IWantApp.Domain.Products;

public class CategoryPost
{
  public static string Template => "/categories";
  public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
  public static Delegate Handler => Action;

  [Authorize(Policy = "EmployeePolicy")]
  public static IResult Action(CategoryRequest categoryRequest, HttpContext http, ApplicationDbContext context)
  {
    var userId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
    var category = new Category(categoryRequest.Name, userId, userId);

    if (!category.IsValid)
      return Results.ValidationProblem(category.Notifications.ConvertToProblemDetails());


    context.Categories.Add(category);
    context.SaveChanges();
    return Results.Created($"/categories/{category.Id}", category);
  }
}