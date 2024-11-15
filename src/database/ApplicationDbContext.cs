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
    /* Products */
    builder.Entity<Product>().Property(p => p.Name).IsRequired();
    builder.Entity<Product>().Property(p => p.Description).HasMaxLength(255);
    builder.Entity<Product>().Property(p => p.Price).HasColumnType("decimal(10,2)").IsRequired();
    /* Category */
    builder.Entity<Category>().Property(c => c.Name).IsRequired();
    /* Order */
    builder.Entity<Order>().Property(o => o.CostumerId).IsRequired();
    builder.Entity<Order>().Property(o => o.DeliveryAddress).IsRequired();
    builder.Entity<Order>().HasMany(o => o.Products).WithMany(p => p.Orders).UsingEntity(x => x.ToTable("OrderProducts"));
  }
  protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
  {
    configurationBuilder.Properties<string>().HaveMaxLength(100);
  }
}