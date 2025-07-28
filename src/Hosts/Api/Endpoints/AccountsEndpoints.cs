using Microsoft.AspNetCore.Mvc;
using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Common.Contracts.Pagination;
using SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Commands.AddTransaction;
using SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Commands.CreateAccount;
using SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Commands.DeleteAccount;
using SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Commands.RemoveTransaction;
using SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Queries.GetAccount;
using SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Queries.GetPaginatedAccounts;
using SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Transactions.Queries.GetPaginatedTransactions;
using SmartLedger.Modules.Transactions.Contracts.Requests.Accounts;
using SmartLedger.Modules.Transactions.Contracts.Requests.Transactions;
using SmartLedger.Modules.Transactions.Contracts.Responses.Accounts;
using SmartLedger.Modules.Transactions.Contracts.Responses.Transactions;

namespace SmartLedger.Hosts.Api.Endpoints;

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
            .WithTags("Accounts")
            .WithOpenApi();

        endpoints.MapPost("/", CreateAccountAsync)
            .WithName("CreateAccount")
            .WithSummary("Creates a new account.")
            .WithDescription("Creates a new account with the specified name and user ID.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        endpoints.MapDelete("/{accountId:guid}", DeleteAccountAsync)
            .WithName("DeleteAccount")
            .WithSummary("Deletes an account.")
            .WithDescription("Removes an account specified by its unique ID.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);

        endpoints.MapPost("/{accountId:guid}/transactions", AddTransactionAsync)
            .WithName("AddTransaction")
            .WithSummary("Adds a transaction to an account.")
            .WithDescription("Adds a new transaction to the specified account with details such as amount, type, category, and notes.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

        endpoints.MapDelete("/{accountId:guid}/transactions/{transactionId:guid}", RemoveTransactionAsync)
            .WithName("RemoveTransaction")
            .WithSummary("Removes a transaction from an account")
            .WithDescription("Removes a specific transaction from the specified account.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

        endpoints.MapGet("/{accountId:guid}", GetAccountAsync)
            .WithName("GetAccount")
            .WithSummary("Retrieves an account by its identifier.")
            .WithDescription("Retrieves the account details for the specified account ID.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapPost("/{userId:guid}/accounts/search", GetPaginatedAccountsAsync)
            .WithName("GetPaginatedAccounts")
            .WithSummary("Retrieves a paginated list of accounts for the specified user.")
            .WithDescription("Returns a paginated list of accounts associated with the given user ID.")
            .WithTags("Accounts")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

        endpoints.MapPost("/{accountId:guid}/transactions/search", GetPaginatedTransactionsAsync)
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
    private static async Task<IResult> CreateAccountAsync(
        [FromBody] CreateAccountRequest request,
        [FromServices] ICommandHandler<CreateAccountCommand, Guid> handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateAccountCommand(request.Name, request.UserId);
        var response = await handler.HandleAsync(command, cancellationToken);

        return Results.Ok(response);
    }

    /// <summary>
    /// Handles the deletion of an existing account.
    /// </summary>
    private static async Task<IResult> DeleteAccountAsync(
        [FromRoute] Guid accountId,
        [FromServices] ICommandHandler<DeleteAccountCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new DeleteAccountCommand(accountId);
        await handler.HandleAsync(command, cancellationToken);

        return Results.NoContent();
    }

    /// <summary>
    /// Handles the addition of a transaction to an account.
    /// </summary>
    private static async Task<IResult> AddTransactionAsync(
        [FromRoute] Guid accountId,
        [FromBody] AddTransactionRequest request,
        [FromServices] ICommandHandler<AddTransactionCommand, Guid> handler,
        CancellationToken cancellationToken)
    {
        var command = new AddTransactionCommand(
            accountId, request.Amount, request.Type, request.Category, request.Notes);
        var response = await handler.HandleAsync(command, cancellationToken);

        return Results.Ok(response);
    }

    /// <summary>
    /// Handles the removal of a transaction from an account.
    /// </summary>
    private static async Task<IResult> RemoveTransactionAsync(
        [FromRoute] Guid accountId,
        [FromRoute] Guid transactionId,
        [FromServices] ICommandHandler<RemoveTransactionCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new RemoveTransactionCommand(accountId, transactionId);
        await handler.HandleAsync(command, cancellationToken);

        return Results.Ok();
    }

    /// <summary>
    /// Retrieves account details by its ID.
    /// </summary>
    private static async Task<IResult> GetAccountAsync(
        [FromRoute] Guid accountId,
        [FromServices] IQueryHandler<GetAccountQuery, AccountResponse> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetAccountQuery(accountId);
        var response = await handler.HandleAsync(query, cancellationToken);

        return Results.Ok(response);
    }

    /// <summary>
    /// Retrieves a paginated list of transactions for the specified account with optional filters.
    /// </summary>
    private static async Task<IResult> GetPaginatedAccountsAsync(
        [FromRoute] Guid userId,
        [FromBody] GetPaginatedAccountsRequest request,
        [FromServices] IQueryHandler<GetPaginatedAccountsQuery, PaginatedList<AccountListItem>> handler,
        CancellationToken cancellationToken)
    {
       var query = new GetPaginatedAccountsQuery(
            request.PageNumber,
            request.PageSize,
            userId,
            request.MinBalance,
            request.MaxBalance,
            request.StartDate,
            request.EndDate);

        var response = await handler.HandleAsync(query, cancellationToken);

        return Results.Ok(response);
    }

    /// <summary>
    /// Retrieves a paginated list of transactions for the specified account with optional filters.
    /// </summary>
    private static async Task<IResult> GetPaginatedTransactionsAsync(
        [FromRoute] Guid accountId,
        [FromBody] GetPaginatedTransactionsRequest request,
        [FromServices] IQueryHandler<GetPaginatedTransactionsQuery, PaginatedList<TransactionListItem>> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetPaginatedTransactionsQuery(
            request.PageNumber,
            request.PageSize,
            accountId, 
            request.MinAmount, 
            request.MaxAmount,
            request.Type,
            request.Category,
            request.StartDate,
            request.EndDate);

        var response = await handler.HandleAsync(query, cancellationToken);

        return Results.Ok(response);
    }
}