using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLedger.Common.Infrastructure.Configurators;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Read.Models;

namespace SmartLedger.Modules.Transactions.Infrastructure.Contexts.Read.Configurations;

/// <summary>
/// Configures the <see cref="AccountReadModelConfiguration"/> entity.
/// </summary>
public sealed class AccountReadModelConfiguration : IEntityTypeConfiguration<AccountReadModel>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<AccountReadModel> builder)
    {
        builder.ToTable("Accounts");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .IsGuid()
            .ValueGeneratedNever();

        builder.Property(a => a.UserId)
            .IsRequired();

        builder.Property(a => a.Name)
            .HasMaxLength(PropertyLengthConstants.Length100)
            .IsRequired();

        builder.Property(a => a.Balance)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(a => a.CreatedAt)
            .IsDateTime()
            .IsRequired();

        builder.Property(a => a.LastUpdatedAt)
            .IsDateTime()
            .IsRequired();

        builder.HasIndex(a => a.UserId)
            .HasDatabaseName("idx_accounts_userid")
            .IsUnique(false);

        builder.HasIndex(a => new { a.UserId, a.CreatedAt })
            .HasDatabaseName("idx_accounts_userid_createdat")
            .IsUnique(false); 
    }
}