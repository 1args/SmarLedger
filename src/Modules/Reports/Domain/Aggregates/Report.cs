using SmartLedger.Common.Domain.Exceptions;
using SmartLedger.Common.Domain.Primitives;
using SmartLedger.Common.Domain.ValueObjects;
using SmartLedger.Modules.Budgets.Domain.Aggregates;
using SmartLedger.Modules.Reports.Domain.Enums;
using SmartLedger.Modules.Reports.Domain.ValueObjects;
using SmartLedger.Modules.Transactions.Domain.Aggregates;
using SmartLedger.Modules.Transactions.Domain.Enums;

namespace SmartLedger.Modules.Reports.Domain.Aggregates;

/// <summary>
/// Represents a financial report for a user.
/// </summary>
public sealed class Report : AggregateRoot<Guid>
{
    /// <summary>Information about user who owns this report.</summary>
    public UserInfo UserInfo { get; set; }

    private readonly List<Account> _accounts = [];

    /// <summary>Collection of accounts associated with this report.</summary>
    public IReadOnlyCollection<Account> Accounts => _accounts.AsReadOnly();

    private readonly List<Budget> _budgets = [];

    /// <summary>Collection of budgets associated with this report.</summary>
    public IReadOnlyCollection<Budget> Budgets => _budgets.AsReadOnly();

    /// <summary>Total income from all transactions in the report period.</summary>
    public Money TotalIncome { get; private set; }

    /// <summary>Total expenses from all transactions in the report period.</summary>
    public Money TotalExpenses { get; private set; }

    /// <summary>Net balance calculated as Total Income minus Total Expenses.</summary>
    public Money NetBalance => Money.Create(TotalIncome.Value - TotalExpenses.Value);

    /// <summary>Average money spent on all transactions in the report period.</summary>
    public Money AverageExpenseAmount { get; private set; }

    /// <summary>Average income from all transactions in the report period.</summary>
    public Money AverageIncomeAmount { get; private set; }

    /// <summary>Total number of transactions in the report period.</summary>
    public int TotalTransactionCount { get; private set; }

    /// <summary>Total number of accounts associated with this report.</summary>
    public int AccountCount => _accounts.Count;

    /// <summary>Total number of budgets associated with this report.</summary>
    public int BudgetCount => _budgets.Count;

    /// <summary>Percentage of income saved, calculated as (Net Balance / Total Income) * 100.</summary>
    public decimal SavingsPercentage { get; private set; }

    private readonly List<AccountDetail> _accountsDetails = [];

    /// <summary>Collection of detailed account information for each account in the report.</summary>
    public IReadOnlyCollection<AccountDetail> AccountDetails => _accountsDetails.AsReadOnly();

    private readonly List<BudgetDetail> _budgetsDetails = [];

    /// <summary>Collection of detailed budget information for each category in the budgets.</summary>
    public IReadOnlyCollection<BudgetDetail> BudgetDetails => _budgetsDetails.AsReadOnly();

    /// <summary>Start date of the report period, inclusive.</summary>
    public DateTime StartPeriod { get; private set; }

    /// <summary>End date of the report period, exclusive.</summary>
    public DateTime EndPeriod { get; private set; }

    /// <summary>Type of the report, indicating whether it is a monthly, quarterly, or yearly report.</summary>
    public ReportType Type { get; private set; }

    /// <summary>Date and time when the report was generated.</summary>
    public DateTime GeneratedAt { get; private set; }

    /// <summary>
    /// Private constructor used by factory method.
    /// </summary>
    private Report(
        UserInfo userInfo,
        IReadOnlyCollection<Account> accounts,
        IReadOnlyCollection<Budget> budgets,
        ReportType type,
        DateTime generatedAt,
        DateTime? startPeriod = null,
        DateTime? endPeriod = null)
    {
        Id = Guid.NewGuid();
        UserInfo = userInfo;
        _accounts = accounts.ToList();
        _budgets = budgets.ToList();
        Type = type;
        GeneratedAt = generatedAt;
    }

    /// <summary>
    /// Factory method to create a new <see cref="Report"/>.
    /// </summary>
    /// <param name="userInfo">List of accounts.</param>
    /// <param name="accounts">List of accounts.</param>
    /// <param name="budgets">List of budgets.</param>
    /// <param name="type">Report type.</param>
    /// <param name="generatedAt">Date and time when report was generated.</param>
    /// <param name="startPeriod">Start date of the report period.</param>
    /// <param name="endPeriod">End date of the report period.</param>
    /// <returns>New instance of <see cref="Report"/>.</returns>
    /// <exception cref="DomainValidationException"></exception>
    public static Report Create(
        UserInfo userInfo,
        IReadOnlyCollection<Account> accounts,
        IReadOnlyCollection<Budget> budgets,
        ReportType type,
        DateTime generatedAt,
        DateTime? startPeriod = null,
        DateTime? endPeriod = null)
    {
        if (accounts is null || !accounts.Any())
        {
            throw new DomainValidationException(nameof(accounts), "Accounts collection cannot be null or empty.");
        }
        if (budgets is null || !budgets.Any())
        {
            throw new DomainValidationException(nameof(budgets), "Budgets collection cannot be null or empty.");
        }
        ArgumentNullException.ThrowIfNull(userInfo);

        var (receivedStartPeriod, receivedEndPeriod) = DetermineReportPeriod(
            type, generatedAt, startPeriod, endPeriod);

        ValidatePeriod(startPeriod, endPeriod);

        var report =  new Report(userInfo, accounts, budgets, type, generatedAt, receivedStartPeriod, receivedEndPeriod);

        report.ComputeFinancialMetrics();
        report.ComputeSavingsPercentage();
        report.GenerateAccountSummaries();
        report.GenerateBudgetSummaries();

        return report;
    }

    /// <summary>
    /// Determines the start and end period for the report based on the type and provided dates.
    /// </summary>
    private static (DateTime StartPeriod, DateTime EndPeriod) DetermineReportPeriod(
        ReportType type,
        DateTime generatedAt,
        DateTime? startPeriod,
        DateTime? endPeriod)
    {
        return type switch
        {
            ReportType.Daily => (
                generatedAt.Date, generatedAt.Date.AddDays(1)),
            ReportType.Weekly => (
                generatedAt.Date.AddDays(-(int)generatedAt.DayOfWeek), generatedAt.Date.AddDays(7 - (int)generatedAt.DayOfWeek)),
            ReportType.Monthly => (
                new DateTime(generatedAt.Year, generatedAt.Month, 1), 
                new DateTime(generatedAt.Year, generatedAt.Month, 1).AddMonths(1)),
            ReportType.Yearly => (
                new DateTime(generatedAt.Year, 1, 1),
                new DateTime(generatedAt.Year, 1, 1).AddYears(1)),
            ReportType.Custom => (
                startPeriod ?? throw new ArgumentNullException(nameof(startPeriod),
                    "Start period cannot be null for custom reports."),
                endPeriod ?? throw new ArgumentNullException(nameof(endPeriod),
                    "End period cannot be null for custom reports.")),
            _ => throw new DomainValidationException(nameof(type), "Invalid report type specified.")
        };
    }

    /// <summary>
    /// Validates the start and end period for the report.
    /// </summary>
    private static void ValidatePeriod(DateTime? startPeriod, DateTime? endPeriod)
    {
        if (startPeriod >= endPeriod)
        {
            throw new DomainValidationException(nameof(startPeriod), "Start period must be before end period.");
        }
    }

    /// <summary>
    /// Calculates financial metrics.
    /// </summary>
    private void ComputeFinancialMetrics()
    {
        var allTransactions = _accounts
            .SelectMany(account => account.Transactions
                .Where(t => t.CreatedAt >= StartPeriod && t.CreatedAt < EndPeriod))
            .ToList();

        TotalIncome = Money.Create(allTransactions
            .Where(t => t.Type == TransactionType.Income)
            .Sum(t => t.Amount.Value));

        TotalExpenses = Money.Create(allTransactions
            .Where(t => t.Type == TransactionType.Expense)
            .Sum(t => t.Amount.Value));

        TotalTransactionCount = allTransactions.Count;

        AverageIncomeAmount = TotalTransactionCount > 0
            ? Money.Create(TotalIncome.Value / TotalTransactionCount)
            : Money.Zero;

        AverageExpenseAmount = TotalTransactionCount > 0
            ? Money.Create(TotalExpenses.Value / TotalTransactionCount)
            : Money.Zero;
    }

    /// <summary>
    /// Calculates the percentage of income saved based on the net balance and total income.
    /// </summary>
    private void ComputeSavingsPercentage()
    {
        SavingsPercentage = TotalIncome.Value > 0 ? (NetBalance.Value / TotalIncome.Value) * 100 : 0;
    }

    /// <summary>
    /// Calculates detailed account information for each account in the report.
    /// </summary>
    private void GenerateAccountSummaries()
    {
        foreach (var account in _accounts)
        {
            var transactions = account.Transactions
                .Where(t => t.CreatedAt >= StartPeriod && t.CreatedAt < EndPeriod)
                .ToList();

            var detail = AccountDetail.Create(account, transactions);
            _accountsDetails.Add(detail);
        }
    }

    /// <summary>
    /// Calculates budget details for each category in the budgets.
    /// </summary>
    private void GenerateBudgetSummaries()
    {
        var allTransactions = _accounts
            .SelectMany(account => account.Transactions
                .Where(t => t.CreatedAt >= StartPeriod && t.CreatedAt < EndPeriod))
            .ToList();

        var expensesTransactions = allTransactions
            .Where(t => t.Type == TransactionType.Expense)
            .GroupBy(t => t.Category)
            .ToDictionary(g => g.Key, g => Money.Create(g.Sum(t => t.Amount.Value)));

        foreach (var budget in _budgets)
        {
            foreach (var category in budget.Categories)
            {
                var amount = expensesTransactions.TryGetValue(category.Category, out var totalExpense)
                    ? totalExpense
                    : Money.Zero;

                var limit = Money.Create(category.Limit.Value);
                var variance = Money.Create(limit.Value - amount.Value);
                var variancePercentage = limit.Value > 0
                    ? (variance.Value / limit.Value) * 100
                    : 0;
                var status = amount.Value > limit.Value
                    ? "Exceeded"
                    : "Active";

                var detail = BudgetDetail.Create(
                    category.Category.ToString(),
                    amount,
                    limit,
                    variance,
                    Money.Create(variancePercentage),
                    status);

                _budgetsDetails.Add(detail);
            }
        }
    }
}