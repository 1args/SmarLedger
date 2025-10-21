using SmartLedger.Host.Public.Extensions;
using SmartLedger.Hosts.Public.Extensions;
using SmartLedger.Hosts.Public.Extensions.Modules;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services
    .AddApi(configuration)
    .AddBankAccountsModule()
    .AddBudgetsModule()
    .AddSecurityModule()
    .AddReportsModule()
    .AddWebhooksModule()
    .AddNotificationsModule()
    .AddDecorators();

var app = builder.Build();

app.UseApiMiddlewares();

app.Run();