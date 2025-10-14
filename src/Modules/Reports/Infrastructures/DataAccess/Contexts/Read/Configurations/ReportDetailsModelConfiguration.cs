using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLedger.Common.Infrastructures.DataAccess.Configurators;
using SmartLedger.Modules.Reports.Infrastructures.DataAccess.Contexts.Read.Models;

namespace SmartLedger.Modules.Reports.Infrastructures.DataAccess.Contexts.Read.Configurations;

/// <summary>
/// Configures the <see cref="ReportDetailsModelConfiguration"/> entity.
/// </summary>
public sealed class ReportDetailsModelConfiguration : IEntityTypeConfiguration<ReportDetailsModel>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<ReportDetailsModel> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .IsGuid()
            .ValueGeneratedNever();

        builder.Property(r => r.UserId)
            .IsRequired();

        builder.Property(r => r.Type)
            .HasColumnName("Type")
            .HasMaxLength(PropertyLengthConstants.Length10)
            .IsRequired();

        builder.Property(r => r.Path)
            .HasColumnName("Path")
            .HasMaxLength(PropertyLengthConstants.Length100)
            .IsRequired();

        builder.Property(r => r.GeneratedAt)
            .HasColumnName("GeneratedAt")
            .IsDateTime()
            .IsRequired();
    }
}