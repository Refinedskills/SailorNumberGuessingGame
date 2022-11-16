using Model = SailorNumberGuessingGame.Shared.Models;

using Microsoft.EntityFrameworkCore;
using SailorNumberGuessingGame.Server.Data;
using SailorNumberGuessingGame.Shared.Models;
using SailorNumberGuessingGame.Server.Helpers;
using SailorNumberGuessingGame.Shared.Models.DTO;
using SailorNumberGuessingGame.Shared.Models.Enums;

namespace SailorNumberGuessingGame.Server.Services.GameService
{
  public class GameService : IGameService
  {
    private const string unknownError = "Unknown error";
    private readonly DataContext _dbContext;


    public GameService(DataContext dbContext)
    {
      _dbContext = dbContext;
    }

    public async Task<ServiceResponse<Model.DTO.DtoPlayer>> LoginPlayer(string name, DateTime birthDate, bool autoRegistration)
    {
      string message = "Unknown error";
      Model.DTO.DtoPlayer dtoPlayer = null;
      bool success = false;

      try
      {
        // Try to find a player with the given name and birthdate
        var dbPlayer = await _dbContext.Players.Where(p => p.Name == name && p.BirthDate == birthDate).SingleOrDefaultAsync();
        if (dbPlayer == null && autoRegistration)
        {
          _dbContext.Players.Add(new Model.Player
          {
            Name = name,
            BirthDate = birthDate
          });
          await _dbContext.SaveChangesAsync();
          dbPlayer = await _dbContext.Players.Where(p => p.Name == name && p.BirthDate == birthDate).SingleOrDefaultAsync();
        }
        dtoPlayer = new Model.DTO.DtoPlayer(dbPlayer);
        success = true;
      }
      catch (Exception ex)
      {
        message = ex.Message;
      }

      return new Model.ServiceResponse<Model.DTO.DtoPlayer>
      {
        Data = dtoPlayer,
        Message = message,
        Success = success
      };
    }

    public async Task<Model.ServiceResponse<List<Model.Player>>> GetPlayersAsync()
    {
      return new Model.ServiceResponse<List<Model.Player>>
      {
        Data = await _dbContext.Players.ToListAsync(),
        Message = "",
        Success = true
      };
    }

    public string PickRandomNumber(int numberOfDigits)
    {
      return RandomNumberHelper.GenerateRandomNumber(numberOfDigits);
    }

    public CompareNumbersResult TestEqualNumbers(string expectedNumber, string enteredNumber)
    {
      CompareNumbersResultCode compareNumbersResultCode = CompareNumbersResultCode.COMPARENUMBERS_PENDING;
      int correctDigitLocations = 0;
      int correctDigits = 0;

      // TODO !!!!! Maak SOLID, b.v. via helper class
      // Check expected number format
      if (string.IsNullOrEmpty(expectedNumber) || !expectedNumber.All(char.IsDigit))
        compareNumbersResultCode = CompareNumbersResultCode.COMPARENUMBERS_EXPEXTEDNUMBERFORMAT_NOT_CORRECT;      
      // Check entered number format
      else if (string.IsNullOrEmpty(enteredNumber) || expectedNumber.Length != enteredNumber.Length)
        compareNumbersResultCode = CompareNumbersResultCode.COMPARENUMBERS_ENTEREDNUMBERFORMAT_NOT_CORRECT;
      // You guess is CORRECT !!!!!!!!!!!!!!!!!
      else if (expectedNumber == enteredNumber)
        compareNumbersResultCode = CompareNumbersResultCode.COMPARENUMBERS_SAME;
      // Tja, sometimes you are wrong, I give a hint ..... 
      else
      {
        var cloneExpectedNumber = new String(expectedNumber).ToArray();
        // Determine correct digit locations
        for (int i = 0; i < cloneExpectedNumber.Length; i++)
        {
          if (cloneExpectedNumber[i] == enteredNumber[i])
          {
            correctDigitLocations++;
            cloneExpectedNumber[i] = ' ';
          }
        }
        // Determine correct digits
        for (int i = 0; i < cloneExpectedNumber.Length; i++)
        {
          string clonedExpectedString = string.Join("", cloneExpectedNumber); 
          int targetIndex = clonedExpectedString.IndexOf(enteredNumber[i]); 
          //if no letter is found IndexOf return -1
          if (targetIndex > -1) //In other words if a letter was found
          {
            correctDigits++;
            cloneExpectedNumber[targetIndex] = ' ';
          }
        }
      }

      return new CompareNumbersResult
      {
        CompareNumbersResultCode = compareNumbersResultCode,
        NeededDigits = compareNumbersResultCode != CompareNumbersResultCode.COMPARENUMBERS_EXPEXTEDNUMBERFORMAT_NOT_CORRECT? expectedNumber.Length : null,
        CorrectDigitLocations = correctDigitLocations,
        CorrectDigits = correctDigits
      };
    }
  }
}
