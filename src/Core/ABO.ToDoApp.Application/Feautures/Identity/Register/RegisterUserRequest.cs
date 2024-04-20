using ABO.ToDoApp.Shared.Identity.Models;
using MediatR;

namespace ABO.ToDoApp.Application.Feautures.Identity.Register;

public class RegisterUserRequest : IRequest<RegisterUserResponse>
{
    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;
}
