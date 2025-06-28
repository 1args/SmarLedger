using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Transactions.Commands.CategorizeTransaction;
using SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Transactions.Commands.UpdateTransactionAmount;
using SmartLedger.Modules.Transactions.Contracts.Requests.Transactions;

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
            .WithTags("Transactions")
            .WithOpenApi();

        endpoints.MapPatch("/{transactionId:guid}/amount", UpdateTransactionAmountAsync)
            .WithName("UpdateTransactionAmount")
            .WithSummary("Updates the amount of a specific transaction.")
            .WithDescription("Modifies the amount of an existing transaction identified by its ID.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

        endpoints.MapPatch("/{transactionId:guid}/category", CategorizeTransactionAmountAsync)
            .WithName("CategorizeTransactionAmount")
            .WithSummary("Updates the category of a specific transaction.")
            .WithDescription("Assigns a new category to an existing transaction identified by its ID.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

        return app;
    }

    /// <summary>
    /// Updates the amount of a specific transaction.
    /// </summary>
    private static async Task<IResult> UpdateTransactionAmountAsync(
        [FromRoute, BindRequired] Guid transactionId,
        [FromBody, BindRequired] UpdateTransactionAmountRequest request,
        [FromServices] UpdateTransactionAmountCommandHandler handler,
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
        [FromRoute, BindRequired] Guid transactionId,
        [FromBody, BindRequired] CategorizeTransactionRequest request,
        [FromServices] CategorizeTransactionCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new CategorizeTransactionCommand(transactionId, request.NewCategory);
        await handler.HandleAsync(command, cancellationToken);

        return Results.Ok();
    }
}