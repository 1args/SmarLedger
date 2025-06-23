using SmartLedger.Common.Domain.Events;

namespace SmartLedger.Common.Contracts.Abstractions;

/// <summary>
/// Represents an integration event intended for communication between different bounded contexts or external systems.
/// </summary>
public interface IIntegrationEvent : IMessage;