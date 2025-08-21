namespace SmartLedger.Common.Contracts.Options;

/// <summary>
/// Options for configuring RabbitMQ.
/// </summary>
public sealed class RabbitMqOptions
{
    /// <summary>Host name.</summary>
    public required string HostName { get; set; }

    /// <summary>User name.</summary>
    public required string Username { get; set; }

    /// <summary>Password.</summary>
    public required string Password { get; set; }

    /// <summary>Virtual host.</summary>
    public required string VirtualHost { get; set; }
}