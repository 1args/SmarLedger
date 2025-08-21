using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLedger.Common.Infrastructures.DataAccess.Configurators;
using SmartLedger.Modules.Budgets.Domain.Aggregates;
using SmartLedger.Modules.Budgets.Domain.ValueObjects;

namespace SmartLedger.Modules.Budgets.Infrastructures.DataAccess.Contexts.Write.Configurations;

/// <summary>
/// Configures the <see cref="Budget"/> entity.
/// </summary>
public sealed class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        builder.ToTable("Budgets");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
           .IsGuid()
           .ValueGeneratedOnAdd();

        builder.Property(b => b.UserId)
            .IsRequired();

        builder.Property(b => b.CreatedAt)
            .IsDateTime()
            .IsRequired();

        builder.OwnsOne(b => b.Name, name =>
        {
            name.Property(n => n.Value)
                .HasColumnName("Name")
                .HasMaxLength(BudgetName.MaxLength)
                .IsRequired();
        });

        builder.OwnsOne(b => b.Period, period =>
        {
            period.Property(p => p.StartDate)
                .HasColumnName("StartDate")
                .IsDateTime()
                .IsRequired();

            period.Property(p => p.EndDate)
                .HasColumnName("EndDate")
                .IsDateTime()
                .IsRequired();
        });

        builder.HasMany(b => b.Categories)
            .WithOne()
            .HasForeignKey(bi => bi.BudgetId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}