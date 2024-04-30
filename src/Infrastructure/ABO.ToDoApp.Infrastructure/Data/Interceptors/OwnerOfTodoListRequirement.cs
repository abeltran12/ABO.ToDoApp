using Microsoft.AspNetCore.Authorization;

namespace ABO.ToDoApp.Infrastructure.Data.Interceptors;

public class OwnerOfTodoListRequirement : IAuthorizationRequirement
{
}
