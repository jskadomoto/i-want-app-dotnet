namespace IWantApp.Domain.Products;
public record CategoryRequest(string Name, bool IsActive);
public record CategoryResponse(Guid Id, string Name, bool IsActive);