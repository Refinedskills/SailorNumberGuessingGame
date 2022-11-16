using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SailorNumberGuessingGame.Shared.Models.Results;
public class ErrorResult
{
  public ErrorResult(string title, HttpStatusCode status, string code, string detail, List<ErrorInfo> errors = null)
  {
    Title = title;
    Status = (int)status;
    Code = code;
    Detail = detail;
    TraceId = Guid.NewGuid().ToString();

    Errors = errors;
  }

  public string Title { get; }
  public int Status { get; }
  public string Code { get; }
  public string Detail { get; }
  public string TraceId { get; }
  public List<ErrorInfo> Errors { get; private set; }

  public void AddErrors(List<ErrorInfo> errors)
  {
    if (errors == null) return;

    if (Errors == null) Errors = new List<ErrorInfo>();

    Errors.AddRange(errors);
  }

  public void AddError(ErrorInfo error)
  {

    if (error == null) return;

    if (Errors == null) Errors = new List<ErrorInfo>();

    Errors.Add(error);
  }
}

