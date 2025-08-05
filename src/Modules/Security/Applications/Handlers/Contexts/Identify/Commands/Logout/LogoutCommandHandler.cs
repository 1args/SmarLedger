using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Security.Applications.AppServices.Contexts.Identify.Abstractions;

namespace SmartLedger.Modules.Security.Applications.Handlers.Contexts.Identify.Commands.Logout;

/// <summary>
/// Handles the logic for processing <see cref="LogoutCommand"/>.
/// </summary>
public sealed class LogoutCommandHandler(
    IAuthorizationService authorizationService) : ICommandHandler<LogoutCommand>
{
    /// <inheritdoc />
    public async Task HandleAsync(LogoutCommand command, CancellationToken cancellationToken)
    {
        await authorizationService.LogoutAsync(cancellationToken);
    }
}