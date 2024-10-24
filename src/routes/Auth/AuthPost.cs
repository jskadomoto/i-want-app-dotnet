namespace IWantApp.Domain.Products;

public class AuthPost
{
  public static string Template => "/auth";
  public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
  public static Delegate Handler => Action;

  [AllowAnonymous]
  public static async Task<IResult> Action(AuthRequest authRequest, IConfiguration configuration, UserManager<IdentityUser> userManager, ILogger<AuthPost> logger)
  {
    logger.LogInformation("Getting auth token");

    var user = await userManager.FindByEmailAsync(authRequest.Email);

    if (user == null)
    {
      logger.LogError("User not found");
      return Results.BadRequest();
    }

    if (!await userManager.CheckPasswordAsync(user, authRequest.Password))
    {
      logger.LogError("Wrong password");
      return Results.BadRequest();
    }

    var claims = await userManager.GetClaimsAsync(user);

    var subject = new ClaimsIdentity(new Claim[] {
        new Claim(ClaimTypes.Email, authRequest.Email),
        new Claim(ClaimTypes.NameIdentifier, user.Id),
      });

    subject.AddClaims(claims);

    var secretKey = configuration["JwtBearerTokenSettings:SecretKey"] ?? string.Empty;
    var key = Encoding.ASCII.GetBytes(secretKey);
    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = subject,
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
      Audience = configuration["JwtBearerTokenSettings:Audience"],
      Issuer = configuration["JwtBearerTokenSettings:Issuer"],
      Expires = DateTime.UtcNow.AddSeconds(3600) /* 1h in seconds */
    };

    var tokenHandler = new JwtSecurityTokenHandler();
    var token = tokenHandler.CreateToken(tokenDescriptor);

    logger.LogInformation("Auth Success");

    return Results.Ok(new
    {
      token = tokenHandler.WriteToken(token)
    });
  }
}