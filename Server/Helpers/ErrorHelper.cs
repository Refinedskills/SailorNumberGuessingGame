using SailorNumberGuessingGame.Shared.Models.Enums;
using SailorNumberGuessingGame.Shared.Models.Results;
using SailorNumberGuessingGame.Shared.Models;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using SailorNumberGuessingGame.Server.Extensions;

namespace SailorNumberGuessingGame.Server.Helpers;

public static class ErrorHelper
{
  public static Dictionary<string, string> MethodToErrorCodeMapping { get; } = new Dictionary<string, string>
  {
      {"GetAllPlayers", ErrorCode.PLAYERS_GET_PLAYERS_ERROR.ToString()},
      {"GetPlayer", ErrorCode.PLAYERS_GET_PLAYER_ERROR.ToString()},

      {"StartNewGame", ErrorCode.PLAYERS_GET_PLAYER_ERROR.ToString()},
  };

  public static string GetJsonPathIdentifier(string json, string errorField, string errorValue = "", int? errorIndex = null)
  {
    var jsonPath = string.Empty;

    try
    {
      var jsonObject = JObject.Parse(json);

      if (errorIndex == null)
      {
        JProperty property;

        if (!string.IsNullOrEmpty(errorValue))
        {
          property = jsonObject.Descendants().OfType<JProperty>().FirstOrDefault(p =>
            string.Equals(p.Name, errorField, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(p.Value.ToString(), errorValue, StringComparison.OrdinalIgnoreCase));
        }
        else
        {
          property = jsonObject.Descendants().OfType<JProperty>().FirstOrDefault(p =>
            string.Equals(p.Name, errorField, StringComparison.OrdinalIgnoreCase));
        }

        if (property != null)
        {
          jsonPath = $"$.{property.Path}";
        }
        else
        {
          JToken token = jsonObject.SelectToken(errorField.ToCamelCaseJsonPath());
          jsonPath = $"$.{(token != null ? token.Path : errorField.ToCamelCaseJsonPath())}";
        }

      }
      else
      {
        //Array
        var property = jsonObject.Descendants().OfType<JProperty>().FirstOrDefault(p => string.Equals(p.Name, errorField, StringComparison.OrdinalIgnoreCase));

        if (property != null && property.Value.Type == JTokenType.Array)
        {
          jsonPath = $"$.{property.Path}[{errorIndex}]";
        }
      }
    }
    catch (Exception)
    {
      return string.Empty;
    }

    return jsonPath;

  }

  public static string GetErrorCodeForMethod(string methodName, bool hasInputModel = false)
  {
    MethodToErrorCodeMapping.TryGetValue(methodName, out var errorCode);

    if (!string.IsNullOrEmpty(errorCode))
    {
      return errorCode;
    }
    else
    {
      if (hasInputModel)
      {
        return ErrorCode.INPUT_MODEL_ERROR.ToString();
      }
      else
      {
        return ErrorCode.UNKNOWN_ERROR.ToString();
      }
    }
  }

  public static ErrorResult CreateErrorResultFromException(Exception exception, string title, string errorCode, HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError)
  {
    var errorMessage = $"{exception.Message}";
    if (exception.InnerException != null && exception.InnerException.Message != exception.Message)
    {
      errorMessage += $" {exception.InnerException.Message}";
    }
    var errorResult = new ErrorResult(title, httpStatusCode, errorCode, errorMessage);
    return errorResult;
  }
}
