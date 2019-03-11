using System.Data.Entity;

namespace BreakAwayConsole
{
  public class TestContext : DbContext
  {
    public TestContext(): base("TestDatabase")
    { }

    public DbSet<Destination> Destinations { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      modelBuilder.Configurations.Add(new DestinationConfiguration());
    }
     
  }
}