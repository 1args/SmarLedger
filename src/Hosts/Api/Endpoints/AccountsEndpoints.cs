using Microsoft.AspNetCore.Mvc;
using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Commands.AddTransaction;
using SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Commands.CreateAccount;
using SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Commands.DeleteAccount;
using SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Commands.RemoveTransaction;
using SmartLedger.Modules.Transactions.Contracts.Requests.Accounts;

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
            .Produces(StatusCodes.Status201Created)
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
        await handler.HandleAsync(command, cancellationToken);

        return Results.Created();
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
        [FromServices] ICommandHandler<AddTransactionCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new AddTransactionCommand(
            accountId, request.Amount, request.Type, request.Category, request.Notes);
        await handler.HandleAsync(command, cancellationToken);

        return Results.Ok();
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
}