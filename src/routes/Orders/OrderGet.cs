public class OrdeGet
{
    public static string Template => "/orders/{id}";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handler => Action;

    [Authorize]
   public static async Task<IResult> Action(Guid id, HttpContext http, ApplicationDbContext context, UserManager<IdentityUser> userManager)
{
    var costumerId = http.GetUserId();
    var employeeClaim = http.User.Claims.FirstOrDefault(c => c.Type == "EmployeeCode")?.Value;

    // Carregar pedido com os produtos associados
    var order = context.Orders
        .Include(o => o.Products)
        .FirstOrDefault(c => c.Id == id);

    if (order == null)
    {
        return Results.NotFound($"Order with ID {id} not found.");
    }

    if (order.CostumerId != costumerId && string.IsNullOrEmpty(employeeClaim))
    {
        return Results.Forbid();
    }

    var costumer = await userManager.FindByIdAsync(order.CostumerId);
    if (costumer == null)
    {
        return Results.NotFound($"Customer with ID {order.CostumerId} not found.");
    }

    var productsResponse = (order.Products ?? new List<Product>())
        .Select(p => new OrderProduct(p.Id, p.Name));

    var orderResponse = new OrderResponse(order.Id, costumer.Email, productsResponse, order.DeliveryAddress);

    return Results.Ok(orderResponse);
}

}
