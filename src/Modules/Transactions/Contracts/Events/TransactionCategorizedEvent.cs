using SmartLedger.Common.Contracts.Abstractions;
using SmartLedger.Modules.Transactions.Domain.Enums;

namespace SmartLedger.Modules.Transactions.Contracts.Events;

/// <summary>
/// Event triggered when a transaction is assigned a new category.
/// </summary>
/// <param name="TransactionId">Transaction ID.</param>
/// <param name="NewCategory">New category.</param>
/// <param name="UpdatedAt">Date and time when the category was updated.</param>
public sealed record TransactionCategorizedEvent(
    Guid TransactionId,
    TransactionCategory NewCategory,
    DateTime UpdatedAt) : Event;