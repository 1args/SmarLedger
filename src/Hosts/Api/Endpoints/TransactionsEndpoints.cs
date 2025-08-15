using Microsoft.AspNetCore.Mvc;
using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Hosts.Api.Features.RateLimiting;
using SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Transactions.Commands.CategorizeTransaction;
using SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Transactions.Commands.UpdateTransactionAmount;
using SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Transactions.Queries.GetTransaction;
using SmartLedger.Modules.Transactions.Contracts.Requests.Transactions;
using SmartLedger.Modules.Transactions.Contracts.Responses.Transactions;

namespace SmartLedger.Hosts.Api.Endpoints;

/// <summary>
/// Maps endpoints related to transaction operations.
/// </summary>
public static class TransactionsEndpoints
{
    /// <summary>
    /// Registers all transaction-related routes.
    /// </summary>
    /// <param name="app">Application's endpoint route builder.</param>
    /// <returns>Modified <see cref="IEndpointRouteBuilder"/>.</returns>
    public static IEndpointRouteBuilder MapTransactionsEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup("/transactions")
            .RequireAuthorization()
            .RequireRateLimiting(RateLimitPolicy.Global)
            .RequireRateLimiting(RateLimitPolicy.IpAddress)
            .WithTags("Transactions")
            .WithOpenApi();

        endpoints.MapPatch("/{transactionId:guid}/amount", UpdateTransactionAmountAsync)
            .RequireRateLimiting(RateLimitPolicy.WriteOperations)
            .WithName("UpdateTransactionAmount")
            .WithSummary("Updates the amount of a specific transaction.")
            .WithDescription("Modifies the amount of an existing transaction identified by its ID.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

        endpoints.MapPatch("/{transactionId:guid}/category", CategorizeTransactionAmountAsync)
            .RequireRateLimiting(RateLimitPolicy.WriteOperations)
            .WithName("CategorizeTransactionAmount")
            .WithSummary("Updates the category of a specific transaction.")
            .WithDescription("Assigns a new category to an existing transaction identified by its ID.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

        endpoints.MapGet("/{transactionId:guid}", GetTransactionAsync)
            .RequireRateLimiting(RateLimitPolicy.ReadOperations)
            .WithName("GetTransaction")
            .WithSummary("Retrieves a transaction by its identifier.")
            .WithDescription("Retrieves the transaction details for the specified transaction ID.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

        return app;
    }

    /// <summary>
    /// Updates the amount of a specific transaction.
    /// </summary>
    private static async Task<IResult> UpdateTransactionAmountAsync(
        [FromRoute] Guid transactionId,
        [FromBody] UpdateTransactionAmountRequest request,
        [FromServices] ICommandHandler<UpdateTransactionAmountCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateTransactionAmountCommand(transactionId, request.NewAmount);
        await handler.HandleAsync(command, cancellationToken);

        return Results.Ok();
    }

    /// <summary>
    /// Updates the category of a specific transaction.
    /// </summary>
    private static async Task<IResult> CategorizeTransactionAmountAsync(
        [FromRoute] Guid transactionId,
        [FromBody] CategorizeTransactionRequest request,
        [FromServices] ICommandHandler<CategorizeTransactionCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new CategorizeTransactionCommand(transactionId, request.NewCategory);
        await handler.HandleAsync(command, cancellationToken);

        return Results.Ok();
    }

    /// <summary>
    /// Retrieves transaction details by its ID.
    /// </summary>
    private static async Task<IResult> GetTransactionAsync(
        [FromRoute] Guid transactionId,
        [FromServices] IQueryHandler<GetTransactionQuery, TransactionResponse> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetTransactionQuery(transactionId);
        var response = await handler.HandleAsync(query, cancellationToken);

        return Results.Ok(response);
    }
}