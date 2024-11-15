namespace IWantApp.Domain.Products;

public class ProductGetShowCase
{
    public static string Template => "/products/showcase";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handler => Action;

    [AllowAnonymous]
    public static async Task<IResult> Action( ApplicationDbContext context, int page = 1, int rows = 10, string orderBy = "name")
    {
        string sortOrder = string.IsNullOrEmpty(orderBy) ? "name" : orderBy.ToLower();

        var products = await GetProductsAsync(page, page, sortOrder, context);

        var response = products.Select(p => new ProductResponse(
            p.Id, p.Name, p.Category.Name, p.Description, p.HasStock, p.Price, p.IsActive));

        return Results.Ok(response);
    }

    private static async Task<List<Product>> GetProductsAsync(int page, int rows, string orderBy, ApplicationDbContext context)
    {
        var query = context.Products
            .Include(p => p.Category)
            .Where(p => p.HasStock && p.Category.IsActive);

        query = orderBy switch
        {
            "name" => query.OrderBy(p => p.Name),
            "price" => query.OrderBy(p => p.Price),
            _ => query.OrderBy(p => p.Name)
        };

        query = query.Skip(Pagination.SkipPage(page, rows)).Take(rows);

        return await query.ToListAsync();
    }
}
