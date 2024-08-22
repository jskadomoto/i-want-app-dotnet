using Flunt.Validations;

namespace IWantApp.Domain.Products;

public class Category : Entity
{
  public string Name { get; private set; }
  public bool IsActive { get; private set; }

  public Category(string name, string createdBy, string updatedBy)
  {
    Name = name;
    IsActive = true;
    CreatedBy = createdBy;
    UpdatedBy = updatedBy;
    CreatedAt = DateTime.UtcNow;
    UpdatedAt = DateTime.UtcNow;

    Validate();
  }

  private void Validate()
  {
    var contract = new Contract<Category>()
      .IsNotNullOrEmpty(Name, "Name", "O nome é obrigatório")
      .IsGreaterOrEqualsThan(Name, 3, "Name", "O nome deve ter 3 ou mais caracteres")
      .IsNotNullOrEmpty(CreatedBy, "CreatedBy")
      .IsNotNullOrEmpty(UpdatedBy, "UpdatedBy");

    AddNotifications(contract);
  }

  public void EditInfo(string name, bool isActive)
  {
    Name = name;
    IsActive = isActive;

    Validate();
  }
}
