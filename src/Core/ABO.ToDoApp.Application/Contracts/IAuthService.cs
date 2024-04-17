using ABO.ToDoApp.Domain.Entities;
using ABO.ToDoApp.Shared.Identity.Models;

namespace ABO.ToDoApp.Contracts;

public interface IAuthService
{
    Task<RegisterUserResponse> RegisterUser(User user, string password);
    //Task<Result<bool>> ValidateUser(LoginUserResponse request);
}
