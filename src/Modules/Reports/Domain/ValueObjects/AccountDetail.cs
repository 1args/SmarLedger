using SmartLedger.Common.Domain.Primitives;
using SmartLedger.Common.Domain.ValueObjects;
using SmartLedger.Modules.BankAccounts.Domain.Aggregates;
using SmartLedger.Modules.BankAccounts.Domain.Entities;
using SmartLedger.Modules.BankAccounts.Domain.Enums;

namespace SmartLedger.Modules.Reports.Domain.ValueObjects;

/// <summary>
/// Represents the account details as a value object.
/// </summary>
public sealed class AccountDetail : ValueObject
{
    /// <summary>Name of the account.</summary>
    public string Name { get; }

    /// <summary>Current balance of the account.</summary>
    public Money Balance { get; }

    /// <summary>Total number of transactions in the account.</summary>
    public int TransactionCount { get; }

    /// <summary>Average sum of the transactions in the account.</summary>
    public Money AverageSum { get; }

    /// <summary>Total amount spent in the account as expenses.</summary>
    public Money TotalExpense { get; }

    /// <summary>Total amount received in the account as income.</summary>
    public Money TotalIncome { get; }

    /// <summary>Percentage of income that has been saved in the account.</summary>
    public Money PercentageSaved { get; }

    /// <summary>
    /// Private constructor used by factory method.
    /// </summary>
    private AccountDetail(
        string name,
        Money balance,
        int transactionCount,
        Money averageSum,
        Money totalExpense,
        Money totalIncome,
        Money percentageSaved)
    {
        Name = name;
        Balance = balance;
        TransactionCount = transactionCount;
        AverageSum = averageSum;
        TotalExpense = totalExpense;
        TotalIncome = totalIncome;
        PercentageSaved = percentageSaved;
    }

    /// <summary>
    /// Factory method to create a new <see cref="AccountDetail"/>.
    /// </summary>
    /// <param name="account"></param>
    /// <param name="transactions"></param>
    /// <returns>New instance of <see cref="AccountDetail"/>.</returns>
    public static AccountDetail Create(Account account, IReadOnlyCollection<Transaction> transactions)
    {
        ArgumentNullException.ThrowIfNull(account);

        var totalIncome = Money.Create(transactions
            .Where(t => t.Type == TransactionType.Income)
            .Sum(t => t.Amount.Value));

        var totalExpense = Money.Create(transactions
            .Where(t => t.Type == TransactionType.Expense)
            .Sum(t => t.Amount.Value));

        var averageSum = Money.Create(transactions
            .Select(t => t.Amount.Value)
            .DefaultIfEmpty(0)
            .Average());

        var percentageSaved = totalIncome.Value > 0
            ? Money.Create((totalIncome.Value - totalExpense.Value) / totalIncome.Value * 100)
            : Money.Zero;

        return new(
            account.Name.Value,
            account.Balance,
            transactions.Count,
            averageSum,
            totalExpense,
            totalIncome,
            percentageSaved);
    }

    /// <inheritdoc />
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return Balance;
        yield return TransactionCount;
        yield return AverageSum;
        yield return TotalExpense;
        yield return TotalIncome;
        yield return PercentageSaved;
    }
}