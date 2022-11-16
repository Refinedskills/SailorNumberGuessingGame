using SailorNumberGuessingGame.Server.Services.GameService;
using Model = SailorNumberGuessingGame.Shared.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using SailorNumberGuessingGame.Server.DAL;
using SailorNumberGuessingGame.Server.Data;
using static MudBlazor.CategoryTypes;
using Swashbuckle.AspNetCore.Annotations;
using SailorNumberGuessingGame.Shared.Models.Results;
using SailorNumberGuessingGame.Shared.Models.Enums;
using System.Net;
using SailorNumberGuessingGame.Server.Exceptions;
using SailorNumberGuessingGame.Server.Helpers;
using System.ComponentModel;
using SailorNumberGuessingGame.Shared.Models.DTO;

namespace SailorNumberGuessingGame.Server.Controllers
{
  [Produces("application/json")]
  [ApiController]
  public class GameController : ControllerBase<IUnitOfWork>
  {
    private readonly ILogger<GameController> _logger;
    private readonly IGameService _gameService;

    public GameController(IUnitOfWork unitOfWork, ILogger<GameController> logger, IGameService gameService) : base(unitOfWork)
    {
      _logger = logger;
      _gameService= gameService;
    }


    /// <summary>
    /// Login (or autoregistrate) a player
    /// </summary>
    [HttpPost("api/login")]
    public async Task<ActionResult<Model.ServiceResponse<Model.Player>>> Login(string playerName, string playerBirthDate, bool autoRegistration = true)
    {
      try
      {
        var result = await _gameService.LoginPlayer(playerName, DateTime.Parse(playerBirthDate), autoRegistration);
        return Ok(result);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    /// <summary>
    /// Get all players
    /// </summary>
    [HttpGet("api/players")]
    [ProducesResponseType(typeof(Model.Player[]), 200)]
    [SwaggerOperation(Tags = new[] { "Players" })]
    public async Task<IActionResult> GetAllPlayers()
    {
      var players = await UnitOfWork.PlayerRepository.Get();
      return Ok(players
        .OrderBy(p => p.Name));
    }

    /// <summary>
    /// Get a player by Id
    /// </summary>
    [HttpGet("api/players/{player_id}")]
    [ProducesResponseType(typeof(Model.Player), 200)]
    [SwaggerOperation(Tags = new[] { "Players" })]
    public async Task<IActionResult> GetPlayer([FromRoute] int player_id)
    {
      var player = await UnitOfWork.PlayerRepository.GetByID(player_id);
      if (player == null)
      {
        var errorResult = new ErrorResult("Error getting player", HttpStatusCode.BadRequest, ErrorCode.PLAYERS_PLAYER_NOT_FOUND.ToString(),
          $"Player with id {player_id} not found");
        return NotFound(errorResult);
      }
      return Ok(player);
    }

    /// <summary>
    /// Get all games of a given player
    /// </summary>
    [HttpGet("api/players/{player_id}/games")]
    [ProducesResponseType(typeof(Model.Game[]), 200)]
    [SwaggerOperation(Tags = new[] { "Games" })]
    public async Task<IActionResult> GetPlayerGames([FromRoute] int player_id)
    {
      var games = await UnitOfWork.GameRepository.Get(g => g.PlayerId == player_id);
      return Ok(games);
    }

    // Game
    [HttpPost("api/players/{player_id}/game/{nr_of_digits}")]
    [SwaggerOperation(Tags = new[] { "Games" })]
    [ProducesResponseType(typeof(Model.Game), 201)]
    public async Task<IActionResult> StartNewGame([FromRoute] int player_id, [FromRoute] int nr_of_digits)
    {
      var player = await UnitOfWork.PlayerRepository.GetByID(player_id);
      if (player == null)
      {
        var errorResult = new ErrorResult("Error getting player", HttpStatusCode.BadRequest, ErrorCode.PLAYERS_PLAYER_NOT_FOUND.ToString(),
          $"Player with id {player_id} not found");
        return NotFound(errorResult);
      }

      var newGame = new Model.Game
      {
        PlayerId = player_id,
        ExpectedNumber = _gameService.PickRandomNumber(nr_of_digits),
        GameStarted = DateTime.Now,
      };

      try
      {
        UnitOfWork.GameRepository.Insert(newGame);
        await UnitOfWork.Save();
      }
      catch (Exception ex)
      {
        var errorResult = ErrorHelper.CreateErrorResultFromException(ex, "Error creating game", ErrorCode.GAMES_CREATE_GAME_ERROR.ToString());
        _logger.LogError($"{errorResult.Title}: {errorResult.Detail}. Stack trace: {ex.StackTrace}");
        return StatusCode((int)HttpStatusCode.InternalServerError, errorResult);
      }
      return CreatedAtAction("StartNewGame", new { game_id = newGame.Id });
    }
    [HttpPost]
    [Route("api/players/{player_id}/game/{game_id}/guess/{entered_number}")]
    [SwaggerOperation(Tags = new[] { "Games" })]
    [ProducesResponseType(typeof(Model.Game), 201)]
    public async Task<IActionResult> MakeGuess([FromRoute] int player_id, [FromRoute] int game_id, [FromRoute] string entered_number)
    {
      // Game check
      var game = await UnitOfWork.GameRepository.GetByID(game_id);
      if (game == null)
      {
        var errorResult = new ErrorResult("Error getting game", HttpStatusCode.BadRequest, ErrorCode.GAMES_GAME_NOT_FOUND.ToString(),
          $"Game with id {game_id} not found");
        return NotFound(errorResult);
      }
      if (game.PlayerId != player_id)
      {
        var errorResult = new ErrorResult("Error, game belongs to other player", HttpStatusCode.BadRequest, ErrorCode.GAMES_GAME_NOT_FOUND.ToString(),
          $"Game with id {game_id} does not belong to player {player_id}");
        return NotFound(errorResult);
      }

      // Player check
      var player = await UnitOfWork.PlayerRepository.GetByID(player_id);
      if (player == null)
      {
        var errorResult = new ErrorResult("Error getting player", HttpStatusCode.BadRequest, ErrorCode.PLAYERS_PLAYER_NOT_FOUND.ToString(),
          $"Player with id {player_id} not found");
        return NotFound(errorResult);
      }

      var result = _gameService.TestEqualNumbers(game.ExpectedNumber, entered_number);
      if (result == null)
      {
        return BadRequest(result);
      }

      var newAction = new Model.Action
      {
        PlayerId = player_id,
        GameId = game_id,
        Game = game,
        NumberEntered = entered_number,
        NumberOfShips = result.CorrectDigitLocations,
        NumberOfBuoys = result.CorrectDigits,
      };

      try
      {
        UnitOfWork.ActionRepository.Insert(newAction);
        await UnitOfWork.Save();
      }
      catch (Exception ex)
      {
        var errorResult = ErrorHelper.CreateErrorResultFromException(ex, "Error creating game action", ErrorCode.GAMES_CREATE_GAME_ERROR.ToString());
        _logger.LogError($"{errorResult.Title}: {errorResult.Detail}. Stack trace: {ex.StackTrace}");
        return StatusCode((int)HttpStatusCode.InternalServerError, errorResult);
      }
      return CreatedAtAction("MakeGuess", new { 
        player_id = player_id, 
        game_id = game.Id,

      });
    }

  }
}
