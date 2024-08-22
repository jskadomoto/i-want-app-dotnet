using Flunt.Validations;

namespace IWantApp.Domain.Products;

public class Category : Entity
{
  public string Name { get; set; }
  public bool IsActive { get; set; }

  public Category(string name, string createdBy, string updatedBy)
  {
    var contract = new Contract<Category>()
      .IsNotNullOrEmpty(name, "Name", "O nome é obrigatório")
      .IsGreaterOrEqualsThan(name, 3, "Name", "O nome deve ter 3 ou mais caracteres")
      .IsNotNullOrEmpty(createdBy, "CreatedBy")
      .IsNotNullOrEmpty(updatedBy, "UpdatedBy");

    AddNotifications(contract);

    Name = name;
    IsActive = true;
    CreatedBy = createdBy;
    UpdatedBy = updatedBy;
    CreatedAt = DateTime.UtcNow;
    UpdatedAt = DateTime.UtcNow;
  }
}
