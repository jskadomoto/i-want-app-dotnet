using System.Security.Claims;

public static class HttpContextExtensions
{
  public static string GetUserId(this HttpContext httpContext)
  {
    return httpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
  }
}