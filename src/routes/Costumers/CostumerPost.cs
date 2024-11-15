public class CostumerPost
{
  public static string Template => "/costumers";
  public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
  public static Delegate Handler => Action;

  [AllowAnonymous]
  public static async Task<IResult> Action(CostumerRequest costumerRequest, CostumerCreator costumerCreator)
  {
    var formattedCpf = CpfHelper.RemoveCpfFormat(costumerRequest.Cpf);
    var userClaims = new List<Claim>

    {
      new Claim("Cpf", formattedCpf),
      new Claim("Name", costumerRequest.Name),
    };

    (IdentityResult identity, string userId) result = 
      await costumerCreator.Create(costumerRequest.Email, costumerRequest.Password, userClaims);

    if (!result.identity.Succeeded)
      return Results.ValidationProblem(result.identity.Errors.ConvertToProblemDetails());


    return Results.Created($"/costumers/{result.userId}", result.userId);

  }
}