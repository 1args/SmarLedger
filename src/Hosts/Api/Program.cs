using SmartLedger.Hosts.Api.Extensions;
using SmartLedger.Modules.Transactions.Hosts.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services
    .AddApi(configuration)
    .AddTransactionsModule();

var app = builder.Build();

app.UseApiMiddlewares();

app.Run();
