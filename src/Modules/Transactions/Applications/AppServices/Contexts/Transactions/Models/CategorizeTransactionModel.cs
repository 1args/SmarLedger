using SmartLedger.Modules.Transactions.Domain.Enums;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Models;

/// <summary>
/// Model for changing the category of an existing transaction.
/// </summary>
/// <param name="TransactionId">Transaction ID.</param>
/// <param name="NewCategory">New category.</param>
/// <param name="UpdatedAt">Date and time when the category was updated.</param>
public sealed record CategorizeTransactionModel(
    Guid TransactionId, 
    TransactionCategory NewCategory,
    DateTime UpdatedAt);