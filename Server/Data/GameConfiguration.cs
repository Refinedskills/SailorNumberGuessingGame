using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SailorNumberGuessingGame.Shared.Models;

namespace SailorNumberGuessingGame.Server.Data;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
  public void Configure(EntityTypeBuilder<Game> builder)
  {
    builder
      .HasOne(b => b.Player)
      .WithMany(a => a.Games)
      .OnDelete(DeleteBehavior.NoAction);
  }
}
