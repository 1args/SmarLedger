using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLedger.Common.Infrastructure.Configurators;
using SmartLedger.Modules.Budgets.Domain.Aggregates;
using SmartLedger.Modules.Budgets.Domain.Entities;

namespace SmartLedger.Modules.Budgets.Infrastructure.Contexts.Write.Configurations;

/// <summary>
/// Configures the <see cref="BudgetItem"/> entity.
/// </summary>
public sealed class BudgetItemConfiguration : IEntityTypeConfiguration<BudgetItem>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<BudgetItem> builder)
    {
        builder.ToTable("BudgetItems");

        builder.HasKey(bi => bi.Id);

        builder.Property(bi => bi.Id)
            .IsGuid()
            .ValueGeneratedOnAdd();

        builder.Property(bi => bi.BudgetId)
            .IsRequired();

        builder.Property(bi => bi.Category)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(bi => bi.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(bi => bi.CreatedAt)
            .IsDateTime()
            .IsRequired();

        builder.OwnsOne(bi => bi.SpentAmount, amount =>
        {
            amount.Property(a => a.Value)
                .HasColumnName("SpentAmount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();
        });

        builder.OwnsOne(bi => bi.Limit, limit =>
        {
            limit.Property(l => l.Value)
                .HasColumnName("Limit")
                .HasColumnType("decimal(18,2)")
                .IsRequired();
        });

        builder.HasOne<Budget>()
            .WithMany(b => b.Items)
            .HasForeignKey(bi => bi.BudgetId)
            .IsRequired();
    }
}