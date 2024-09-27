using Dapper;
using Microsoft.Data.SqlClient;

public class EmployeeGetAll
{
  public static string Template => "/employees";
  public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
  public static Delegate Handler => Action;
  public static IResult Action(int? page, int? rows, IConfiguration configuration)
  {
    page ??= 1;

    if (rows == null || rows > 10) {
      rows = 10;
      var warning = "O número máximo de registros por página é 10. A busca foi limitada.";

      return Results.Ok(new { Warning = warning, Employees = GetEmployees(page.Value, rows.Value, configuration) });
    }
    var employees = GetEmployees(page.Value, rows.Value, configuration);

    return Results.Ok(employees);
  }
  private static IEnumerable<EmployeeResponse> GetEmployees(int page, int rows, IConfiguration configuration)
  {

    var db = new SqlConnection(configuration["ConnectionString:IWantDb"]);

    var query = @"select Email, ClaimValue as Name
        from AspNetUsers u
        inner join AspNetUserClaims c
        on u.Id = c.UserId
        and ClaimType = 'Name'
        order by Name
        OFFSET (@page - 1) * @rows ROWS
        FETCH NEXT @rows ROWS ONLY
      ";

    return db.Query<EmployeeResponse>(query, new { page, rows });
  }
}