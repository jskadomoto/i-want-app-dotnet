namespace IWantApp.Domain;

public static class ProblemDetailsExtensions
{
  public static Dictionary<string, string[]> ConvertToProblemDetails(this IReadOnlyCollection<Notification> notifications)
  {
    return notifications.GroupBy(group => group.Key)
        .ToDictionary(g => g.Key, g => g.Select(err => err.Message).ToArray());
  }

  public static Dictionary<string, string[]> ConvertToProblemDetails(this IEnumerable<IdentityError> error)
  {
    var dictionary = new Dictionary<string, string[]>();
    dictionary.Add("Error", error.Select(err => err.Description).ToArray());
    return dictionary;
  }
}