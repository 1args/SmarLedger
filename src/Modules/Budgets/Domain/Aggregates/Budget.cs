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

    private readonly List<BudgetItem> _items = [];

    /// <summary>List of items (categories) tracked in this budget.</summary>
    public IReadOnlyCollection<BudgetItem> Items => _items.AsReadOnly();

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
        BudgetName name,
        BudgetPeriod period,
        Guid userId,
        DateTime createAt)
    {
        Name = name;
        Period = period;
        UserId = userId;
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
        BudgetName name,
        BudgetPeriod period,
        Guid userId,
        DateTime createdAt)
    {
        if (userId == Guid.Empty)
        {
            throw new DomainValidationException(nameof(userId), "User ID cannot be empty.");
        }

        ArgumentNullException.ThrowIfNull(name, nameof(name));
        ArgumentNullException.ThrowIfNull(period, nameof(period));

        return new(name, period, userId, createdAt);
    }

    /// <summary>
    /// Adds a new item to the budget.
    /// </summary>
    /// <param name="item">Item to add.</param>
    /// <exception cref="InvalidBudgetOperationException">Thrown when an item with the same category already exists.</exception>
    public void AddItem(BudgetItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (_items.Any(i => i.Category == item.Category))
        {
            throw new InvalidBudgetOperationException(
                nameof(item),
                $"Category '{item.Category.ToString()}' already exists in budget.");
        }

        _items.Add(item);
    }

    /// <summary>
    /// Removes an item from the budget.
    /// </summary>
    /// <param name="item">Item to remove.</param>
    /// <exception cref="InvalidBudgetOperationException">Thrown when the item does not exist in the budget.</exception>
    public void RemoveItem(BudgetItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (!_items.Contains(item))
        {
            throw new InvalidBudgetOperationException(
                nameof(item),
                $"Item with category '{item.Category.ToString()}' does not exist in budget.");
        }

        _items.Remove(item);
    }
}