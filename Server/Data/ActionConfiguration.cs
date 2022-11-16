using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Model = SailorNumberGuessingGame.Shared.Models;

namespace SailorNumberGuessingGame.Server.Data;

public class ActionConfiguration : IEntityTypeConfiguration<Model.Action>
{
  public void Configure(EntityTypeBuilder<Model.Action> builder)
  {
    builder
      .HasOne(b => b.Game)
      .WithMany(a => a.Actions)
      .OnDelete(DeleteBehavior.NoAction);
  }
}
