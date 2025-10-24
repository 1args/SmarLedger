using SmartLedger.Modules.Budgets.Domain.Aggregates;

namespace SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;

/// <summary>
/// Interface for sending budget-related notifications.
/// </summary>
public interface IBudgetNotificationService
{
    /// <summary>
    /// Sends notifications to the specified user for all budget categories that have exceeded their spending limit.
    /// </summary>
    /// <param name="userId">User ID.</param>
    /// <param name="budgets">List of budgets associated with the user.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task NotifyLimitExceededAsync(Guid userId, List<Budget> budgets, CancellationToken cancellationToken);
}