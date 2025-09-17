using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLedger.Common.Infrastructures.DataAccess.Configurators;
using SmartLedger.Modules.Webhooks.Infrastructures.DataAccess.Contexts.Models;

namespace SmartLedger.Modules.Webhooks.Infrastructures.DataAccess.Contexts.Configurations;

/// <summary>
/// Configures the <see cref="WebhookDeliveryAttempt"/> entity.
/// </summary>
public sealed class WebhookDeliveryAttemptConfiguration : IEntityTypeConfiguration<WebhookDeliveryAttempt>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<WebhookDeliveryAttempt> builder)
    {
        builder.HasKey(wda => wda.Id);

        builder.Property(wda => wda.Id)
            .IsGuid();

        builder.Property(wda => wda.WebhookId)
            .IsRequired();

        builder.Property(wda => wda.Payload)
            .IsRequired();

        builder.Property(wda => wda.ResponseStatusCode)
            .IsRequired();

        builder.Property(wda => wda.IsSuccess)
            .IsRequired();

        builder.Property(wda => wda.AttemptedAt)
            .IsDateTime()
            .IsRequired();

        builder.HasOne<Webhook>()
            .WithMany()
            .HasForeignKey(wda => wda.WebhookId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}