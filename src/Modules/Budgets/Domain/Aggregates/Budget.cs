using SmartLedger.Common.Domain.Exceptions;
using SmartLedger.Common.Domain.Primitives;
using SmartLedger.Modules.Budgets.Domain.Entities;
using SmartLedger.Modules.Budgets.Domain.Exceptions;
using SmartLedger.Modules.Budgets.Domain.ValueObjects;

namespace SmartLedger.Modules.Budgets.Domain.Aggregates;

/// <summary>
/// Represents a user's budget.
/// </summary>
public sealed class Budget : AggregateRoot<Guid>
{
    /// <summary>User ID who owns this budget.</summary>
    public Guid UserId { get; private set; }

    /// <summary>Budget name.</summary>
    public BudgetName Name { get; private set; }

    /// <summary>Time period covered by the budget.</summary>
    public BudgetPeriod Period { get; private set; }

    private readonly List<BudgetCategory> _categories = [];

    /// <summary>List of items (categories) tracked in this budget.</summary>
    public IReadOnlyCollection<BudgetCategory> Categories => _categories.AsReadOnly();

    /// <summary>Date and time when budget was created.</summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Constructor for EF Core.
    /// </summary>
    private Budget() { }

    /// <summary>
    /// Private constructor used by factory method.
    /// </summary>
    private Budget(
        Guid userId,
        BudgetName name,
        BudgetPeriod period,
        DateTime createAt)
    {
        UserId = userId;
        Name = name;
        Period = period;
        CreatedAt = createAt;
    }

    /// <summary>
    /// Factory method to create a new <see cref="Budget"/>.
    /// </summary>
    /// <param name="name">Budget name.</param>
    /// <param name="period">Time period covered by the budget.</param>
    /// <param name="userId">User ID.</param>
    /// <param name="createdAt">Date and time of creation.</param>
    /// <returns>New instance of <see cref="Budget"/>.</returns>
    /// <exception cref="DomainValidationException">Thrown when userId is empty or arguments are null.</exception>
    public static Budget Create(
        Guid userId,
        BudgetName name,
        BudgetPeriod period,
        DateTime createdAt)
    {
        if (userId == Guid.Empty)
        {
            throw new DomainValidationException(nameof(userId), "User ID cannot be empty.");
        }

        ArgumentNullException.ThrowIfNull(name, nameof(name));
        ArgumentNullException.ThrowIfNull(period, nameof(period));

        return new(userId, name, period, createdAt);
    }

    /// <summary>
    /// Adds a new category to the budget.
    /// </summary>
    /// <param name="category">Category to add.</param>
    /// <exception cref="InvalidBudgetOperationException">Thrown when an item with the same category already exists.</exception>
    public void AddCategory(BudgetCategory category)
    {
        ArgumentNullException.ThrowIfNull(category);

        if (_categories.Any(i => i.Category == category.Category))
        {
            throw new InvalidBudgetOperationException(
                nameof(category),
                $"Category '{category.Category.ToString()}' already exists in budget.");
        }

        _categories.Add(category);
    }

    /// <summary>
    /// Removes a category from the budget.
    /// </summary>
    /// <param name="category">Category to remove.</param>
    /// <exception cref="InvalidBudgetOperationException">Thrown when the item does not exist in the budget.</exception>
    public void RemoveCategory(BudgetCategory category)
    {
        ArgumentNullException.ThrowIfNull(category);

        if (!_categories.Contains(category))
        {
            throw new InvalidBudgetOperationException(
                nameof(category),
                $"Item with category '{category.Category.ToString()}' does not exist in budget.");
        }

        _categories.Remove(category);
    }
}