using Hangfire;
using Hangfire.PostgreSql;
using Hangfire.PostgreSql.Factories;
using SmartLedger.Common.Host.Features.Abstractions;

namespace SmartLedger.Host.Public.Features.Hangfire;

/// <summary>
/// Feature for configuring Hangfire.
/// </summary>
public class HangfireFeature : IAppFeature
{
    /// <inheritdoc />
    public void UseFeature(IServiceCollection services, IConfiguration configuration)
    {
        var connectingString = configuration.GetConnectionString("BackgroundJobs");
        var connectionFactory = new NpgsqlConnectionFactory(connectingString, new PostgreSqlStorageOptions());

        services.AddHangfire(hangfireConfiguration =>
        {
            hangfireConfiguration.UsePostgreSqlStorage(options =>
            {
                options.UseNpgsqlConnection(connectingString);
                options.UseConnectionFactory(connectionFactory);
            });
        });

        JobStorage.Current = new PostgreSqlStorage(connectionFactory);
        services.AddHangfireServer();
    }
}