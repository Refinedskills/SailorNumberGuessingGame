using Microsoft.AspNetCore.Mvc;
using SailorNumberGuessingGame.Server.DAL;

namespace SailorNumberGuessingGame.Server.Controllers
{
  public abstract class ControllerBase<T> : Controller where T : IUnitOfWorkBase
  {
    public ControllerBase(T unitOfWork)
    {
      UnitOfWork = unitOfWork;
    }

    public T UnitOfWork { get; private set; }

    protected void SyncItems<U, V, W>(U repository,
      ICollection<W> targetItems,
      IEnumerable<V> sourceItems,
      Func<W, V, bool> match,
      Action<W, V> setTarget
    ) where U : DAL.IGenericRepository<W> where W : class, new()
    {
      foreach (var targetItem in targetItems)
      {
        var sourceItem = sourceItems.FirstOrDefault(i => match(targetItem, i));
        if (null == sourceItem)
        {
          repository.Delete(targetItem);
          continue;
        }
        setTarget(targetItem, sourceItem);
      }
      foreach (var sourceItem in sourceItems)
      {
        if (null != targetItems.FirstOrDefault(i => match(i, sourceItem))) continue;
        W target = new W();
        setTarget(target, sourceItem);
        repository.Insert(target);
      }
    }
  }
}
