public class CostumerGet
{
    public static string Template => "/costumers";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handler => Action;

    [AllowAnonymous]
    public static async Task<IResult> Action(HttpContext http)
    {
        var userId = http.GetUserId();
        var user = http.User;

        var cpfClaim = user.Claims.FirstOrDefault(c => c.Type == "Cpf");
        var cpf = cpfClaim?.Value;

        var nameClaim = user.Claims.FirstOrDefault(c => c.Type == "Name");
        var name = nameClaim?.Value ?? "Usuário sem nome";

        var results = new
        {
            Id = userId,
            Name = name,
            Cpf = cpf
        };

        return Results.Ok(results);
    }
}
