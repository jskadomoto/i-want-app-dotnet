namespace IWantApp.Domain.Products;
public record ProductRequest(string Name, Guid CategoryId, string Description, bool HasStock, bool Active);
public record ProductResponse(string Name, string CategoryName, string Description, bool HasStock, bool IsActive);