using MediatR;

namespace ABO.ToDoApp.Application.Feautures.Identity.Login;

public class LoginUserRequest : IRequest<TokenResponse>
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
