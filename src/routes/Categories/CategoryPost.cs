using IWantApp.Database;

namespace IWantApp.Domain.Products;

public class CategoryPost
{
  public static string Template => "/categories";
  public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
  public static Delegate Handler => Action;
  public static IResult Action(CategoryRequest categoryRequest, ApplicationDbContext context)
  {
    string testCreatedBy = "teste";
    var category = new Category(categoryRequest.Name, testCreatedBy, testCreatedBy);

    if (!category.IsValid)
    {
      var err = category.Notifications
        .GroupBy(group => group.Key)
        .ToDictionary(g => g.Key, g => g.Select(err => err.Message).ToArray());

      return Results.ValidationProblem(err);
    }

    context.Categories.Add(category);
    context.SaveChanges();
    return Results.Created($"/categories/{category.Id}", category);
  }
}