namespace SmartLedger.Common.Hosts.Migrations.Abstractions;

/// <summary>
/// Represents executing all registered seed implementations.
/// </summary>
public interface ISeedExecutor
{
    /// <summary>
    /// Executes all seed.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task ExecuteAsync(CancellationToken cancellationToken);
}