
public record EmployeeRequest(string Email, string Password, string Name, string EmployeeCode);
public record EmployeeResponse(string Email, string Name);