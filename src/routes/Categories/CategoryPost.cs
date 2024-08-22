using IWantApp.Database;

namespace IWantApp.Domain.Products;

public class CategoryPost
{
  public static string Template => "/categories";
  public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
  public static Delegate Handler => Action;
  public static IResult Action(CategoryRequest categoryRequest, ApplicationDbContext context)
  {
    var category = new Category
    {
      Name = categoryRequest.Name,
      CreatedAt = DateTime.UtcNow,
      CreatedBy = "categoryRequest.CreatedBy",
      UpdatedAt = DateTime.UtcNow,
      UpdatedBy = "categoryRequest.UpdatedBy",
    };

    context.Categories.Add(category);
    context.SaveChanges();
    return Results.Created($"/categories/{category.Id}", category);
  }
}