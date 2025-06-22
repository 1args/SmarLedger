namespace SmartLedger.Common.Domain.Primitives;

/// <summary>
/// Represents the base class for all aggregate roots.
/// </summary>
/// <typeparam name="TKey">Type of the unique identifier.</typeparam>
public abstract class AggregateRoot<TKey> : Entity<TKey>
    where TKey : struct;