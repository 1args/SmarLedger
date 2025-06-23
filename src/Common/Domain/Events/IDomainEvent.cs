namespace SmartLedger.Common.Domain.Events;

/// <summary>
/// Represents a domain event that signifies something important occurred within bounded contexts.
/// </summary>
public interface IDomainEvent : IMessage;