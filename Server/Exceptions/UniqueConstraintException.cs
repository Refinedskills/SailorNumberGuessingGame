namespace SailorNumberGuessingGame.Server.Exceptions
{
  public class UniqueConstraintException : Exception
  {
    public UniqueConstraintException() { }

    public UniqueConstraintException(string message) : base(message)
    {
    }

    public UniqueConstraintException(string message, Exception inner) : base(message, inner)
    {

    }

  }
}
