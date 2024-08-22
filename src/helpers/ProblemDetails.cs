using Flunt.Notifications;
namespace IWantApp.Domain;

public static class ProblemDetailsExtensions
{
  public static Dictionary<string, string[]> ConvertToProblemDetails(this IReadOnlyCollection<Notification> notifications)
  {
    return notifications.GroupBy(group => group.Key)
        .ToDictionary(g => g.Key, g => g.Select(err => err.Message).ToArray());
  }
}