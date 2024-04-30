using ABO.ToDoApp.Infrastructure.Data.DbContexts;
using ABO.ToDoApp.Shared.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ABO.ToDoApp.Infrastructure.Data.Interceptors;

public class OwnerOfTodoListHandler : AuthorizationHandler<OwnerOfTodoListRequirement>
{
    private readonly ToDoAppContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IdentityConfig _identityConfig;

    public OwnerOfTodoListHandler(ToDoAppContext context, 
        IHttpContextAccessor httpContextAccessor, 
        IdentityConfig identityConfig)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _identityConfig = identityConfig;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnerOfTodoListRequirement requirement)
    {
        // Get the TodoListId from the route parameters
        var routeData = _httpContextAccessor.HttpContext!.GetRouteData();
        var todoListId = Convert.ToInt32(routeData.Values["todolistId"]);

        // Check if the user is the owner of the TodoList
        var userId = _identityConfig.UserId;
        var todoList = await _context.TodoLists.FindAsync(todoListId);

        if (todoList != null && todoList.UserId == userId)
        {
            context.Succeed(requirement);
        }
    }
}