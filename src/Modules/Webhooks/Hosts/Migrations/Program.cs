using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartLedger.Common.Host.Migrations;
using SmartLedger.Common.Host.Migrations.Abstractions;
using SmartLedger.Modules.Webhooks.Hosts.Migrations;
using SmartLedger.Modules.Webhooks.Infrastructures.DataAccess.Contexts;

// Entry point for executing database migrations
var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((context, services) =>
{
    var connectionString = context.Configuration.GetConnectionString("Webhooks");

    services.AddDbContext<WebhooksDbContext>(contextBuilder => contextBuilder.UseNpgsql(
        connectionString,
        optionsBuilder => optionsBuilder.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName)));

    services
        .AddSingleton<IDatabaseMigrationsService, DatabaseMigrationsService>()
        .AddSingleton<Startup>();
});

var app = builder.Build();

var migration = app.Services.GetRequiredService<Startup>()
                ?? throw new InvalidOperationException("Unable to get Startup service.");

using var cancellationTokenSource = new CancellationTokenSource();

await migration.StartMigrationAsync(cancellationTokenSource.Token);