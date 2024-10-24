namespace IWantApp.Domain.Products;

public class CategoryPut
{
  public static string Template => "/categories/{id:guid}";
  public static string[] Methods => new string[] { HttpMethod.Put.ToString() };
  public static Delegate Handler => Action;

  [Authorize(Policy = "EmployeePolicy")]
  public static IResult Action([FromRoute] Guid id, CategoryRequest categoryRequest, HttpContext http, ApplicationDbContext context)
  {
    var userId = http.GetUserId();
    var category = context.Categories.Where(c => c.Id == id).FirstOrDefault();
    if (category == null)
    {
      return Results.NotFound("Categoria não encontrada.");
    }

    category.EditInfo(categoryRequest.Name, categoryRequest.IsActive, userId);

    if (!category.IsValid)
      return Results.ValidationProblem(category.Notifications.ConvertToProblemDetails());

    context.SaveChanges();
    return Results.Ok(category);
  }
}