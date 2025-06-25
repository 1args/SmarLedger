using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLedger.Common.Infrastructure.Configurators;
using SmartLedger.Modules.Transactions.Domain.Aggregates;
using SmartLedger.Modules.Transactions.Domain.ValueObjects;

namespace SmartLedger.Modules.Transactions.Infrastructure.Contexts.Write.Configurations;

/// <summary>
/// Configures the <see cref="Account"/> entity.
/// </summary>
public sealed class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .IsGuid()
            .ValueGeneratedOnAdd();

        builder.Property(a => a.UserId)
            .IsRequired();

        builder.Property(a => a.CreatedAt)
            .IsDateTime()
            .IsRequired();

        builder.OwnsOne(a => a.Name, name =>
        {
            name.Property(n => n.Value)
                .HasColumnName("Name")
                .HasMaxLength(AccountName.MaxLength)
                .IsRequired();
        });

        builder.OwnsOne(a => a.Balance, balance =>
        {
            balance.Property(b => b.Value)
                .HasColumnName("Balance")
                .HasColumnType("decimal(18,2)")
                .IsRequired();
        });

        builder.HasMany(a => a.Transactions)
            .WithOne()
            .HasForeignKey(t => t.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}