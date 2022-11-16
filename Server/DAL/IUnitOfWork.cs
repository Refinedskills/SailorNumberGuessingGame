using Model = SailorNumberGuessingGame.Shared.Models;

namespace SailorNumberGuessingGame.Server.DAL
{
  public interface IUnitOfWork : IUnitOfWorkBase
  {
    IGenericRepository<Model.Player> PlayerRepository { get; }
    IGenericRepository<Model.Game> GameRepository { get; }
    IGenericRepository<Model.Action> ActionRepository { get; }
  }
}
