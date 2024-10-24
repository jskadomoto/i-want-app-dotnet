public static class LoggingExtensions
{
  public static IHostBuilder AddSerilogLogging(this IHostBuilder hostBuilder, IConfiguration configuration)
  {
    hostBuilder.UseSerilog((context, _, config) =>
    {
      var connectionString = configuration["ConnectionString:IWantDb"];

      config.WriteTo.Console()
                .WriteTo.MSSqlServer(
                    connectionString,
                    new MSSqlServerSinkOptions
                  {
                    AutoCreateSqlTable = true,
                    TableName = "LogAPI"
                  });
    });

    return hostBuilder;
  }
}
