namespace SmartLedger.Common.Contracts.Authorization;

/// <summary>
/// Represents authorization data containing user information.
/// </summary>
public sealed class AuthorizationData : IAuthorizationData
{
    /// <inheritdoc />
    public Guid UserId { get; set; }
}