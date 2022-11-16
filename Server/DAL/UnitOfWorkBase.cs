using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using SailorNumberGuessingGame.Server.Exceptions;

namespace SailorNumberGuessingGame.Server.DAL
{
  public class UnitOfWorkBase<T> : IDisposable where T : DbContext
  {
    protected T _context;

    public UnitOfWorkBase(T context)
    {
      _context = context;
    }

    public IDbContextTransaction BeginTransAction()
    {
      return _context.Database.BeginTransaction();
    }
    public void ExecuteAsRetriable(Action operation)
    {
      _context.Database.CreateExecutionStrategy().Execute(operation);
    }

    public void SaveSynchronous()
    {
      try
      {
        _context.SaveChanges();
      }
      catch (DbUpdateException ex)
      {
        HandleDbUpdateException(ex);
      }
    }

    public async Task Save()
    {
      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateException ex)
      {
        HandleDbUpdateException(ex);
      }
    }

    private void HandleDbUpdateException(DbUpdateException ex)
    {
      if (ex.InnerException is Microsoft.Data.SqlClient.SqlException sqlException && (sqlException.Number == 2601 || sqlException.Number == 2627))
      {
        throw new UniqueConstraintException("Cannot insert duplicate values.", ex);
      }
      throw ex;
    }


    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
      if (!this.disposed)
      {
        if (disposing)
        {
          _context.Dispose();
        }
      }
      this.disposed = true;
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }
  }
}
