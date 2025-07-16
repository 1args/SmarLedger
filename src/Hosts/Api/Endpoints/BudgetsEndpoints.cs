using Microsoft.AspNetCore.Mvc;
using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Commands.AddCategory;
using SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Commands.CreateBudget;
using SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Commands.DeleteBudget;
using SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Commands.RemoveCategory;
using SmartLedger.Modules.Budgets.Contracts.Requests;

namespace SmartLedger.Hosts.Api.Endpoints;

/// <summary>
/// Maps endpoints related to budgets operations.
/// </summary>
public static class BudgetsEndpoints 
{
    /// <summary>
    /// Registers all budgets-related routes.
    /// </summary>
    /// <param name="app">Application's endpoint route builder.</param>
    /// <returns>Modified <see cref="IEndpointRouteBuilder"/>.</returns>
    public static IEndpointRouteBuilder MapBudgetsEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup("/budgets")
            .WithTags("Budgets")
            .WithOpenApi();

        endpoints.MapPost("/", CreateBudgetAsync)
            .WithName("CreateBudget")
            .WithSummary("Creates a new budget.")
            .WithDescription("Creates a new budget for the specified user, with name, start date, and end date.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        endpoints.MapPost("/{budgetId:guid}/categories", AddCategoryAsync)
            .WithName("AddCategoryToBudget")
            .WithSummary("Adds a category to a budget.")
            .WithDescription("Adds a new category with a limit to the specified budget.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

        endpoints.MapDelete("/{budgetId:guid}/categories/{categoryId:guid}", RemoveCategoryAsync)
            .WithName("RemoveCategoryFromBudget")
            .WithSummary("Removes a category from a budget.")
            .WithDescription("Removes an existing category from the specified budget by its unique category ID.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);

        endpoints.MapDelete("/{budgetId:guid}", DeleteBudgetAsync)
            .WithName("DeleteBudget")
            .WithSummary("Deletes a budget by its identifier.")
            .WithDescription("Removes an existing budget identified by its unique budget ID.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);

        return app;
    }

    /// <summary>
    /// Handles the creation of a new budget.
    /// </summary>
    private static async Task<IResult> CreateBudgetAsync(
        [FromBody] CreateBudgetRequest request,
        [FromServices] ICommandHandler<CreateBudgetCommand, Guid> handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateBudgetCommand(
            request.UserId, request.Name, request.StartDate, request.EndDate);
        var response = await handler.HandleAsync(command, cancellationToken);

        return Results.Ok(response);
    }

    private static async Task<IResult> AddCategoryAsync(
        [FromRoute] Guid budgetId,
        [FromBody] AddCategoryRequest request,
        [FromServices] ICommandHandler<AddCategoryCommand, Guid> handler,
        CancellationToken cancellationToken)
    {
        var command = new AddCategoryCommand(
            budgetId, request.Category, request.Limit);
        var response = await handler.HandleAsync(command, cancellationToken);

        return Results.Ok(response);
    }

    private static async Task<IResult> RemoveCategoryAsync(
        [FromRoute] Guid budgetId,
        [FromRoute] Guid categoryId,
        [FromServices] ICommandHandler<RemoveCategoryCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new RemoveCategoryCommand(
            budgetId, categoryId);
        await handler.HandleAsync(command, cancellationToken);

        return Results.NoContent();
    }

    /// <summary>
    /// Handles the deletion of an existing budget.
    /// </summary>
    private static async Task<IResult> DeleteBudgetAsync(
        [FromRoute] Guid budgetId,
        [FromServices] ICommandHandler<DeleteBudgetCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new DeleteBudgetCommand(budgetId); 
        await handler.HandleAsync(command, cancellationToken);

        return Results.NoContent();
    }
}
