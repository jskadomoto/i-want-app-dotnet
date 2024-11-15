public class UserCreator {

  private readonly UserManager<IdentityUser> _userManager;
  public UserCreator(UserManager<IdentityUser> userManager) {
    _userManager = userManager;
  }

  public async Task<(IdentityResult, string)> Create(string email, string password, List<Claim> claims) {
    var user = new IdentityUser { UserName = email, Email = email };
    var result = await _userManager.CreateAsync(user, password);

    if (!result.Succeeded)
      return (result, string.Empty);

    foreach (var claim in claims)
    {
      var claimResult = await _userManager.AddClaimAsync(user, claim);

      if (!claimResult.Succeeded)
        return (claimResult, string.Empty);
    }

    return (IdentityResult.Success, user.Id);
  }
}