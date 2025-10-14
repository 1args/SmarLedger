using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLedger.Common.Domain.ValueObjects;
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
            .HasConversion(id => id.Value, value => BudgetId.Create(value))
            .ValueGeneratedNever();

        builder.Property(b => b.UserId)
             .HasConversion(id => id.Value, value => UserId.Create(value))
             .IsRequired();

        builder.Property(b => b.Name)
           .HasColumnName("Name")
           .HasConversion(name => name.Value, value => BudgetName.Create(value))
           .HasMaxLength(BudgetName.MaxLength)
           .IsRequired();

        builder.ComplexProperty(b => b.Period, period =>
        {
            period.Property(p => p.StartDate)
                .HasColumnName("StartDate")
                .IsRequired();

            period.Property(p => p.EndDate)
                .HasColumnName("EndDate")
                .IsRequired();
        });

        builder.Property(b => b.CreatedAt)
             .HasColumnName("CreatedAt")
             .HasConversion(date => date.Value, value => CreationDate.Create(value))
             .IsRequired();

        builder.HasMany(b => b.Categories)
            .WithOne()
            .HasForeignKey(bi => bi.BudgetId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Navigation(b => b.Categories)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}