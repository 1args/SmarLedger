using Microsoft.EntityFrameworkCore;
using SmartLedger.Modules.Webhooks.Infrastructures.DataAccess.Contexts.Models;

namespace SmartLedger.Modules.Webhooks.Infrastructures.DataAccess.Contexts;

/// <summary>
/// Represents the database context for the Webhooks module.
/// </summary>
/// <param name="options">DbContext options.</param>
public sealed class WebhooksDbContext(
    DbContextOptions<WebhooksDbContext> options) : DbContext(options)
{
    /// <summary>Webhooks.</summary>
    public DbSet<Webhook> Webhooks { get; set; }

    /// <summary>Webhook delivery attempts.</summary>
    public DbSet<WebhookDeliveryAttempt> WebhookDeliveryAttempts { get; set; }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        CustomModelBuilder.OnModelCreating(modelBuilder);
    }
}