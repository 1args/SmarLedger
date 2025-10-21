using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLedger.Common.Domain.ValueObjects;
using SmartLedger.Modules.BankAccounts.Domain.Aggregates;
using SmartLedger.Modules.BankAccounts.Domain.ValueObjects;

namespace SmartLedger.Modules.BankAccounts.Infrastructures.DataAccess.Contexts.Write.Configurations;

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
            .HasConversion(id => id.Value, value => AccountId.Create(value))
            .ValueGeneratedNever();

        builder.Property(a => a.Name)
            .HasColumnName("Name")
            .HasConversion(name => name.Value, value => AccountName.Create(value))
            .HasMaxLength(AccountName.MaxLength)
            .IsRequired();

        builder.Property(a => a.Balance)
            .HasColumnName("Balance")
            .HasConversion(balance => balance.Value, value => Money.Create(value))
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(a => a.UserId)
            .HasConversion(id => id.Value, value => UserId.Create(value))
            .IsRequired();

        builder.Property(a => a.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasConversion(date => date.Value, value => CreationDate.Create(value))
            .IsRequired();

        builder.HasMany(a => a.Transactions)
            .WithOne()
            .HasForeignKey(t => t.AccountId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Navigation(a => a.Transactions)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}