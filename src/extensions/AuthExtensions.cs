public static class AuthenticationExtensions
{
  public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddAuthentication(options =>
    {
      options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
      options.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["JwtBearerTokenSettings:Issuer"],
        ValidAudience = configuration["JwtBearerTokenSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtBearerTokenSettings:SecretKey"])),
        ClockSkew = TimeSpan.Zero
      };
    });

    return services;
  }
  public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
  {
    services.AddAuthorization(options =>
    {
      options.FallbackPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
              .RequireAuthenticatedUser()
              .Build();

      options.AddPolicy("EmployeePolicy", p => p.RequireAuthenticatedUser().RequireClaim("EmployeeCode"));
      options.AddPolicy("EmployeeSuperAdminPolicy", p => p.RequireAuthenticatedUser().RequireClaim("EmployeeCode", "001"));
      options.AddPolicy("CpfPolicy", p => p.RequireAuthenticatedUser().RequireClaim("Cpf"));
    });

    return services;
  }

}
