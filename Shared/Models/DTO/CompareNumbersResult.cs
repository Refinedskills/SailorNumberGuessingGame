using SailorNumberGuessingGame.Shared.Models.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailorNumberGuessingGame.Shared.Models.DTO;

public class CompareNumbersResult
{
  public CompareNumbersResultCode CompareNumbersResultCode { get; set; }

  public int? NeededDigits { get; set; }
  public int CorrectDigitLocations { get; set; }
  public int CorrectDigits { get; set; }
}
