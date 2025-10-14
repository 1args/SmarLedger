using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLedger.Common.Domain.ValueObjects;
using SmartLedger.Common.Infrastructures.DataAccess.Configurators;
using SmartLedger.Modules.BankAccounts.Domain.Aggregates;
using SmartLedger.Modules.BankAccounts.Domain.Entities;
using SmartLedger.Modules.BankAccounts.Domain.ValueObjects;

namespace SmartLedger.Modules.BankAccounts.Infrastructures.DataAccess.Contexts.Write.Configurations;

/// <summary>
/// Configures the <see cref="Transaction"/> entity.
/// </summary>
public sealed class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasConversion(id => id.Value, value => TransactionId.Create(value))
            .ValueGeneratedNever();

        builder.Property(t => t.AccountId)
            .HasConversion(id => id.Value, value => AccountId.Create(value))
            .IsRequired();

        builder.Property(t => t.Amount)
            .HasColumnName("Amount")
            .HasConversion(amount => amount.Value, value => Money.Create(value))
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(t => t.Type)
            .HasColumnName("Type")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(t => t.Category)
            .HasColumnName("Category")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(t => t.Notes)
            .HasColumnName("Notes")
            .HasConversion(notes => notes.Value, value => TransactionDescription.Create(value))
            .HasMaxLength(TransactionDescription.MaxLength)
            .IsRequired();

        builder.Property(t => t.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasConversion(date => date.Value, value => CreationDate.Create(value))
            .IsRequired();

        builder.HasOne<Account>()
            .WithMany(a => a.Transactions)
            .HasForeignKey(t => t.AccountId)
            .IsRequired();
    }
}