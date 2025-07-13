using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLedger.Common.Infrastructure.Configurators;
using SmartLedger.Modules.Budgets.Infrastructure.Contexts.Read.Models;

namespace SmartLedger.Modules.Budgets.Infrastructure.Contexts.Read.Configurations;

/// <summary>
/// Configures the <see cref="BudgetItemReadModel"/> entity.
/// </summary>
public sealed class BudgetItemReadModelConfiguration : IEntityTypeConfiguration<BudgetItemReadModel>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<BudgetItemReadModel> builder)
    {
        builder.ToTable("BudgetItems");

        builder.HasKey(bi => bi.Id);

        builder.Property(bi => bi.Id)
            .IsGuid()
            .ValueGeneratedNever();

        builder.Property(bi => bi.BudgetId)
            .IsRequired();

        builder.Property(bi => bi.UserId)
            .IsRequired();

        builder.Property(bi => bi.Category)
            .HasMaxLength(PropertyLengthConstants.Length20)
            .IsRequired();

        builder.Property(bi => bi.Limit)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(bi => bi.SpentAmount)
            .HasColumnName("decimal(18,2)")
            .IsRequired();

        builder.Property(bi => bi.Status)
            .HasMaxLength(PropertyLengthConstants.Length20)
            .IsRequired();

        builder.Property(bi => bi.BudgetName)
            .HasMaxLength(PropertyLengthConstants.Length100);

        builder.Property(bi => bi.CreatedAt)
            .IsDateTime()
            .IsRequired();

        builder.Property(bi => bi.LastUpdatedAt)
            .IsDateTime()
            .IsRequired();

        builder.HasIndex(bi => bi.BudgetId)
            .HasDatabaseName("idx_budgetitems_budgetid")
            .IsUnique(false);

        builder.HasIndex(bi => bi.UserId)
            .HasDatabaseName("idx_budgetitems_userid")
            .IsUnique(false);

        builder.HasIndex(bi => new { bi.BudgetId, bi.Category })
            .HasDatabaseName("idc_budgetitems_budgetid_userid")
            .IsUnique(false);
    }
}