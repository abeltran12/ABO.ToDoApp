using ABO.ToDoApp.Domain.Repositories;
using ABO.ToDoApp.Infrastructure.Data.DbContexts;

namespace ABO.ToDoApp.Infrastructure.Data.Repositories;

public class Unitofwork : IUnitofwork
{
    private readonly ToDoAppContext _context;
    private readonly Lazy<ITodoListRepository> _todoListRepository;

    public Unitofwork(ToDoAppContext context)
    {
        _context = context;
        _todoListRepository = new Lazy<ITodoListRepository>(() => new TodoListRepository(_context));
    }

    public ITodoListRepository TodoListRepository => _todoListRepository.Value;

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
