using HandlebarsDotNet;
using SmartLedger.Hosts.Api.Extensions;
using SmartLedger.Hosts.Api.Extensions.Modules;
using System.Globalization;

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
    .AddDecorators();

var app = builder.Build();

app.UseApiMiddlewares();

app.Run();