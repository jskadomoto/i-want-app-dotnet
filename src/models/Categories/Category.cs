using Flunt.Validations;

namespace IWantApp.Domain.Products;

public class Category : Entity
{
  public required string Name { get; set; }
  public bool IsActive { get; set; }

  public Category(string name)
  {
    var contract = new Contract<Category>().IsNotNullOrEmpty(name, "Name", "O nome é obrigatório");

    AddNotifications(contract);

    Name = name;
    IsActive = true;
  }
}
