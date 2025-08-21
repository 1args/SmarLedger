using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLedger.Common.Infrastructures.DataAccess.Configurators;
using SmartLedger.Modules.Transactions.Domain.Aggregates;
using SmartLedger.Modules.Transactions.Domain.Entities;

namespace SmartLedger.Modules.Transactions.Infrastructures.DataAccess.Contexts.Write.Configurations;

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
            .IsGuid()
            .ValueGeneratedOnAdd();

        builder.Property(t => t.AccountId)
            .IsRequired();

        builder.Property(t => t.Type)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(t => t.Category)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(t => t.CreatedAt)
            .IsDateTime()
            .IsRequired();

        builder.OwnsOne(t => t.Amount, amount =>
        {
            amount.Property(m => m.Value)
                .HasColumnName("Amount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();
        });

        builder.OwnsOne(t => t.Notes, notes =>
        {
            notes.Property(n => n.Value)
                .HasColumnName("Notes")
                .HasMaxLength(500)
                .IsRequired();
        });

        builder.HasOne<Account>()
            .WithMany(a => a.Transactions)
            .HasForeignKey(t => t.AccountId)
            .IsRequired();
    }
}