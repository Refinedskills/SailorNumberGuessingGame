using Microsoft.EntityFrameworkCore.Storage;

namespace SailorNumberGuessingGame.Server.DAL
{
  public interface IUnitOfWorkBase
  {
    void Dispose();
    Task Save();
    IDbContextTransaction BeginTransAction();
    void ExecuteAsRetriable(Action operation);
    void SaveSynchronous();
  }
}
