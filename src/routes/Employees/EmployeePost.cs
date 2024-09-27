using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

public class EmployeePost
{
  public static string Template => "/employees";
  public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
  public static Delegate Handler => Action;
  public static IResult Action(EmployeeRequest employeeRequest, UserManager<IdentityUser> userManager)
  {
    var user = new IdentityUser { UserName = employeeRequest.Email, Email = employeeRequest.Email };
    var result = userManager.CreateAsync(user, employeeRequest.Password).Result;

    if (!result.Succeeded)
      return Results.BadRequest(result.Errors.First());

    var userClaims = new List<Claim>
    {
      new Claim("EmployeeCode", employeeRequest.EmployeeCode),
      new Claim("Name", employeeRequest.Name),
    };

    foreach (var claim in userClaims)
    {
      var claimResult = userManager.AddClaimAsync(user, claim).Result;

      if (!claimResult.Succeeded)
        return Results.BadRequest(claimResult.Errors.First());
    }


    return Results.Created($"/employees/{user.Id}", user.Id);

  }
}