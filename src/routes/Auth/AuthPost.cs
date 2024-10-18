using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace IWantApp.Domain.Products;

public class AuthPost
{
  public static string Template => "/auth";
  public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
  public static Delegate Handler => Action;

  [AllowAnonymous]
  public static IResult Action(AuthRequest authRequest, IConfiguration configuration, UserManager<IdentityUser> userManager)
  {
    var user = userManager.FindByEmailAsync(authRequest.Email).Result;
    if (!userManager.CheckPasswordAsync(user, authRequest.Password).Result)
    {
      Results.BadRequest();
    }

    if (user == null)
    {
      Results.BadRequest();
    }

    var claims = userManager.GetClaimsAsync(user).Result;

    var subject = new ClaimsIdentity(new Claim[] {
        new Claim(ClaimTypes.Email, authRequest.Email),
        new Claim(ClaimTypes.NameIdentifier, user.Id),
      });

    subject.AddClaims(claims);

    var key = Encoding.ASCII.GetBytes(configuration["JwtBearerTokenSettings:SecretKey"]);
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

    return Results.Ok(new
    {
      token = tokenHandler.WriteToken(token)
    });
  }
}