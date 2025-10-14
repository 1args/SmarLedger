using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLedger.Common.Infrastructures.DataAccess.Configurators;
using SmartLedger.Modules.Budgets.Infrastructures.DataAccess.Contexts.Read.Models;

namespace SmartLedger.Modules.Budgets.Infrastructures.DataAccess.Contexts.Read.Configurations;

/// <summary>
/// Configures the <see cref="BudgetCategoryReadModel"/> entity.
/// </summary>
public sealed class BudgetCategoryReadModelConfiguration : IEntityTypeConfiguration<BudgetCategoryReadModel>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<BudgetCategoryReadModel> builder)
    {
        builder.ToTable("BudgetCategories");

        builder.HasKey(bc => bc.Id);

        builder.Property(bc => bc.Id)
            .IsGuid()
            .ValueGeneratedNever();

        builder.Property(bc => bc.BudgetId)
            .IsRequired();

        builder.Property(bc => bc.UserId)
            .IsRequired();

        builder.Property(bc => bc.Category)
            .HasColumnName("Category")
            .HasMaxLength(PropertyLengthConstants.Length20)
            .IsRequired();

        builder.Property(bc => bc.Limit)
            .HasColumnName("Limit")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(bc => bc.SpentAmount)
            .HasColumnName("SpentAmount")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(bc => bc.Status)
            .HasColumnName("Status")
            .HasMaxLength(PropertyLengthConstants.Length20)
            .IsRequired();

        builder.Property(bc => bc.BudgetName)
            .HasColumnName("BudgetName")
            .HasMaxLength(PropertyLengthConstants.Length100);

        builder.Property(b => b.StartDate)
            .HasColumnName("StartDate")
            .IsDateTime()
            .IsRequired();

        builder.Property(b => b.EndDate)
            .HasColumnName("EndDate")
            .IsDateTime()
            .IsRequired();

        builder.Property(bc => bc.CreatedAt)
            .HasColumnName("CreatedAt")
            .IsDateTime()
            .IsRequired();

        builder.Property(bc => bc.LastUpdatedAt)
            .HasColumnName("LastUpdatedAt")
            .IsDateTime()
            .IsRequired();

        builder.HasIndex(bc => bc.BudgetId)
            .HasDatabaseName("idx_budgetitems_budgetid")
            .IsUnique(false);

        builder.HasIndex(bc => bc.UserId)
            .HasDatabaseName("idx_budgetitems_userid")
            .IsUnique(false);

        builder.HasIndex(bc => new { bc.BudgetId, bc.Category })
            .HasDatabaseName("idc_budgetitems_budgetid_userid")
            .IsUnique(false);
    }
}