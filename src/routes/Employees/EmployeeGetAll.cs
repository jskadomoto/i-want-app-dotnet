using System.Security.Claims;
using IWantApp.Domain;
using Microsoft.AspNetCore.Identity;

public class EmployeeGetAll
{
  public static string Template => "/employees";
  public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
  public static Delegate Handler => Action;
  public static IResult Action(int page, int rows, UserManager<IdentityUser> userManager)
  {
    var users = userManager.Users.Skip(Pagination.SkipPage(page, rows)).Take(rows).ToList();
    var employees = new List<EmployeeResponse>();

    foreach (var item in users)
    {
      var claims = userManager.GetClaimsAsync(item).Result;
      var claimName = claims.FirstOrDefault(c => c.Type == "Name");
      var userName = claimName != null ? claimName.Value : string.Empty;

      employees.Add(new EmployeeResponse(item.Email, userName));
    }

    return Results.Ok(employees);

  }
}