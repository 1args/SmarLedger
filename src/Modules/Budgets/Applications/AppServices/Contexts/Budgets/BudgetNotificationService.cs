using SmartLedger.Common.Infrastructures.DataAccess.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;
using SmartLedger.Modules.Budgets.Domain.Aggregates;
using SmartLedger.Modules.Budgets.Domain.Enums;
using SmartLedger.Modules.Notifications.Contracts.Common.Notifications;
using SmartLedger.Modules.Notifications.Contracts.Enums;
using SmartLedger.Modules.Notifications.Contracts.Events;

namespace SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets;

/// <summary>
/// Class for sending budget-related notifications.
/// </summary>
public class BudgetNotificationService(IEventBus eventBus) : IBudgetNotificationService
{
    /// <inheritdoc/>
    public async Task NotifyLimitExceededAsync(Guid userId, List<Budget> budgets, CancellationToken cancellationToken)
    {
        var exceededCategories = budgets
            .SelectMany(b => b.Categories)
            .Where(c => c.Status == BudgetCategoryStatus.Exceeded);

        foreach (var category in exceededCategories)
        {
            var notificationEvent = new NotificationSentEvent(
                NotificationType.LimitExceeded,
                userId,
                new Dictionary<string, string>
                {
                    { NotificationKeys.LimitAmount, category.Limit.Value.ToString() },
                    { NotificationKeys.CurrentAmount, category.SpentAmount.Value.ToString() },
                    { NotificationKeys.ExceededAmount, (category.SpentAmount.Value - category.Limit.Value).ToString() }
                });

            await eventBus.PublishAsync(notificationEvent, cancellationToken);
        }
    }
}