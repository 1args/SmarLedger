using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLedger.Common.Domain.ValueObjects;
using SmartLedger.Common.Infrastructures.DataAccess.Configurators;
using SmartLedger.Modules.Budgets.Domain.Aggregates;
using SmartLedger.Modules.Budgets.Domain.Entities;
using SmartLedger.Modules.Budgets.Domain.ValueObjects;

namespace SmartLedger.Modules.Budgets.Infrastructures.DataAccess.Contexts.Write.Configurations;

/// <summary>
/// Configures the <see cref="BudgetCategory"/> entity.
/// </summary>
public sealed class BudgetCategoryConfiguration : IEntityTypeConfiguration<BudgetCategory>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<BudgetCategory> builder)
    {
        builder.ToTable("BudgetCategories");

        builder.HasKey(bc => bc.Id);

        builder.Property(bc => bc.Id)
            .HasConversion(id => id.Value, value => BudgetCategoryId.Create(value))
            .ValueGeneratedNever();

        builder.Property(bc => bc.BudgetId)
            .HasConversion(id => id.Value, value => BudgetId.Create(value))
            .IsRequired();

        builder.Property(bc => bc.Category)
            .HasColumnName("Category")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(bc => bc.Status)
            .HasColumnName("Status")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(bc => bc.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasConversion(date => date.Value, value => CreationDate.Create(value))
            .IsRequired();

        builder.Property(bc => bc.SpentAmount)
           .HasColumnName("SpentAmount")
           .HasConversion(amount => amount.Value, value => Money.Create(value))
           .HasColumnType("decimal(18,2)")
           .IsRequired();

        builder.Property(bc => bc.Limit)
          .HasColumnName("Limit")
          .HasConversion(limit => limit.Value, value => BudgetCategoryLimit.Create(value))
          .HasColumnType("decimal(18,2)")
          .IsRequired();

        builder.HasOne<Budget>()
            .WithMany(b => b.Categories)
            .HasForeignKey(bc => bc.BudgetId)
            .IsRequired();
    }
}