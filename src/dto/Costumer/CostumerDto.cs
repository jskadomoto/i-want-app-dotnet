
public record CostumerRequest(string Email, string Password, string Name, string Cpf);
public record CostumerResponse(string Email, string Name);