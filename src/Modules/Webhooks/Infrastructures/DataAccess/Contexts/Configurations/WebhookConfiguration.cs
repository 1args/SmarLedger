using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLedger.Common.Infrastructures.DataAccess.Configurators;
using SmartLedger.Modules.Webhooks.Infrastructures.DataAccess.Contexts.Models;

namespace SmartLedger.Modules.Webhooks.Infrastructures.DataAccess.Contexts.Configurations;

/// <summary>
/// Configures the <see cref="Webhook"/> entity.
/// </summary>
public sealed class WebhookConfiguration : IEntityTypeConfiguration<Webhook>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Webhook> builder)
    {
        builder.HasKey(w => w.Id);

        builder.Property(w => w.Id)
            .IsGuid();

        builder.Property(w => w.EventType)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(w => w.CallbackUrl)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(w => w.CreatedAt)
            .IsDateTime()
            .IsRequired();
    }
}