using ABO.ToDoApp.Application.Exceptions;
using ABO.ToDoApp.Infrastructure.Data.DbContexts;
using ABO.ToDoApp.Shared.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ABO.ToDoApp.Infrastructure.Identity.Handlers;

public class OwnerOfTodoListHandler(ToDoAppContext context,
    IHttpContextAccessor httpContextAccessor,
    IdentityConfig identityConfig) : AuthorizationHandler<OwnerOfTodoListRequirement>
{
    private readonly ToDoAppContext _context = context;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IdentityConfig _identityConfig = identityConfig;

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnerOfTodoListRequirement requirement)
    {
        // Get the TodoListId from the route parameters
        var routeData = _httpContextAccessor.HttpContext!.GetRouteData();
        var todoListId = Convert.ToInt32(routeData.Values["todolistId"]);

        // Check if the user is the owner of the TodoList
        var userId = _identityConfig.UserId;
        var todoList = await _context.TodoLists.FindAsync(todoListId);

        if (todoList != null)
        {
            if (todoList.UserId == userId)
                context.Succeed(requirement);
        }
        else
            throw new NotFoundException("TodoList", todoListId);
    }
}