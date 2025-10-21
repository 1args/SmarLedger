using MassTransit;
using MassTransit.Observables;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartLedger.Common.Applications.AppServices.Extensions;
using SmartLedger.Common.Contracts.Authorization;
using SmartLedger.Common.Contracts.Options;
using SmartLedger.Common.Infrastructures.DataAccess.Abstractions;
using SmartLedger.Common.Infrastructures.DataAccess.Events;
using SmartLedger.Modules.Budgets.Host.Consumers.Consumers;
using SmartLedger.Modules.Budgets.Host.Consumers.Extensions;

// Entry point for consumers
var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((context, services) =>
{
    var configuration = context.Configuration;

    services.Configure<RabbitMqOptions>(configuration.GetSection(nameof(RabbitMqOptions)));
    services.AddScoped<IEventBus, EventBus>();

    services.AddBudgetsModule();
    services.AddDateTimeProvider();

    services
        .AddScoped(provider => new Lazy<IAuthorizationData>(provider.GetRequiredService<IAuthorizationData>))
        .AddScoped<IAuthorizationData, AuthorizationData>();

    services.AddMassTransit(cfg =>
    {
        var rabbitMqOptions = configuration.GetSection(nameof(RabbitMqOptions)).Get<RabbitMqOptions>();

        cfg.AddConsumers(typeof(BudgetCreatedEventConsumer).Assembly);

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