public class Order : Entity {
  public string CostumerId { get; private set; }
  public List<Product> Products { get; private set; }
  public decimal Total { get; private set; }
  public string DeliveryAddress { get; private set; }

  private Order() {}

  public Order(string costumerId, string costumerName, List<Product> products, string deliveryAddress) {
    CostumerId = costumerId;
    Products = products;
    DeliveryAddress = deliveryAddress;
    CreatedBy = costumerName;
    CreatedAt = DateTime.UtcNow;
    UpdatedBy = costumerName;
    UpdatedAt = DateTime.UtcNow;

    Total = 0;

    foreach (var product in products) {
      Total += product.Price;
    }

    Validate();
  }

  private void Validate() {
    var contract = new Contract<Order>()
        .IsNotNull(CostumerId, "Costumer")
        .IsTrue(Products != null && Products.Any(), "Products")
        .IsNotNullOrEmpty(DeliveryAddress, "DeliveryAddress");

    AddNotifications(contract);
}
}