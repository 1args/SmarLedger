using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLedger.Common.Infrastructures.DataAccess.Configurators;
using SmartLedger.Modules.Budgets.Infrastructures.DataAccess.Contexts.Read.Models;

namespace SmartLedger.Modules.Budgets.Infrastructures.DataAccess.Contexts.Read.Configurations;

/// <summary>
/// Configures the <see cref="BudgetReadModel"/> entity.
/// </summary>
public sealed class BudgetReadModelConfiguration : IEntityTypeConfiguration<BudgetReadModel>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<BudgetReadModel> builder)
    {
        builder.ToTable("Budgets");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .IsGuid()
            .ValueGeneratedNever();

        builder.Property(b => b.UserId)
            .IsRequired();

        builder.Property(b => b.Name)
            .HasMaxLength(PropertyLengthConstants.Length100)
            .IsRequired();

        builder.Property(b => b.StartDate)
            .IsDateTime()
            .IsRequired();

        builder.Property(b => b.EndDate)
            .IsDateTime()
            .IsRequired();

        builder.Property(b => b.CreatedAt)
            .IsDateTime()
            .IsRequired();

        builder.Property(b => b.LastUpdatedAt)
            .IsDateTime()
            .IsRequired();

        builder.HasIndex(b => b.UserId)
            .HasDatabaseName("idx_budgets_userid")
            .IsUnique(false);

        builder.HasIndex(b => new { b.UserId, b.StartDate, b.EndDate })
            .HasDatabaseName("idx_budgets_userid_period")
            .IsUnique(false);
    }
}