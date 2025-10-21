using MassTransit;
using MassTransit.Observables;
using Microsoft.EntityFrameworkCore;
using SmartLedger.Common.Contracts.Options;
using SmartLedger.Common.Infrastructures.DataAccess.Abstractions;
using SmartLedger.Common.Infrastructures.DataAccess.Events;

namespace SmartLedger.Hosts.Public.Extensions;

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
    /// <param name="configuration">Configuration.</param>
    /// <param name="connectionString">Connection string used for the PostgreSQL database context.</param>
    /// <returns>Modified <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddMessageBroker<TDbContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        string connectionString)
        where TDbContext : DbContext
    {
        services.AddDbContext<TDbContext>(options =>
        {
            options.UseNpgsql(connectionString); 
        });

        services.AddMassTransit(cfg =>
        {
            var rabbitMqOptions = configuration.GetSection(nameof(RabbitMqOptions)).Get<RabbitMqOptions>();

            cfg.SetKebabCaseEndpointNameFormatter();

            cfg.AddTelemetryListener();

            cfg.UsingRabbitMq((context, rmqCfg) =>
            {
                rmqCfg.Host(rabbitMqOptions!.HostName, rabbitMqOptions.VirtualHost, hostCfg =>
                {
                    rmqCfg.ConnectReceiveObserver(new ReceiveObservable());
                    rmqCfg.ConnectSendObserver(new SendObservable());
                    hostCfg.Username(rabbitMqOptions.Username);
                    hostCfg.Password(rabbitMqOptions.Password);
                });
                rmqCfg.ConfigureEndpoints(context);
            });

            cfg.AddEntityFrameworkOutbox<TDbContext>(outBoxCfg =>
            {
                outBoxCfg.QueryDelay = TimeSpan.FromSeconds(30);
                outBoxCfg.UsePostgres().UseBusOutbox();
            });
        });

        services.AddScoped<IEventBus, EventBus>();

        return services;
    }
}