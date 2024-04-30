using ABO.ToDoApp.Domain.Entities;
using ABO.ToDoApp.Domain.Models;
using ABO.ToDoApp.Domain.Repositories;
using ABO.ToDoApp.Infrastructure.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace ABO.ToDoApp.Infrastructure.Data.Repositories;

public class TodoItemRepository : ITodoItemRepository
{
    private readonly ToDoAppContext _context;

    public TodoItemRepository(ToDoAppContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TodoItemSelect>> GetAll(int idTodoList, string userId)
    {
        return 
            (IEnumerable<TodoItemSelect>)await _context.TodoItems.Where
                (x => x.TodoListId == idTodoList && x.TodoList!.UserId == userId).ToListAsync();
    }

    public async Task<TodoItem> GetByIdAsync(int id, string userId)
    {
        return await _context.TodoItems.FindAsync(id) ?? new TodoItem();
    }

    public async Task CreateAsync(TodoItem item)
    {
        await _context.TodoItems.AddAsync(item);
    }

    public void Update(TodoItem item)
    {
        _context.TodoItems.Update(item);
    }
}
