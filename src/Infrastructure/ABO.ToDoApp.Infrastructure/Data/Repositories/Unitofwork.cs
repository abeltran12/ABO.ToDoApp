using ABO.ToDoApp.Domain.Repositories;
using ABO.ToDoApp.Infrastructure.Data.DbContexts;
using ABO.ToDoApp.Shared.Identity.Models;

namespace ABO.ToDoApp.Infrastructure.Data.Repositories;

public class Unitofwork : IUnitofwork
{
    private readonly ToDoAppContext _context;
    private readonly IdentityConfig _identityConfig;
    private readonly Lazy<ITodoListRepository> _todoListRepository;
    private readonly Lazy<ITodoItemRepository> _todoItemRepository;

    public Unitofwork(ToDoAppContext context, IdentityConfig identityConfig)
    {
        _context = context;
        _identityConfig = identityConfig;
        _todoListRepository = new Lazy<ITodoListRepository>(() => new TodoListRepository(_context, _identityConfig));
        _todoItemRepository = new Lazy<ITodoItemRepository>(() => new TodoItemRepository(_context));
    }

    public ITodoListRepository TodoListRepository => _todoListRepository.Value;

    public ITodoItemRepository TodoItemRepository => _todoItemRepository.Value;

    public async Task SaveAsync() => await _context.SaveChangesAsync();

}