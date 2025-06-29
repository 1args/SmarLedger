using SmartLedger.Common.Cqrs.Extensions;
using SmartLedger.Hosts.Api.Extensions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services
    .AddApi(configuration)
    .AddTransactionsModule()
    .AddDecorators();

var app = builder.Build();

app.UseApiMiddlewares();

app.Run();
