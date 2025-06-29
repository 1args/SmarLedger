using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLedger.Common.Infrastructure.Configurators;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Read.Models;

namespace SmartLedger.Modules.Transactions.Infrastructure.Contexts.Read.Configurations;

/// <summary>
/// Configures the <see cref="TransactionReadModel"/> entity.
/// </summary>
public sealed class TransactionReadModelConfiguration : IEntityTypeConfiguration<TransactionReadModel>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<TransactionReadModel> builder)
    {
        builder.ToTable("TransactionReadModel");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .IsGuid()
            .ValueGeneratedOnAdd();

        builder.Property(t => t.AccountId)
            .IsRequired();

        builder.Property(t => t.UserId)
            .IsRequired();

        builder.Property(t => t.Amount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(t => t.Type)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(t => t.Category)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(t => t.Notes)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(t => t.AccountName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(t => t.CreatedAt)
            .IsDateTime()
            .IsRequired();

        builder.HasIndex(t => t.AccountId)
            .HasDatabaseName("idx_transactionreadmodel_accountid")
            .IsUnique(false); 

        builder.HasIndex(t => t.UserId)
            .HasDatabaseName("idx_transactionreadmodel_userid")
            .IsUnique(false); 

        builder.HasIndex(t => new { t.UserId, t.CreatedAt })
            .HasDatabaseName("idx_transactionreadmodel_userid_createdat")
            .IsUnique(false); 

        builder.HasIndex(t => new { t.AccountId, t.Category, t.CreatedAt })
            .HasDatabaseName("idx_transactionreadmodel_accountid_category_createdat")
            .IsUnique(false);

        builder.HasIndex(t => new { t.UserId, t.Type, t.CreatedAt })
            .HasDatabaseName("idx_transactionreadmodel_userid_type_createdat")
            .IsUnique(false); 
    }
}