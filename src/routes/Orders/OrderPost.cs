public class OrderPost
{
  public static string Template => "/orders";
  public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
  public static Delegate Handler => Action;

  [Authorize(Policy = "CpfPolicy")]
  public static async Task<IResult> Action(OrderRequest orderRequest, HttpContext http, ApplicationDbContext context)
  {
    var costumerId = http.GetUserId();
    var costumerName = http.User.Claims.First(c => c.Type == "Name").Value;

    List<Product> productsFound = null;

    if(orderRequest.ProductIds != null || !orderRequest.ProductIds.Any()) 
      productsFound = context.Products.Where(p => orderRequest.ProductIds.Contains(p.Id)).ToList();

    var order = new Order(costumerId, costumerName, productsFound, orderRequest.AddressDelivery);
    if (!order.IsValid) {
      return Results.ValidationProblem(order.Notifications.ConvertToProblemDetails());
    }

    await context.Orders.AddAsync(order);
    await context.SaveChangesAsync();

    return Results.Created($"/orders/{order.Id}", order.Id);

  }
}