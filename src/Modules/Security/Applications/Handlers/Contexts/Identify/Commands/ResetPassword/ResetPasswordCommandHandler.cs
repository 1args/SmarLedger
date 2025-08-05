using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Security.Applications.AppServices.Contexts.Identify.Abstractions;
using SmartLedger.Modules.Security.Applications.AppServices.Contexts.Identify.Models;

namespace SmartLedger.Modules.Security.Applications.Handlers.Contexts.Identify.Commands.ResetPassword;

/// <summary>
/// Handles the logic for processing <see cref="ResetPasswordCommand"/>.
/// </summary>
public sealed class ResetPasswordCommandHandler(
    IAuthorizationService authorizationService) : ICommandHandler<ResetPasswordCommand>
{
    /// <inheritdoc />
    public async Task HandleAsync(ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        var request = new ResetPasswordModel(command.CurrentPassword, command.NewPassword);
        await authorizationService.ResetPasswordAsync(request, cancellationToken);
    }
}