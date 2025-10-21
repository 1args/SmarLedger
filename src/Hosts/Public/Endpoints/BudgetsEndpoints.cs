using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Commands.CreateBudget;
using SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Commands.CreateBudgetCategory;
using SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Commands.DeleteBudget;
using SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Commands.DeleteBudgetCategory;
using SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Queries.GetBudget;
using SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Queries.GetBudgetCategoriesPage;
using SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Queries.GetBudgetCategory;
using SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Queries.GetBudgetsPage;
using SmartLedger.Modules.Budgets.Contracts.Requests.BudgetCategories;
using SmartLedger.Modules.Budgets.Contracts.Requests.Budgets;

namespace SmartLedger.Host.Public.Endpoints;

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
            .RequireAuthorization()
            .WithTags("Budgets")
            .WithOpenApi();

        endpoints.MapPost("/", CreateBudget)
            .WithName("CreateBudget")
            .WithSummary("Creates a new budget.")
            .WithDescription("Creates a new budget for the specified user, with name, start date, and end date.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        endpoints.MapPost("/{budgetId:guid}/categories", CreateBudgetCategory)
            .WithName("AddCategoryToBudget")
            .WithSummary("Creates a category to a budget.")
            .WithDescription("Creates a new category with a limit to the specified budget.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

        endpoints.MapDelete("/{budgetId:guid}/categories/{categoryId:guid}", DeleteBudgetCategory)
            .WithName("RemoveCategoryFromBudget")
            .WithSummary("Deletes a category from a budget.")
            .WithDescription("Deletes an existing category from the specified budget by its unique category ID.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);

        endpoints.MapDelete("/{budgetId:guid}", DeleteBudget)
            .WithName("DeleteBudget")
            .WithSummary("Deletes a budget by its identifier.")
            .WithDescription("Removes an existing budget identified by its unique budget ID.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);

        endpoints.MapPost("/search", GetBudgetsPage)
            .WithName("GetPaginatedBudgets")
            .WithSummary("Retrieves a paginated list of budgets for the specified user.")
            .WithDescription("Returns a paginated list of budgets associated with the given user ID, with optional filters for start and end dates.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        endpoints.MapGet("/{budgetId:guid}", GetBudget)
            .WithName("GetBudget")
            .WithSummary("Retrieves a budget by its identifier.")
            .WithDescription("Retrieves the budget details for the specified budget ID.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

        endpoints.MapPost("/{budgetId:guid}/categories/search", GetBudgetCategoriesPage)
            .WithName("GetPaginatedBudgetCategories")
            .WithSummary("Retrieves a paginated list of budget categories for the specified budget.")
            .WithDescription("Returns a paginated list of budget categories associated with the given budget ID, with optional filters for category, limits, spent amounts, status, and dates.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

        endpoints.MapGet("/{budgetId:guid}/categories/{budgetCategoryId:guid}", GetBudgetCategory)
            .WithName("GetBudgetCategory")
            .WithSummary("Retrieves a budget category by its identifier.")
            .WithDescription("Retrieves the budget category details for the specified budget and category IDs.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

        return app;
    }

    /// <summary>
    /// Handles the creation of a new budget.
    /// </summary>
    private static async Task<IResult> CreateBudget(
        [FromBody] CreateBudgetRequest request,
        [FromServices] IMessageBus bus,
        CancellationToken cancellationToken)
    {
        var command = new CreateBudgetCommand(request.Name, request.StartDate, request.EndDate);
        var response = await bus.SendAsync(command, cancellationToken);

        return Results.Ok(response);
    }

    /// <summary>
    /// Handles the deletion of an existing budget.
    /// </summary>
    private static async Task<IResult> DeleteBudget(
        [FromRoute] [BindRequired] Guid budgetId,
        [FromServices] IMessageBus bus,
        CancellationToken cancellationToken)
    {
        var command = new DeleteBudgetCommand(budgetId);
        await bus.SendAsync(command, cancellationToken);

        return Results.NoContent();
    }

    /// <summary>
    /// Handles adding a new category to an existing budget.
    /// </summary>
    private static async Task<IResult> CreateBudgetCategory(
        [FromRoute] [BindRequired] Guid budgetId,
        [FromBody] CreateBudgetCategoryRequest request,
        [FromServices] IMessageBus bus,
        CancellationToken cancellationToken)
    {
        var command = new CreateBudgetCategoryCommand(
            budgetId, request.Category, request.Limit);
        var response = await bus.SendAsync(command, cancellationToken);

        return Results.Ok(response);
    }

    /// <summary>
    /// Handles removing a category from an existing budget.
    /// </summary>
    private static async Task<IResult> DeleteBudgetCategory(
        [FromRoute] [BindRequired] Guid budgetId,
        [FromRoute] [BindRequired] Guid categoryId,
        [FromServices] IMessageBus bus,
        CancellationToken cancellationToken)
    {
        var command = new RemoveCategoryCommand(
            budgetId, categoryId);
        await bus.SendAsync(command, cancellationToken);

        return Results.NoContent();
    }

    /// <summary>
    /// Retrieves budget details by its ID.
    /// </summary>
    private static async Task<IResult> GetBudget(
        [FromRoute] [BindRequired] Guid budgetId,
        [FromServices] IMessageBus bus,
        CancellationToken cancellationToken)
    {
        var query = new GetBudgetQuery(budgetId);
        var response = await bus.QueryAsync(query, cancellationToken);

        return Results.Ok(response);
    }

    /// <summary>
    /// Retrieves a paginated list of budgets for the specified user with optional filters.
    /// </summary>
    private static async Task<IResult> GetBudgetsPage(
        [FromBody] GetBudgetsPageRequest request,
        [FromServices] IMessageBus bus,
        CancellationToken cancellationToken)
    {
        var query = new GetBudgetsPageQuery(
            request.PageNumber,
            request.PageSize,
            request.StartDate,
            request.EndDate);
        var response = await bus.QueryAsync(query, cancellationToken);

        return Results.Ok(response);
    }

    /// <summary>
    /// Retrieves budget category details by its ID.
    /// </summary>
    private static async Task<IResult> GetBudgetCategory(
        [FromRoute] [BindRequired] Guid budgetId,
        [FromRoute] [BindRequired] Guid budgetCategoryId,
        [FromServices] IMessageBus bus,
        CancellationToken cancellationToken)
    {
        var query = new GetBudgetCategoryQuery(budgetCategoryId);
        var response = await bus.QueryAsync(query, cancellationToken);

        return Results.Ok(response);
    }

    /// <summary>
    /// Retrieves a paginated list of budget categories for the specified budget with optional filters.
    /// </summary>
    private static async Task<IResult> GetBudgetCategoriesPage(
        [FromRoute] [BindRequired] Guid budgetId,
        [FromBody] GetBudgetCategoriesPageRequest request,
        [FromServices] IMessageBus bus,
        CancellationToken cancellationToken)
    {
        var query = new GetBudgetCategoriesPageQuery(
            request.PageNumber,
            request.PageSize,
            budgetId,
            request.Category,
            request.MinLimit,
            request.MaxLimit,
            request.MinSpentAmount,
            request.MaxSpentAmount,
            request.Status,
            request.StartDate,
            request.EndDate);
        var response = await bus.QueryAsync(query, cancellationToken);

        return Results.Ok(response);
    }
}