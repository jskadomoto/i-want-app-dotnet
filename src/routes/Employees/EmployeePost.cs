public class EmployeePost
{
  public static string Template => "/employees";
  public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
  public static Delegate Handler => Action;

  [Authorize(Policy = "EmployeePolicy")]
  public static async Task<IResult> Action(EmployeeRequest employeeRequest, HttpContext http, UserManager<IdentityUser> userManager)
  {
    var userId = http.GetUserId();
    var user = new IdentityUser { UserName = employeeRequest.Email, Email = employeeRequest.Email };
    var result = await userManager.CreateAsync(user, employeeRequest.Password);

    if (!result.Succeeded)
      return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());

    var userClaims = new List<Claim>
    {
      new Claim("EmployeeCode", employeeRequest.EmployeeCode),
      new Claim("Name", employeeRequest.Name),
      new Claim("CreatedBy", userId),
    };

    foreach (var claim in userClaims)
    {
      var claimResult = await userManager.AddClaimAsync(user, claim);

      if (!claimResult.Succeeded)
        return Results.ValidationProblem(claimResult.Errors.ConvertToProblemDetails());
    }


    return Results.Created($"/employees/{user.Id}", user.Id);

  }
}