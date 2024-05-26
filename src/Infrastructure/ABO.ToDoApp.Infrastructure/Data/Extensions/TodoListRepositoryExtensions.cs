using ABO.ToDoApp.Domain.Entities;
using ABO.ToDoApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ABO.ToDoApp.Infrastructure.Data.Extensions;

public static class TodoListRepositoryExtensions
{
    public static IQueryable<TodoListSelect> SearchByName(this IQueryable<TodoListSelect> todoLists, string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return todoLists;

        var lowerCaseTerm = searchTerm.Trim().ToLower();
        return todoLists.Where(t => t.Name.Contains(lowerCaseTerm, StringComparison.CurrentCultureIgnoreCase));
    }

    public static IQueryable<TodoListSelect> SearchStatus(this IQueryable<TodoListSelect> todoLists, Status status)
    {
        return todoLists.Where(e => e.Status == status);
    }
}
