using Microsoft.EntityFrameworkCore;
using SmartLedger.Modules.Transactions.Domain.Aggregates;
using SmartLedger.Modules.Transactions.Domain.Entities;

namespace SmartLedger.Modules.Transactions.Infrastructure.Contexts.Write;

/// <summary>
/// Represents the write-side database context for the Transactions module.
/// </summary>
/// <param name="options">DbContext options.</param>
public sealed class TransactionsWriteDbContext(
    DbContextOptions<TransactionsWriteDbContext> options) : DbContext(options)
{
    /// <summary>Accounts.</summary>
    public DbSet<Account> Accounts { get; set; }

    /// <summary>Transactions.</summary>
    public DbSet<Transaction> Transactions { get; set; }

    /// <summary>TransactionCategories.</summary>
    public DbSet<TransactionCategory> TransactionCategories { get; set; }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        CustomModelBuilder.OnModelCreating(modelBuilder);
    }
}