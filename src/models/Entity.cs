namespace IWantApp.Domain;

public abstract class Entity : Notifiable<Notification>
{
  public Entity()
  {
    Id = Guid.NewGuid();
  }
  public Guid Id { get; set; }
  public DateTime CreatedAt { get; set; }
  public string CreatedBy { get; set; }
  public DateTime UpdatedAt { get; set; }
  public string UpdatedBy { get; set; }
}