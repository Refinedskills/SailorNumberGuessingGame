using SailorNumberGuessingGame.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SailorNumberGuessingGame.Server.Data;

public class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
  public void Configure(EntityTypeBuilder<Player> builder)
  {
    builder
      .HasMany(b => b.Games)
      .WithOne(a => a.Player)
      .OnDelete(DeleteBehavior.NoAction);
  }
}
