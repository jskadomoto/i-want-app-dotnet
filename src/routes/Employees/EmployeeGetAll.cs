public class EmployeeGetAll
{
  public static string Template => "/employees";
  public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
  public static Delegate Handler => Action;

  [Authorize(Policy = "EmployeeSuperAdminPolicy")]
  public static async Task<IResult> Action(int? page, int? rows, QueryAllUsersWithClaimName query)
  {
    page ??= 1;

    if (rows == null || rows > 10) {
      rows = 10;
      var warning = "O número máximo de registros por página é 10. A busca foi limitada.";

      return Results.Ok(new { Warning = warning, Employees = query.Execute(page.Value, rows.Value) });
    }
    var employees = await query.Execute(page.Value, rows.Value);

    return Results.Ok(employees);
  }
}