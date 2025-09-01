using Microsoft.EntityFrameworkCore;
using SmartLedger.Modules.BankAccounts.Domain.Aggregates;
using SmartLedger.Modules.BankAccounts.Domain.Entities;

namespace SmartLedger.Modules.BankAccounts.Infrastructures.DataAccess.Contexts.Write;

/// <summary>
/// Represents the write-side database context for the Transactions module.
/// </summary>
/// <param name="options">DbContext options.</param>
public sealed class BackAccountsWriteDbContext(
    DbContextOptions<BackAccountsWriteDbContext> options) : DbContext(options)
{
    /// <summary>Accounts.</summary>
    public DbSet<Account> Accounts { get; set; }

    /// <summary>Transactions.</summary>
    public DbSet<Transaction> Transactions { get; set; }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        CustomModelBuilder.OnModelCreating(modelBuilder);
    }
}