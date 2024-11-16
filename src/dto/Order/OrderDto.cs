
public record OrderRequest(List<Guid> ProductIds, string AddressDelivery);
public record OrderResponse(Guid Id, string CostumerEmail, IEnumerable<OrderProduct> Products, string DeliveryAddress);
public record OrderProduct (Guid Id, string Name);