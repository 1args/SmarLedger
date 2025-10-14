using SmartLedger.Common.Domain.Exceptions;
using SmartLedger.Common.Domain.Primitives;

namespace SmartLedger.Modules.Budgets.Domain.ValueObjects;

/// <summary>
/// Represents a period of time during which a budget is valid as a value object.
/// </summary>
public sealed class BudgetPeriod : ValueObject
{
    /// <summary>Start date of the budget period.</summary>
    public DateTime StartDate { get; }

    /// <summary>End date of the budget period.</summary>
    public DateTime EndDate { get; }

    /// <summary>
    /// Constructor for EF Core.
    /// </summary>
    private BudgetPeriod() { }

    /// <summary>
    /// Private constructor used by factory method.
    /// </summary>
    private BudgetPeriod(DateTime startDate, DateTime endDate) =>
        (StartDate, EndDate) = (startDate, endDate);

    /// <summary>
    /// Creates a new <see cref="BudgetPeriod"/> instance.
    /// </summary>
    /// <param name="startDate">Start date of the period.</param>
    /// <param name="endDate">End date of the period.</param>
    /// <returns>New <see cref="BudgetPeriod"/> instance.</returns>
    /// <exception cref="DomainValidationException">Thrown if dates are invalid or start date is in the past.</exception>
    public static BudgetPeriod Create(DateTime startDate, DateTime endDate)
    {
        if (startDate >= endDate)
        {
            throw new DomainValidationException(nameof(startDate), "Start date must be earlier than end date.");
        }
        if (startDate < DateTime.UtcNow.Date)
        {
            throw new DomainValidationException(nameof(startDate), "Start date cannot be in the past.");
        }
        return new(startDate, endDate);
    }

    /// <summary>
    /// Checks if a given date is within the period.
    /// </summary>
    /// <param name="date">Date to check.</param>
    /// <returns><c>true</c> if the date is within the range; otherwise, <c>false</c>.</returns>
    public bool IsActive(DateTime date) => 
        date >= StartDate && date <= EndDate;

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return StartDate;
        yield return EndDate;
    }
}