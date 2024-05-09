using ABO.ToDoApp.Domain.Entities;
using ABO.ToDoApp.Domain.Models;
using ABO.ToDoApp.Domain.Repositories;
using ABO.ToDoApp.Domain.RequestFilters;
using ABO.ToDoApp.Infrastructure.Data.DbContexts;
using ABO.ToDoApp.Infrastructure.Data.Extensions;
using ABO.ToDoApp.Shared.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace ABO.ToDoApp.Infrastructure.Data.Repositories;

public class TodoListRepository : ITodoListRepository
{
    private readonly ToDoAppContext _context;
    private readonly IdentityConfig _identityConfig;

    public TodoListRepository(ToDoAppContext context, IdentityConfig identityConfig)
    {
        _context = context;
        _identityConfig = identityConfig;
    }

    public async Task<PagedList<TodoListSelect>> GetAll(TodoListParameters parameters)
    {
        var query = _context.TodoLists
                .AsNoTracking()
                .Select(x => new TodoListSelect 
                {
                    Id = x.Id,
                    Name = x.Name,
                    StatusDecription = StatusHelper.GetStatusString(x.Status),
                    Status = x.Status,
                    UserId = x.UserId
                })
                .SearchByName(parameters.Name ?? string.Empty)
                .SearchStatus(parameters.Status)
                .Where(x => x.UserId == _identityConfig.UserId)
                .OrderBy(x => x.Name);

        var todoListSelects = await query
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();

        var count = await query.CountAsync();

        return new PagedList<TodoListSelect>(todoListSelects, count, parameters.PageNumber, parameters.PageSize);
    }

    public async Task<TodoList?> GetByIdAsync(int id)
    {
        return await _context.TodoLists
            .FirstOrDefaultAsync(x => x.Id == id 
            && x.UserId == _identityConfig.UserId);
    }

    public async Task CreateAsync(TodoList todoList)
    {
        await _context.TodoLists.AddAsync(todoList);
    }

    public void Update(TodoList todoList)
    {
        _context.TodoLists.Update(todoList);
    }

}