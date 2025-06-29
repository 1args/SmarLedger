using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace SmartLedger.Common.Infrastructure.Events;

/// <summary>
/// Represents the database context for the outbox pattern.
/// </summary>
/// <param name="options">DbContext options.</param>
public sealed class OutboxDbContext(
    DbContextOptions<OutboxDbContext> options) : DbContext(options)
{
    /// <summary>Outbox messages.</summary>
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    /// <summary>Outbox states.</summary>
    public DbSet<OutboxState> OutboxStates { get; set; }

    /// <summary>Inbox states.</summary>
    public DbSet<InboxState> InboxStates { get; set; }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("outbox");
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
        modelBuilder.AddInboxStateEntity();
    }
}