using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SailorNumberGuessingGame.Server.Helpers;


namespace TestSailorNumberGuessingGame.Helpers
{
  public class RandomNumberHelperTests
  {
    // Length of the random number: >= 4 && <= 8
    [Theory]
    [InlineData(4, -1483092)]
    [InlineData(4, -1)]
    [InlineData(4, 0)]
    [InlineData(4, 4)]
    [InlineData(5, 5)]
    [InlineData(6, 6)]
    [InlineData(7, 7)]
    [InlineData(8, 8)]
    [InlineData(8, 9)]
    [InlineData(8, 10000)]
    public void LengthRandomNumberTheory(int expected, int nrOfDigits)
    {
      Assert.Equal(expected, RandomNumberHelper.GenerateRandomNumber(nrOfDigits).Length);
    }

  }
}
