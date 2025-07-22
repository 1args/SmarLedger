using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLedger.Common.Infrastructure.Configurators;
using SmartLedger.Modules.Budgets.Domain.Aggregates;
using SmartLedger.Modules.Budgets.Domain.Entities;

namespace SmartLedger.Modules.Budgets.Infrastructure.Contexts.Write.Configurations;

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
            .IsGuid()
            .ValueGeneratedOnAdd();

        builder.Property(bc => bc.BudgetId)
            .IsRequired();

        builder.Property(bc => bc.Category)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(bc => bc.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(bc => bc.CreatedAt)
            .IsDateTime()
            .IsRequired();

        builder.OwnsOne(bc => bc.SpentAmount, amount =>
        {
            amount.Property(a => a.Value)
                .HasColumnName("SpentAmount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();
        });

        builder.OwnsOne(bc => bc.Limit, limit =>
        {
            limit.Property(l => l.Value)
                .HasColumnName("Limit")
                .HasColumnType("decimal(18,2)")
                .IsRequired();
        });

        builder.HasOne<Budget>()
            .WithMany(b => b.Categories)
            .HasForeignKey(bc => bc.BudgetId)
            .IsRequired();
    }
}