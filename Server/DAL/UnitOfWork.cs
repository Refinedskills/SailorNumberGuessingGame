using Microsoft.EntityFrameworkCore;
using SailorNumberGuessingGame.Server.Data;
using Model = SailorNumberGuessingGame.Shared.Models;

namespace SailorNumberGuessingGame.Server.DAL;

public class UnitOfWork : UnitOfWorkBase<DataContext>, IUnitOfWork
{
  private IGenericRepository<Model.Player> _playerRepository;
  private IGenericRepository<Model.Game> _gameRepository;
  private IGenericRepository<Model.Action> _actionRepository;

  public UnitOfWork(DataContext context) : base(context) { }

  public IGenericRepository<Model.Player> PlayerRepository
  {
    get
    {
      if (this._playerRepository == null)
      {
        this._playerRepository = new GenericRepository<Model.Player>(_context);
      }
      return this._playerRepository;
    }
  }

  public IGenericRepository<Model.Game> GameRepository
  {
    get
    {
      if (this._gameRepository == null)
      {
        this._gameRepository = new GenericRepository<Model.Game>(_context);
      }
      return this._gameRepository;
    }
  }

  public IGenericRepository<Model.Action> ActionRepository
  {
    get
    {
      if (this._actionRepository == null)
      {
        this._actionRepository = new GenericRepository<Model.Action>(_context);
      }
      return this._actionRepository;
    }
  }
}
