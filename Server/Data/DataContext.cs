using Model = SailorNumberGuessingGame.Shared.Models;
using Microsoft.EntityFrameworkCore;


namespace SailorNumberGuessingGame.Server.Data
{
  public class DataContext : DbContext
  {
    public DataContext() { }

    /// <summary>
    /// constructor for the database context
    /// </summary>
    /// <param name="options">Database context options</param>
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Model.Player> Players { get; set; }
    public DbSet<Model.Game> Games { get; set; }
    public DbSet<Model.Action> Actions { get; set; }

    /// <summary>
    /// Builds the model with the right configuration.
    /// </summary>
    /// <param name="modelBuilder">The model builder to build a model for the database context</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfiguration(new PlayerConfiguration());
      modelBuilder.ApplyConfiguration(new GameConfiguration());
      modelBuilder.ApplyConfiguration(new ActionConfiguration());
    }


  }
}
