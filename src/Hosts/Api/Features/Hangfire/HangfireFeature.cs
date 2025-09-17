using Hangfire;
using Hangfire.PostgreSql;
using Hangfire.PostgreSql.Factories;
using SmartLedger.Common.Hosts.Features.Abstractions;

namespace SmartLedger.Hosts.Api.Features.Hangfire;

public class HangfireFeature : IAppFeature
{
    public void UseFeature(IServiceCollection services, IConfiguration configuration)
    {
        var connectingString = configuration.GetConnectionString("DefaultConnection");
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