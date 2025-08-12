using Microsoft.AspNetCore.Mvc;
using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Common.Contracts.Pagination;
using SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Commands.AddCategory;
using SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Commands.CreateBudget;
using SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Commands.DeleteBudget;
using SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Commands.RemoveCategory;
using SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Queries.GetBudget;
using SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Queries.GetBudgetCategory;
using SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Queries.GetPaginatedBudgetCategories;
using SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Queries.GetPaginatedBudgets;
using SmartLedger.Modules.Budgets.Contracts.Requests.BudgetCategories;
using SmartLedger.Modules.Budgets.Contracts.Requests.Budgets;
using SmartLedger.Modules.Budgets.Contracts.Responses.BudgetCategories;
using SmartLedger.Modules.Budgets.Contracts.Responses.Budgets;

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
            .RequireAuthorization()
            .WithName("CreateBudget")
            .WithSummary("Creates a new budget.")
            .WithDescription("Creates a new budget for the specified user, with name, start date, and end date.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        endpoints.MapPost("/{budgetId:guid}/categories", AddCategoryAsync)
            .RequireAuthorization()
            .WithName("AddCategoryToBudget")
            .WithSummary("Adds a category to a budget.")
            .WithDescription("Adds a new category with a limit to the specified budget.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

        endpoints.MapDelete("/{budgetId:guid}/categories/{categoryId:guid}", RemoveCategoryAsync)
            .RequireAuthorization()
            .WithName("RemoveCategoryFromBudget")
            .WithSummary("Removes a category from a budget.")
            .WithDescription("Removes an existing category from the specified budget by its unique category ID.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);

        endpoints.MapDelete("/{budgetId:guid}", DeleteBudgetAsync)
            .RequireAuthorization()
            .WithName("DeleteBudget")
            .WithSummary("Deletes a budget by its identifier.")
            .WithDescription("Removes an existing budget identified by its unique budget ID.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);

        endpoints.MapPost("/search", GetPaginatedBudgetsAsync)
            .RequireAuthorization()
            .WithName("GetPaginatedBudgets")
            .WithSummary("Retrieves a paginated list of budgets for the specified user.")
            .WithDescription("Returns a paginated list of budgets associated with the given user ID, with optional filters for start and end dates.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        endpoints.MapGet("/{budgetId:guid}", GetBudgetAsync)
            .RequireAuthorization()
            .WithName("GetBudget")
            .WithSummary("Retrieves a budget by its identifier.")
            .WithDescription("Retrieves the budget details for the specified budget ID.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

        endpoints.MapPost("/{budgetId:guid}/categories/search", GetPaginatedBudgetCategoriesAsync)
            .RequireAuthorization()
            .WithName("GetPaginatedBudgetCategories")
            .WithSummary("Retrieves a paginated list of budget categories for the specified budget.")
            .WithDescription("Returns a paginated list of budget categories associated with the given budget ID, with optional filters for category, limits, spent amounts, status, and dates.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

        endpoints.MapGet("/{budgetId:guid}/categories/{budgetCategoryId:guid}", GetBudgetCategoryAsync)
            .RequireAuthorization()
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
    private static async Task<IResult> CreateBudgetAsync(
        [FromBody] CreateBudgetRequest request,
        [FromServices] ICommandHandler<CreateBudgetCommand, Guid> handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateBudgetCommand(request.Name, request.StartDate, request.EndDate);
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

    /// <summary>
    /// Retrieves budget details by its ID.
    /// </summary>
    private static async Task<IResult> GetBudgetAsync(
        [FromRoute] Guid budgetId,
        [FromServices] IQueryHandler<GetBudgetQuery, BudgetResponse> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetBudgetQuery(budgetId);
        var response = await handler.HandleAsync(query, cancellationToken);

        return Results.Ok(response);
    }

    /// <summary>
    /// Retrieves a paginated list of budgets for the specified user with optional filters.
    /// </summary>
    private static async Task<IResult> GetPaginatedBudgetsAsync(
        [FromBody] GetPaginatedBudgetsRequest request,
        [FromServices] IQueryHandler<GetPaginatedBudgetsQuery, PaginatedList<BudgetListItem>> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetPaginatedBudgetsQuery(
            request.PageNumber,
            request.PageSize,
            request.StartDate,
            request.EndDate);

        var response = await handler.HandleAsync(query, cancellationToken);

        return Results.Ok(response);
    }

    /// <summary>
    /// Retrieves budget category details by its ID.
    /// </summary>
    private static async Task<IResult> GetBudgetCategoryAsync(
        [FromRoute] Guid budgetId,
        [FromRoute] Guid budgetCategoryId,
        [FromServices] IQueryHandler<GetBudgetCategoryQuery, BudgetCategoryResponse> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetBudgetCategoryQuery(budgetCategoryId);
        var response = await handler.HandleAsync(query, cancellationToken);

        return Results.Ok(response);
    }

    /// <summary>
    /// Retrieves a paginated list of budget categories for the specified budget with optional filters.
    /// </summary>
    private static async Task<IResult> GetPaginatedBudgetCategoriesAsync(
        [FromRoute] Guid budgetId,
        [FromBody] GetPaginatedBudgetCategoriesRequest request,
        [FromServices] IQueryHandler<GetPaginatedBudgetCategoriesQuery, PaginatedList<BudgetCategoryListItem>> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetPaginatedBudgetCategoriesQuery(
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

        var response = await handler.HandleAsync(query, cancellationToken);

        return Results.Ok(response);
    }
}