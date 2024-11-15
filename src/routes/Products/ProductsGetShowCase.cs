namespace IWantApp.Domain.Products;

public static class ProductOrderBy
{
    public const string Name = "name";
    public const string Price = "price";

    // Validation method to check if a value is valid
    public static bool IsValid(string value) =>
        value == Name || value == Price;
}

public class ProductGetShowCase
{
    public static string Template => "/products/showcase";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handler => Action;

    [AllowAnonymous]
    public static async Task<IResult> Action(ApplicationDbContext context, int page = 1, int rows = 10, string orderBy = ProductOrderBy.Name)
    {
        if (rows > 100)
        {
            return Results.Problem(title: "Row limit exceeded. Maximum allowed is 100.", statusCode: 400);
        }

        if (!ProductOrderBy.IsValid(orderBy))
        {
            return Results.Problem(title: "Invalid orderBy value. Use 'name' or 'price'.", statusCode: 400);
        }

        var products = await GetProductsAsync(page, rows, orderBy, context);

        var response = products.Select(p => new ProductResponse(
            p.Id, p.Name, p.Category.Name, p.Description, p.HasStock, p.Price, p.IsActive));

        return Results.Ok(response);
    }

    private static async Task<List<Product>> GetProductsAsync(int page, int rows, string orderBy, ApplicationDbContext context)
    {
        var query = context.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .Where(p => p.HasStock && p.Category.IsActive);

        query = orderBy switch
        {
            ProductOrderBy.Name => query.OrderBy(p => p.Name),
            ProductOrderBy.Price => query.OrderBy(p => p.Price),
            _ => query.OrderBy(p => p.Name)
        };

        query = query.Skip(Pagination.SkipPage(page, rows)).Take(rows);

        return await query.ToListAsync();
    }
}
