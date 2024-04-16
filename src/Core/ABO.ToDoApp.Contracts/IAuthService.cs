using ABO.ToDoApp.Domain.Entities;
using ABO.ToDoApp.Shared.Identity.Models;
using ABO.ToDoApp.Shared.Models.Responses;

namespace ABO.ToDoApp.Contracts;

public interface IAuthService
{
    Task<Result<string>> RegisterUser(User request, string password);
    //Task<Result<bool>> ValidateUser(LoginUserResponse request);
}
