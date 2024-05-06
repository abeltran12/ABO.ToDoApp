using ABO.ToDoApp.Application.Exceptions;
using ABO.ToDoApp.Contracts;
using ABO.ToDoApp.Shared.Constants.GenericMessages;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABO.ToDoApp.Application.Feautures.Identity.Login;

public class LoginUserHandler : IRequestHandler<LoginUserRequest,TokenResponse>
{
    private readonly IAuthService _service;
    private readonly ILogger<LoginUserHandler> _logger;

    public LoginUserHandler(IAuthService service, ILogger<LoginUserHandler> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task<TokenResponse> Handle(LoginUserRequest request, CancellationToken cancellationToken)
    {
        _logger
            .LogInformation(GenericMessageConstants.ExecutingMessage 
                + " {Request}", nameof(LoginUserHandler));

        var validator = new LoginUserValidator();
        var validatorResult = await validator.ValidateAsync(request, cancellationToken);

        if (validatorResult.Errors.Count != 0)
            throw new BadRequestException("Login request invalid.", validatorResult);

        var response = await _service.ValidateUser(request);

        _logger.LogInformation(GenericMessageConstants.ExecutingMessage
                + " {Request} " + GenericMessageConstants.ProcessedMessage, nameof(LoginUserHandler));

        return response;
    }
}
