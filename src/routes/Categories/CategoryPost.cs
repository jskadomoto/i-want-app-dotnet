namespace IWantApp.Domain.Products;

public class CategoryPost
{
  public static string Template => "/categories";
  public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
  public static Delegate Handler => Action;

  [Authorize(Policy = "EmployeePolicy")]
  public static async Task<IResult> Action(CategoryRequest categoryRequest, HttpContext http, ApplicationDbContext context)
  {
    var userId = http.GetUserId();
    var category = new Category(categoryRequest.Name, userId, userId);

    if (!category.IsValid)
      return Results.ValidationProblem(category.Notifications.ConvertToProblemDetails());


    await context.Categories.AddAsync(category);
    await context.SaveChangesAsync();
    return Results.Created($"/categories/{category.Id}", category);
  }
}