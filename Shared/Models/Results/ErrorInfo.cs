using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailorNumberGuessingGame.Shared.Models.Results;
/// <summary>
/// Supply information about an error
/// </summary>
public class ErrorInfo
{
  /// <summary>
  /// Type of error. Use ErrorType for this (will be returned as string)
  /// </summary>
  public string Type { get; set; }
  /// <summary>
  /// The code of the error. Use ErrorCode for this (will be returned as string)
  /// </summary>
  public string Code { get; set; }
  /// <summary>
  /// The identifier can contain extra information about the value that caused the error depending on the error type
  /// </summary>
  ///<remarks>
  /// Identifier can for example contain a jsonpath (for error type Json), parameter name (for error type path, query), etc.
  /// </remarks>
  public string Identifier { get; set; }
  /// <summary>
  /// The reason the error occured
  /// </summary>
  public string Reason { get; set; }
}
