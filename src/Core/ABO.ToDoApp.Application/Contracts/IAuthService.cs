using ABO.ToDoApp.Application;
using ABO.ToDoApp.Shared.Identity.Models;

namespace ABO.ToDoApp.Contracts;

public interface IAuthService
{
    Task<RegisterUserResponse> RegisterUser(RegisterUserRequest request);
    //Task<Result<bool>> ValidateUser(LoginUserResponse request);
}
