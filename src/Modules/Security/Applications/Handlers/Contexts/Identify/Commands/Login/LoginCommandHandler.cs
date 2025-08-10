using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Security.Applications.AppServices.Contexts.Identify.Abstractions;
using SmartLedger.Modules.Security.Applications.AppServices.Contexts.Identify.Models;
using SmartLedger.Modules.Security.Contracts.Responses.Identify;

namespace SmartLedger.Modules.Security.Applications.Handlers.Contexts.Identify.Commands.Login;

/// <summary>
/// Handles the logic for processing <see cref="LoginCommand"/>.
/// </summary>
public sealed class LoginCommandHandler(
    IAuthorizationService authorizationService) : ICommandHandler<LoginCommand, LoginResponse>
{
    /// <inheritdoc />
    public async Task<LoginResponse> HandleAsync(LoginCommand command, CancellationToken cancellationToken)
    {
        var request = new LoginModel(command.Username, command.Password);
        return await authorizationService.AuthorizeAsync(request, cancellationToken);
    }
}