using SailorNumberGuessingGame.Shared.Models.DTO;

using Model = SailorNumberGuessingGame.Shared.Models;


namespace SailorNumberGuessingGame.Server.Services.GameService;
public interface IGameService
{
  public Task<Model.ServiceResponse<Model.DTO.DtoPlayer>> LoginPlayer(string name, DateTime birthDate, bool autoRegistration);
  public Task<Model.ServiceResponse<List<Model.Player>>> GetPlayersAsync();
  public string PickRandomNumber(int numberOfDigits);
  public CompareNumbersResult TestEqualNumbers(string expectedNumber, string actualNumber);
}
