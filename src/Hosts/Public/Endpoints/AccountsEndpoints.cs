using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.BankAccounts.Applications.Handlers.Contexts.Accounts.Commands.CreateAccount;
using SmartLedger.Modules.BankAccounts.Applications.Handlers.Contexts.Accounts.Commands.CreateTransaction;
using SmartLedger.Modules.BankAccounts.Applications.Handlers.Contexts.Accounts.Commands.DeleteAccount;
using SmartLedger.Modules.BankAccounts.Applications.Handlers.Contexts.Accounts.Commands.DeleteTransaction;
using SmartLedger.Modules.BankAccounts.Applications.Handlers.Contexts.Accounts.Queries.GetAccount;
using SmartLedger.Modules.BankAccounts.Applications.Handlers.Contexts.Accounts.Queries.GetAccountsPage;
using SmartLedger.Modules.BankAccounts.Applications.Handlers.Contexts.Accounts.Queries.GetTransaction;
using SmartLedger.Modules.BankAccounts.Applications.Handlers.Contexts.Accounts.Queries.GetTransactionsPage;
using SmartLedger.Modules.BankAccounts.Contracts.Requests.Accounts;
using SmartLedger.Modules.BankAccounts.Contracts.Requests.Transactions;

namespace SmartLedger.Host.Public.Endpoints;

/// <summary>
/// Maps endpoints related to account operations.
/// </summary>
public static class AccountsEndpoints
{
    /// <summary>
    /// Registers all account-related routes.
    /// </summary>
    /// <param name="app">Application's endpoint route builder.</param>
    /// <returns>Modified <see cref="IEndpointRouteBuilder"/>.</returns>
    public static IEndpointRouteBuilder MapAccountsEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup("/accounts")
            .RequireAuthorization()
            .WithTags("Accounts")
            .WithOpenApi();

        endpoints.MapPost("/", CreateAccount)
            .WithName("CreateAccount")
            .WithSummary("Creates a new account.")
            .WithDescription("Creates a new account with the specified name and user ID.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        endpoints.MapDelete("/{accountId:guid}", DeleteAccount)
            .WithName("DeleteAccount")
            .WithSummary("Deletes an account.")
            .WithDescription("Deletes an account specified by its unique ID.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);

        endpoints.MapPost("/{accountId:guid}/transactions", CreateTransaction)
            .WithName("AddTransaction")
            .WithSummary("Creates a transaction to an account.")
            .WithDescription("Creates a new transaction to the specified account with details such as amount, type, category, and notes.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

        endpoints.MapDelete("/{accountId:guid}/transactions/{transactionId:guid}", DeleteTransaction)
            .WithName("RemoveTransaction")
            .WithSummary("Deletes a transaction from an account")
            .WithDescription("Deletes a specific transaction from the specified account.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

        endpoints.MapGet("/{accountId:guid}/transactions/{transactionId:guid}", GetTransaction)
            .WithName("GetTransaction")
            .WithSummary("Retrieves a transaction by its identifier.")
            .WithDescription("Retrieves the transaction details for the specified transaction ID.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

        endpoints.MapGet("/{accountId:guid}", GetAccount)
            .WithName("GetAccount")
            .WithSummary("Retrieves an account by its identifier.")
            .WithDescription("Retrieves the account details for the specified account ID.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapPost("/accounts/me", GetAccountsPage)
            .WithName("GetPaginatedAccounts")
            .WithSummary("Retrieves a paginated list of accounts for the specified user.")
            .WithDescription("Returns a paginated list of accounts associated with the given user ID.")
            .WithTags("Accounts")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

        endpoints.MapPost("/{accountId:guid}/transactions/search", GetTransactionsPage)
            .WithName("GetPaginatedTransactions")
            .WithSummary("Retrieves a paginated list of transactions for the specified account.")
            .WithDescription("Returns a paginated list of transactions associated with the given account ID.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

        return app;
    }

    /// <summary>
    /// Handles the creation of a new account.
    /// </summary>
    private static async Task<IResult> CreateAccount(
        [FromBody] CreateAccountRequest request,
        [FromServices] IMessageBus bus,
        CancellationToken cancellationToken)
    {
        var command = new CreateAccountCommand(request.Name);
        var response = await bus.SendAsync(command, cancellationToken);

        return Results.Ok(response);
    }

    /// <summary>
    /// Handles the deletion of an existing account.
    /// </summary>
    private static async Task<IResult> DeleteAccount(
        [FromRoute] [BindRequired] Guid accountId,
        [FromServices] IMessageBus bus,
        CancellationToken cancellationToken)
    {
        var command = new DeleteAccountCommand(accountId);
        await bus.SendAsync(command, cancellationToken);

        return Results.NoContent();
    }

    /// <summary>
    /// Handles the addition of a transaction to an account.
    /// </summary>
    private static async Task<IResult> CreateTransaction(
        [FromRoute] [BindRequired] Guid accountId,
        [FromBody] CreateTransactionRequest request,
        [FromServices] IMessageBus bus,
        CancellationToken cancellationToken)
    {
        var command = new CreateTransactionCommand(
            accountId, request.Amount, request.Type, request.Category, request.Notes);
        var response = await bus.SendAsync(command, cancellationToken);

        return Results.Ok(response);
    }

    /// <summary>
    /// Handles the removal of a transaction from an account.
    /// </summary>
    private static async Task<IResult> DeleteTransaction(
        [FromRoute] [BindRequired] Guid accountId,
        [FromRoute] [BindRequired] Guid transactionId,
        [FromServices] IMessageBus bus,
        CancellationToken cancellationToken)
    {
        var command = new DeleteTransactionCommand(accountId, transactionId);
        await bus.SendAsync(command, cancellationToken);

        return Results.Ok();
    }

    /// <summary>
    /// Retrieves transaction details by its ID.
    /// </summary>
    private static async Task<IResult> GetTransaction(
        [FromRoute] [BindRequired] Guid accountId,
        [FromRoute] [BindRequired] Guid transactionId,
        [FromServices] IMessageBus bus,
        CancellationToken cancellationToken)
    {
        var query = new GetTransactionQuery(accountId, transactionId);
        var response = await bus.QueryAsync(query, cancellationToken);

        return Results.Ok(response);
    }

    /// <summary>
    /// Retrieves account details by its ID.
    /// </summary>
    private static async Task<IResult> GetAccount(
        [FromRoute] [BindRequired] Guid accountId,
        [FromServices] IMessageBus bus,
        CancellationToken cancellationToken)
    {
        var query = new GetAccountQuery(accountId);
        var response = await bus.QueryAsync(query, cancellationToken);

        return Results.Ok(response);
    }

    /// <summary>
    /// Retrieves a paginated list of transactions for the specified account with optional filters.
    /// </summary>
    private static async Task<IResult> GetAccountsPage(
        [FromBody] GetAccountsPageRequest request,
        [FromServices] IMessageBus bus,
        CancellationToken cancellationToken)
    {
       var query = new GetAccountsPageQuery(
            request.PageNumber,
            request.PageSize,
            request.MinBalance,
            request.MaxBalance,
            request.StartDate,
            request.EndDate);
        var response = await bus.QueryAsync(query, cancellationToken);

        return Results.Ok(response);
    }

    /// <summary>
    /// Retrieves a paginated list of transactions for the specified account with optional filters.
    /// </summary>
    private static async Task<IResult> GetTransactionsPage(
        [FromRoute] [BindRequired] Guid accountId,
        [FromBody] GetTransactionsPageRequest request,
        [FromServices] IMessageBus bus,
        CancellationToken cancellationToken)
    {
        var query = new GetTransactionsPageQuery(
            request.PageNumber,
            request.PageSize,
            accountId, 
            request.MinAmount, 
            request.MaxAmount,
            request.Type,
            request.Category,
            request.StartDate,
            request.EndDate);
        var response = await bus.QueryAsync(query, cancellationToken);

        return Results.Ok(response);
    }
}