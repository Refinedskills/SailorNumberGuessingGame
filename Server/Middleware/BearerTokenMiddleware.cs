namespace SailorNumberGuessingGame.Server.Middleware
{
  public class BearerTokenMiddleware
  {
    private readonly RequestDelegate _next;

    public BearerTokenMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
      string authHeader = context.Request.Headers["Authorization"];
      if (!string.IsNullOrEmpty(authHeader) && !authHeader.StartsWith("bearer", System.StringComparison.OrdinalIgnoreCase))
      {
        context.Request.Headers["Authorization"] = "Bearer " + authHeader;
      }
      await _next(context);
    }
  }
}
