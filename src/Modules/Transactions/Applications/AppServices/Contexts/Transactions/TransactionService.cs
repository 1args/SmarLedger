using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Contracts.Exceptions;
using SmartLedger.Common.Infrastructure.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Models;
using SmartLedger.Modules.Transactions.Domain.Aggregates;
using SmartLedger.Modules.Transactions.Domain.ValueObjects;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions;

/// <inheritdoc />
public sealed class TransactionService(
    IRepository<Transaction> transactionRepository,
    ILogger<TransactionService> logger) : ITransactionService
{
    /// <inheritdoc />
    public async Task CreateAsync(CreateTransactionModel request, CancellationToken cancellationToken)
    {
        var amount = Money.Create(request.Amount);
        var notes = TransactionDescription.Create(request.Notes);

        var transaction = Transaction.Create(
            request.AccountId,
            amount,
            request.Type,
            request.CategoryId,
            request.CreatedAt,
            notes);

        await transactionRepository.AddAsync(transaction, cancellationToken);
    }

    /// <inheritdoc />
    public async Task UpdateAmountAsync(UpdateAmountModel request, CancellationToken cancellationToken)
    {
        var transaction = await GetTransactionAsync(request.TransactionId, cancellationToken);
        var amount = Money.Create(request.NewAmount);

        transaction.UpdateAmount(amount);
        await transactionRepository.UpdateAsync(transaction, cancellationToken);
    }

    /// <inheritdoc />
    public async Task CategorizeAsync(CategorizeTransactionModel request, CancellationToken cancellationToken)
    {
        var transaction = await GetTransactionAsync(request.TransactionId, cancellationToken);

        transaction.Categorize(request.NewCategoryId);
        await transactionRepository.UpdateAsync(transaction, cancellationToken);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(IdOnlyModel request, CancellationToken cancellationToken)
    {
        var transaction = await GetTransactionAsync(request.TransactionId, cancellationToken);

        await transactionRepository.DeleteAsync([transaction], cancellationToken);
    }

    /// <summary>
    /// Retrieves transaction by its ID or throws if not found.
    /// </summary>
    private async Task<Transaction> GetTransactionAsync(Guid transactionId, CancellationToken cancellationToken)
    {
        var transaction = await transactionRepository
            .Where(t => t.Id == transactionId)
            .SingleOrDefaultAsync(cancellationToken);

        if (transaction is null)
        {
            throw new NotFoundException($"Transaction with ID '{transactionId}' was not found.");
        }

        return transaction;
    }
}