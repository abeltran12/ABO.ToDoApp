using Microsoft.AspNetCore.Authorization;

namespace ABO.ToDoApp.Infrastructure.Identity.Handlers;

public class OwnerOfTodoListRequirement : IAuthorizationRequirement
{
}
