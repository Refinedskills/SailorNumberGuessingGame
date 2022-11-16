namespace SailorNumberGuessingGame.Server.Helpers
{
  public static class RandomNumberHelper
  {
    private const int minNrOfDigits = 4;
    private const int maxNrOfDigits = 8;

    public static string GenerateRandomNumber(int nrOfDigits)
    {
      nrOfDigits = nrOfDigits < minNrOfDigits ? minNrOfDigits : nrOfDigits > maxNrOfDigits ? maxNrOfDigits : nrOfDigits;
      var random = new Random();
      string s = string.Format("{0}{1}", new string('0', nrOfDigits - 1), random.Next(0, (int)Math.Pow(10, nrOfDigits)));
      return s.Substring(s.Length - nrOfDigits);
    }

  }
}
