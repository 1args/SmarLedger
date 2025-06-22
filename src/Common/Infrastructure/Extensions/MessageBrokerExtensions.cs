using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SmartLedger.Common.Contracts.Options;

namespace SmartLedger.Common.Infrastructure.Extensions;

/// <summary>
/// Extension for configuring a message broker using MassTransit and RabbitMQ.
/// </summary>
public static class MessageBrokerExtensions
{
    /// <summary>
    /// Registers the message broker with MassTransit and configures it
    /// to use RabbitMQ with Entity Framework outbox support.
    /// </summary>
    /// <typeparam name="TDbContext">Type of the <see cref="DbContext"/> used for the outbox pattern.</typeparam>
    /// <param name="services">Service collection.</param>
    /// <param name="connectionString">Connection string used for the PostgreSQL database context.</param>
    /// <returns>Modified <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddMessageBroker<TDbContext>(
        IServiceCollection services,
        string connectionString)
        where TDbContext : DbContext
    {
        services.AddDbContext<TDbContext>(options =>
        {
            options.UseNpgsql(connectionString); 
        });

        services.AddMassTransit(cfg =>
        {
            using var serviceProvider = services.BuildServiceProvider();

            var options = serviceProvider.GetRequiredService<IOptions<RabbitMqOptions>>().Value;

            cfg.SetKebabCaseEndpointNameFormatter();

            cfg.UsingRabbitMq((context, rmqCfg) =>
            {
                rmqCfg.Host(options.HostName, options.VirtualHost, hostCfg =>
                {
                    hostCfg.Username(options.Username);
                    hostCfg.Password(options.Password);
                });
                rmqCfg.ConfigureEndpoints(context);
            });

            cfg.AddEntityFrameworkOutbox<TDbContext>(outBoxCfg =>
            {
                outBoxCfg.QueryDelay = TimeSpan.FromSeconds(30);
                outBoxCfg.UsePostgres().UseBusOutbox();
            });
        });

        return services;
    }
}