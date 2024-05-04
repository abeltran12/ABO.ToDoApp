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

    public async Task<IEnumerable<TodoItemSelect>> GetAll(int idTodoList)
    {
        return
            await _context.TodoItems.AsNoTracking()
            .Select(x => new TodoItemSelect
            {
                Id = x.Id,
                Title = x.Title,
                Status = x.Status,
                StatusDecription = StatusHelper.GetStatusString(x.Status),
                TodoListId = x.TodoListId
            }).Where
                (x => x.TodoListId == idTodoList).ToListAsync();
    }

    public async Task<List<TodoItem>> GetAllForUpdate(int idTodoList)
    {
        return await _context.TodoItems.Where
                (x => x.TodoListId == idTodoList).ToListAsync();
    }

    public async Task<int> GetTodoItemsCount(int idTodoList)
    {
        return await _context.TodoItems
            .Where(x => x.TodoListId == idTodoList).CountAsync();
    }

    public async Task<int> GetTodoItemsCompletedCount(int idTodoList)
    {
        return await _context.TodoItems
            .Where(x => x.TodoListId == idTodoList 
                && x.Status == Status.Completed).CountAsync();
    }

    public async Task<TodoItem> GetByIdAsync(int idTodoList, int id)
    {
        return await _context.TodoItems.Where(x => x.Id == id 
            && x.TodoListId == x.TodoListId).FirstOrDefaultAsync() ?? new TodoItem();
    }

    public async Task CreateAsync(TodoItem item)
    {
        await _context.TodoItems.AddAsync(item);
    }

    public void Update(TodoItem item)
    {
        _context.TodoItems.Update(item);
    }

    public void UpdateAll(List<TodoItem> items)
    {
        _context.TodoItems.UpdateRange(items);
    }
}
