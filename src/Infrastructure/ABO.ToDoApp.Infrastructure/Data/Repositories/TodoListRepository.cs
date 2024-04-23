using ABO.ToDoApp.Domain.Entities;
using ABO.ToDoApp.Domain.Repositories;
using ABO.ToDoApp.Infrastructure.Data.DbContexts;

namespace ABO.ToDoApp.Infrastructure.Data.Repositories;

public class TodoListRepository : ITodoListRepository
{
    private readonly ToDoAppContext _context;

    public TodoListRepository(ToDoAppContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(TodoList todoList)
    {
        await _context.TodoLists.AddAsync(todoList);
    }

    public IEnumerable<TodoList> GetAll()
    {
        return _context.TodoLists;
    }

    public Task<TodoList> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<TodoList> GetByNameAsync(string name)
    {
        throw new NotImplementedException();
    }
}
