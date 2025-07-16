using SmartLedger.Hosts.Api.Extensions;
using SmartLedger.Hosts.Api.Extensions.Modules;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services
    .AddApi(configuration)
    .AddTransactionsModule()
    .AddBudgetsModule()
    .AddDecorators();

var app = builder.Build();

app.UseApiMiddlewares();

app.Run();