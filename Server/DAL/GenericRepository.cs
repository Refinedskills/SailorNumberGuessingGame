using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace SailorNumberGuessingGame.Server.DAL
{
  public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
  {
    internal DbContext context;
    internal DbSet<TEntity> dbSet;

    public GenericRepository(DbContext context)
    {
      this.context = context;
      this.dbSet = context.Set<TEntity>();
    }

    public virtual async Task<IEnumerable<TEntity>> Get(
      Expression<Func<TEntity, bool>> filter = null,
      Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
      string includeProperties = "")
    {
      IQueryable<TEntity> query = dbSet;

      if (filter != null)
      {
        query = query.Where(filter);
      }

      includeProperties = Regex.Replace(includeProperties, @"\s", "");
      foreach (var includeProperty in includeProperties.Split
        (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
      {
        query = query.Include(includeProperty.Trim());
      }

      if (orderBy != null)
      {
        return await orderBy(query).ToListAsync();
      }
      else
      {
        return await query.ToListAsync();
      }
    }

    public virtual async Task<TEntity> GetByID(object id)
    {
      return await dbSet.FindAsync(id);
    }

    public virtual void Insert(TEntity entity)
    {
      dbSet.Add(entity);
    }

    public virtual void Delete(object id)
    {
      TEntity entityToDelete = dbSet.Find(id);
      Delete(entityToDelete);
    }

    public virtual void Delete(TEntity entityToDelete)
    {
      if (context.Entry(entityToDelete).State == EntityState.Detached)
      {
        dbSet.Attach(entityToDelete);
      }
      dbSet.Remove(entityToDelete);
    }

    public virtual void Delete(Expression<Func<TEntity, bool>> filter = null)
    {
      IQueryable<TEntity> query = dbSet;

      if (filter != null)
      {
        query = query.Where(filter);
      }

      foreach (var entityToDelete in query)
      {
        if (context.Entry(entityToDelete).State == EntityState.Detached)
        {
          dbSet.Attach(entityToDelete);
        }
        dbSet.Remove(entityToDelete);
      }
    }

    public virtual void Update(TEntity entityToUpdate)
    {
      dbSet.Attach(entityToUpdate);
      context.Entry(entityToUpdate).State = EntityState.Modified;
    }
  }
}
