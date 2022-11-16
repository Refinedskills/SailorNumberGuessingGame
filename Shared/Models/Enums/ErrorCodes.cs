﻿namespace SailorNumberGuessingGame.Shared.Models.Enums;

public enum ErrorCode
{
  // Players
  PLAYERS_GET_PLAYERS_ERROR,
  PLAYERS_GET_PLAYER_ERROR,
  PLAYERS_PLAYER_NOT_FOUND,
  PLAYERS_CREATE_PLAYER_ERROR,

  // Games
  GAMES_CREATE_GAME_ERROR,
  GAMES_GAME_NOT_FOUND,

  // Actions
  ACTIONS_CREATE_ACTION_ERROR,

  INPUT_MODEL_ERROR,
  INPUT_MODEL_REQUIRED_FIELD,
  INPUT_MODEL_INVALID_FIELD,

  UNKNOWN_ERROR,
  AUTHENTICATION_USER_MISSING
}