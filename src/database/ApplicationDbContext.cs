namespace IWantApp.Database;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
  public DbSet<Product> Products { get; set; }
  public DbSet<Category> Categories { get; set; }
  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);
    builder.Ignore<Notification>();
    builder.Entity<Product>().Property(p => p.Name).IsRequired();
    builder.Entity<Product>().Property(p => p.Description).HasMaxLength(255);
    builder.Entity<Category>().Property(c => c.Name).IsRequired();
  }
  protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
  {
    configurationBuilder.Properties<string>().HaveMaxLength(100);
  }
}