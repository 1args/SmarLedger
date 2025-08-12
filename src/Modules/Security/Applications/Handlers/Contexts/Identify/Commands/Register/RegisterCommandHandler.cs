using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Security.Applications.AppServices.Contexts.Identify.Abstractions;
using SmartLedger.Modules.Security.Applications.AppServices.Contexts.Identify.Models;

namespace SmartLedger.Modules.Security.Applications.Handlers.Contexts.Identify.Commands.Register;

/// <summary>
/// Handles the logic for processing <see cref="RegisterCommand"/>.
/// </summary>
public sealed class RegisterCommandHandler(
    IAuthorizationService authorizationService) : ICommandHandler<RegisterCommand>
{
    /// <inheritdoc />
    public async Task HandleAsync(RegisterCommand command, CancellationToken cancellationToken)
    {
        var request = new UserRegistrationModel(
            command.Username,
            command.Email,
            command.FirstName,
            command.LastName,
            command.Password);

        await authorizationService.RegisterAsync(request, cancellationToken);
    }
}