namespace IWantApp.Domain.Products;

public class ProductSoldGet
{
  public static string Template => "/products/sold";
  public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
  public static Delegate Handler => Action;
  [Authorize(Policy = "EmployeePolicy")]
  public static async Task<IResult> Action(QueryAllProductsSold query)
  {
    var response = await query.Execute();

    return Results.Ok(response);
  }
}