using System.Linq.Expressions;

namespace SailorNumberGuessingGame.Server.DAL
{
  public interface IGenericRepository<TEntity> where TEntity : class
  {
    void Delete(object id);
    void Delete(TEntity entityToDelete);
    void Delete(Expression<Func<TEntity, bool>> filter = null);
    Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");
    Task<TEntity> GetByID(object id);
    void Insert(TEntity entity);
    void Update(TEntity entityToUpdate);
  }
}
