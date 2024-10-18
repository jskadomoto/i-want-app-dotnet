using System.Security.Claims;
using IWantApp.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

public class EmployeePost
{
  public static string Template => "/employees";
  public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
  public static Delegate Handler => Action;

  [Authorize(Policy = "EmployeePolicy")]
  public static IResult Action(EmployeeRequest employeeRequest, HttpContext http, UserManager<IdentityUser> userManager)
  {
    var userId = http.GetUserId();
    var user = new IdentityUser { UserName = employeeRequest.Email, Email = employeeRequest.Email };
    var result = userManager.CreateAsync(user, employeeRequest.Password).Result;

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
      var claimResult = userManager.AddClaimAsync(user, claim).Result;

      if (!claimResult.Succeeded)
        return Results.ValidationProblem(claimResult.Errors.ConvertToProblemDetails());
    }


    return Results.Created($"/employees/{user.Id}", user.Id);

  }
}