namespace SmartLedger.Common.Contracts.Authorization;

/// <summary>
/// Interface for authorization data containing user information.
/// </summary>
public interface IAuthorizationData
{
    /// <summary>
    /// Unique identifier of the user.
    /// </summary>
    public Guid? UserId { get; set; }
}