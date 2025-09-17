namespace SmartLedger.Modules.Notifications.Contracts.Options;

/// <summary>
/// Represents configuration options for email sending.
/// </summary>
public sealed class EmailSendingOptions
{
    /// <summary>server host.</summary>
    public string Server { get; set; } = string.Empty;

    /// <summary>Port number.</summary>
    public int Port { get; set; }

    /// <summary>Name of the email sender.</summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>Email address of the sender.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Password for the sender's email account.</summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>Indicates whether to use SSL for the connection.</summary>
    public bool UseSsl { get; set; }
}