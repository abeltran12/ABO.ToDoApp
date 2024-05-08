using ABO.ToDoApp.Contracts;
using MediatR;

namespace ABO.ToDoApp.Application.Feautures.Identity.Login;

public class LoginUserHandler : IRequestHandler<LoginUserRequest,TokenResponse>
{
    private readonly IAuthService _service;

    public LoginUserHandler(IAuthService service)
    {
        _service = service;
    }

    public async Task<TokenResponse> Handle(LoginUserRequest request, CancellationToken cancellationToken)
    {
        var response = await _service.ValidateUser(request);
        return response;
    }
}
