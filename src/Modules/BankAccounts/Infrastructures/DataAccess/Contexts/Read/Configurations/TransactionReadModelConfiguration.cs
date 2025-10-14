using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLedger.Common.Infrastructures.DataAccess.Configurators;
using SmartLedger.Modules.BankAccounts.Infrastructures.DataAccess.Contexts.Read.Models;

namespace SmartLedger.Modules.BankAccounts.Infrastructures.DataAccess.Contexts.Read.Configurations;

/// <summary>
/// Configures the <see cref="TransactionReadModel"/> entity.
/// </summary>
public sealed class TransactionReadModelConfiguration : IEntityTypeConfiguration<TransactionReadModel>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<TransactionReadModel> builder)
    {
        builder.ToTable("Transactions");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .IsGuid()
            .ValueGeneratedNever();

        builder.Property(t => t.AccountId)
            .IsRequired();

        builder.Property(t => t.UserId)
            .IsRequired();

        builder.Property(t => t.Amount)
            .HasColumnName("Amount")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(t => t.Type)
            .HasColumnName("Type")
            .HasConversion<string>()
            .HasMaxLength(PropertyLengthConstants.Length20)
            .IsRequired();

        builder.Property(t => t.Category)
            .HasColumnName("Category")
            .HasConversion<string>()
            .HasMaxLength(PropertyLengthConstants.Length20)
            .IsRequired();

        builder.Property(t => t.Notes)
            .HasColumnName("Notes")
            .HasMaxLength(PropertyLengthConstants.Length200)
            .IsRequired();

        builder.Property(t => t.AccountName)
            .HasColumnName("AccountName")
            .HasMaxLength(PropertyLengthConstants.Length100)
            .IsRequired();

        builder.Property(t => t.CreatedAt)
            .HasColumnName("CreatedAt")
            .IsDateTime()
            .IsRequired();

        builder.HasIndex(t => t.AccountId)
            .HasDatabaseName("idx_transactions_accountid")
            .IsUnique(false); 

        builder.HasIndex(t => t.UserId)
            .HasDatabaseName("idx_transactions_userid")
            .IsUnique(false); 

        builder.HasIndex(t => new { t.UserId, t.CreatedAt })
            .HasDatabaseName("idx_transactions_userid_createdat")
            .IsUnique(false); 

        builder.HasIndex(t => new { t.AccountId, t.Category, t.CreatedAt })
            .HasDatabaseName("idx_transactions_accountid_category_createdat")
            .IsUnique(false);

        builder.HasIndex(t => new { t.UserId, t.Type, t.CreatedAt })
            .HasDatabaseName("idx_transactions_userid_type_createdat")
            .IsUnique(false); 
    }
}