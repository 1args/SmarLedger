using MassTransit;
using MassTransit.Observables;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartLedger.Common.Applications.AppServices.Extensions;
using SmartLedger.Common.Contracts.Options;
using SmartLedger.Hosts.Public.Extensions.Modules;
using SmartLedger.Modules.Notifications.Host.Consumers.Consumers;

// Entry point for consumers
var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((context, services) =>
{
    var configuration = context.Configuration;

    services.Configure<RabbitMqOptions>(configuration.GetSection(nameof(RabbitMqOptions)));

    services.AddNotificationsModule(configuration);
    services.AddDateTimeProvider();

    services.AddMassTransit(cfg =>
    {
        var rabbitMqOptions = configuration.GetSection(nameof(RabbitMqOptions)).Get<RabbitMqOptions>();

        cfg.AddConsumers(typeof(NotificationSentEventConsumer).Assembly);

        cfg.SetKebabCaseEndpointNameFormatter();
        cfg.AddTelemetryListener();

        cfg.UsingRabbitMq((registrationContext, rmqCfg) =>
        {
            rmqCfg.Host(rabbitMqOptions!.HostName, rabbitMqOptions.VirtualHost, hostCfg =>
            {
                rmqCfg.ConnectReceiveObserver(new ReceiveObservable());
                rmqCfg.ConnectSendObserver(new SendObservable());
                hostCfg.Username(rabbitMqOptions.Username);
                hostCfg.Password(rabbitMqOptions.Password);
            });
            rmqCfg.ConfigureEndpoints(registrationContext);
        });
    });
});

var app = builder.Build();

await app.RunAsync();