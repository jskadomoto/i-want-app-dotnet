namespace IWantApp.Domain;

public abstract class Entity
{
  public Entity()
  {
    Id = Guid.NewGuid();
  }
  public Guid Id { get; set; }
  public required DateTime CreatedAt { get; set; }
  public required string CreatedBy { get; set; }
  public required DateTime UpdatedAt { get; set; }
  public required string UpdatedBy { get; set; }
}