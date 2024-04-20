using ABO.ToDoApp.Application.Exceptions;
using ABO.ToDoApp.Application.Feautures.Identity.Register;
using ABO.ToDoApp.Contracts;
using MediatR;

namespace ABO.ToDoApp.Application.Feautures.Identity.Login;

public class LoginUserHandler : IRequestHandler<LoginUserRequest, bool>
{
    private readonly IAuthService _service;

    public LoginUserHandler(IAuthService service)
    {
        _service = service;
    }

    public async Task<bool> Handle(LoginUserRequest request, CancellationToken cancellationToken)
    {
        var validator = new LoginUserValidator();
        var validatorResult = await validator.ValidateAsync(request, cancellationToken);

        if (validatorResult.Errors.Count != 0)
            throw new BadRequestException("Login request invalid.", validatorResult);

        var response = await _service.ValidateUser(request);

        return response;
    }
}
