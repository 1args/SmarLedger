using Microsoft.EntityFrameworkCore;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Read.Models;

namespace SmartLedger.Modules.Transactions.Infrastructure.Contexts.Read;

/// <summary>
/// Represents the read-side database context for the Transactions module.
/// </summary>
/// <param name="options">DbContext options.</param>
public sealed class TransactionsReadDbContext(
    DbContextOptions<TransactionsReadDbContext> options) : DbContext(options)
{
    /// <summary>Accounts.</summary>
    public DbSet<AccountReadModel> Accounts { get; set; }

    /// <summary>Transactions.</summary>
    public DbSet<TransactionReadModel> Transactions { get; set; }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        CustomModelBuilder.OnModelCreating(modelBuilder);
    }
}