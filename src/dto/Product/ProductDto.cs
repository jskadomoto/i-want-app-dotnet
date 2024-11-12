namespace IWantApp.Domain.Products;
public record ProductRequest(string Name, Guid CategoryId, string Description, bool HasStock, decimal Price, bool Active);
public record ProductResponse(Guid Id, string Name, string CategoryName, string Description, bool HasStock, decimal Price, bool IsActive);