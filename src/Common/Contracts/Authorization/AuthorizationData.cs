namespace SmartLedger.Common.Contracts.Authorization;

/// <summary>
/// Represents authorization data containing user information.
/// </summary>
/// <param name="userId">User ID.</param>
public sealed class AuthorizationData(Guid userId) : IAuthorizationData
{
    /// <inheritdoc />
    public Guid? UserId { get; set; } = userId;
}